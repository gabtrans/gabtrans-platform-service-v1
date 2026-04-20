using Azure.Core;
using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.Abstractions.Notification;
using GabTrans.Application.Abstractions.Repositories;
using GabTrans.Application.Abstractions.Services;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Enums;
using GabTrans.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace GabTrans.Application.Services
{
    public class SecurityService(ILogService logService, IFileService fileService, IAuditService auditService, IUserRepository userRepository, IAuditRepository auditRepository, IEmailNotificationService emailService, ILoginRepository loginRepository, IPasswordService securityService, ISignUpRepository signUpRepository, ISignInRepository signInRepository, IPasswordService passwordService, IValidationService validationService, IOneTimePasswordService oneTimeService) : ISecurityService
    {
        private readonly ILogService _logService = logService;
        private readonly IFileService _fileService = fileService;
        private readonly IAuditService _auditService = auditService;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IAuditRepository _auditRepository = auditRepository;
        private readonly IEmailNotificationService _emailService = emailService;
        private readonly ILoginRepository _loginRepository = loginRepository;
        private readonly ISignUpRepository _signUpRepository = signUpRepository;
        private readonly IPasswordService _securityService = securityService;
        private readonly IPasswordService _passwordService = passwordService;
        private readonly ISignInRepository _signInRepository = signInRepository;
        private readonly IValidationService _validationService = validationService;
        private readonly IOneTimePasswordService _oneTimeService = oneTimeService;

        public async Task<ApiResponse> SignInAsync(SignInRequest request, string browser, string ipAddress)
        {
            var user = await _userRepository.GetDetailsByUserEmailAsync(request.EmailAddress);
            if (user is null)
            {
                return new ApiResponse
                {
                    Message = "Incorrect EmailAddress"
                };
            }

            if (string.Equals(user.Status, UserStatuses.Blocked, StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = "Profile is locked, Please contact the administrator"
                };
            }

            if (!string.Equals(user.Status, UserStatuses.Active, StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = "Your profile is not active"
                };
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            {
                return new ApiResponse
                {
                    Message = "Incorrect Password"
                };
            }

            var otp = await _oneTimeService.GenerateAsync(user.Id, (long)OTPCategories.Login);
            if (!otp.Success)
            {
                return new ApiResponse
                {
                    Message = "Kindly try again later"
                };
            }

            await _emailService.AuthorizationAsync(user.FirstName, user.EmailAddress, otp.Data.ToString());

            await _auditService.InsertAsync(user.Id, ModuleActions.Sign_In, browser, ipAddress);

            return new ApiResponse { Success = true, Message = "Kindly authenticate the OTP sent to your email", Data = user.Id };
        }

        public async Task<bool> ClearLoginAttemptAsync(long userId)
        {
            var login = await _loginRepository.DetailsAsync(userId);
            if (login is null) return false;

            return await _loginRepository.DeleteAsync(userId);
        }

        public async Task<ApiResponse> GenerateTokenAsync(User user, string browser, string ipAddress)
        {
            var userRole = await _signUpRepository.GetUserRoleAsync(user.Id);
            if (userRole is null)
            {
                return new ApiResponse
                {
                    Message = "Unable to get role"
                };
            }

            var claims = new List<Claim>
            {
                    new Claim(JwtRegisteredClaimNames.Nickname, $"{user.FirstName} {user.LastName}"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, user.EmailAddress),
                    new Claim(ClaimTypes.System, "web"),
                    new Claim(ClaimTypes.Role, userRole.RoleId.ToString()),
                    new Claim(ClaimTypes.Actor, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(StaticData.Jwtkey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(StaticData.JwtExpireMinutes));

            var token = new JwtSecurityToken(StaticData.JwtIssuer, StaticData.JwtIssuer, claims, expires: expires, signingCredentials: credentials);

            string jwtTokenString = new JwtSecurityTokenHandler().WriteToken(token);

            string refreshToken = GenerateRefreshToken();

            var refreshTokenExpiry = DateTime.Now.AddMinutes(Convert.ToDouble(StaticData.RefreshTokenExpireMinutes));
            var loginToken = new LoginModel
            {
                RefreshToken = refreshToken,
                UserId = user.Id,
                RefreshTokenExpiryTime = refreshTokenExpiry,
                SessionToken = jwtTokenString,
                Expires = expires,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                Status = LoginStatuses.Login
            };

            bool save = await _loginRepository.SaveAsync(loginToken, ipAddress);
            if (!save)
            {
                return new ApiResponse
                {
                    Message = "Unable to generate session token"
                };
            }

            user.LastLogin = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            await _userRepository.UpdateAsync(user);

            await _auditService.InsertAsync(user.Id, ModuleActions.Generate_Token, browser, ipAddress);

            return new ApiResponse
            {
                Success = true,
                Message = "Token generated successfully",
                Data = loginToken
            };
        }

        public async Task<ApiResponse> RefreshAsync(string accessToken, string refreshToken, string browser, string ipAddress)
        {
            var accessDetails = await _loginRepository.ValidateTokenAsync(accessToken, refreshToken);
            if (accessDetails is null)
            {
                return new ApiResponse
                {
                    Message = "session token details not found"
                };
            }

            if (string.Equals(accessDetails.Status, LoginStatuses.Logout, StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResponse
                {
                    Message = "The session token is not valid"
                };
            }

            var user = await _userRepository.GetDetailsByUserIdAsync(accessDetails.UserId);
            if (user is null)
            {
                return new ApiResponse
                {
                    Message = "Unable to fetch user profile"
                };
            }

            var userRole = await _signUpRepository.GetUserRoleAsync(user.Id);
            if (userRole is null)
            {
                return new ApiResponse
                {
                    Message = "Unable to get role"
                };
            }

            var claims = new List<Claim>
            {
                    new Claim(JwtRegisteredClaimNames.Nickname, $"{user.FirstName} {user.LastName}"),
                   //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, user.EmailAddress.ToString()),
                    new Claim(ClaimTypes.System, "web"),
                    new Claim(ClaimTypes.Role, userRole.RoleId.ToString()),
                    new Claim(ClaimTypes.Actor, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(StaticData.Jwtkey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(StaticData.JwtExpireMinutes));

            var token = new JwtSecurityToken(StaticData.JwtIssuer, StaticData.JwtIssuer, claims, expires: expires, signingCredentials: credentials);

            string jwtTokenString = new JwtSecurityTokenHandler().WriteToken(token);

            refreshToken = GenerateRefreshToken();

            var refreshTokenExpiry = DateTime.Now.AddMinutes(Convert.ToDouble(StaticData.RefreshTokenExpireMinutes));
            var loginToken = new LoginModel
            {
                RefreshToken = refreshToken,
                UserId = user.Id,
                RefreshTokenExpiryTime = refreshTokenExpiry,
                SessionToken = jwtTokenString,
                Expires = expires,
                UpdatedAt = DateTime.Now,
                CreatedAt = DateTime.Now,
                Status = LoginStatuses.Login
            };

            bool save = await _loginRepository.SaveAsync(loginToken, ipAddress);
            if (!save)
            {
                return new ApiResponse
                {
                    Message = "Unable to generate session token"
                };
            }

            await _auditService.InsertAsync(user.Id, ModuleActions.Refresh_Token, browser, ipAddress);

            return new ApiResponse
            {
                Success = true,
                Message = "Token successfully refreshed",
                Data = loginToken
            };
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public bool ValidateCurrentToken(string token)
        {
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(StaticData.Jwtkey));

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidIssuer = StaticData.JwtIssuer,
                    IssuerSigningKey = mySecurityKey
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public string GetTerminalIdClaim(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            var serialClaimValue = securityToken.Claims.First(claim => claim.Type == ClaimTypes.SerialNumber).Value;

            return serialClaimValue;
        }

        public string GetBusinessIdFromClaim(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                var serialClaimValue = securityToken.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;

                return serialClaimValue;
            }
            catch (Exception e)
            {
                _logService.LogError("BaseService", "GetBusinessIdFromClaim", e);
            }

            return string.Empty;
        }

        public string GetSecretKey(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            return securityToken.Claims.First(claim => claim.Type == ClaimTypes.Sid).Value;
        }

        public int GetAccountId(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            var claimValue = securityToken.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;
            return !string.IsNullOrEmpty(claimValue) ? Convert.ToInt16(claimValue) : 0;
        }

        public string GetChannel(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            var claimValue = securityToken.Claims.First(claim => claim.Type == ClaimTypes.System).Value;
            return claimValue;
        }

        public long GetUserId(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            var claimValue = securityToken.Claims.First(claim => claim.Type == ClaimTypes.Actor).Value;
            return !string.IsNullOrEmpty(claimValue) ? Convert.ToInt64(claimValue) : 0;
        }

        public async Task<ApiResponse> SignOutAsync(long userId)
        {
            var login = await _loginRepository.DetailsAsync(userId);
            if (login is null)
            {
                return new ApiResponse
                {
                    Message = "session token details not found"
                };
            }

            login.Status = LoginStatuses.Logout;
            bool update = await _loginRepository.UpdateAsync(login);
            if (!update)
            {
                return new ApiResponse
                {
                    Message = "Unable to logout"
                };
            }

            return new ApiResponse { Success = true, Message = "Session successfully logout" };
        }
    }
}
