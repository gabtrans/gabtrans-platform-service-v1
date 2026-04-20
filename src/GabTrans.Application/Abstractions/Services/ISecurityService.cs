using GabTrans.Application.Abstractions.Logging;
using GabTrans.Application.DataTransfer;
using GabTrans.Domain.Entities;
using GabTrans.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GabTrans.Application.Abstractions.Services
{
    public interface ISecurityService
    {
        Task<bool> ClearLoginAttemptAsync(long userId);
        Task<ApiResponse> SignInAsync(SignInRequest request, string browser, string ipAddress);
        Task<ApiResponse> SignOutAsync(long userId);
        Task<ApiResponse> GenerateTokenAsync(User user, string browser, string ipAddress);
        Task<ApiResponse> RefreshAsync(string accessToken, string refreshToken, string browser, string ipAddress);
        string GenerateRefreshToken();
        bool ValidateCurrentToken(string token);
        string GetTerminalIdClaim(string token);
        string GetBusinessIdFromClaim(string token);
        string GetSecretKey(string token);
        int GetAccountId(string token);
        string GetChannel(string token);
        long GetUserId(string token);
    }
}
