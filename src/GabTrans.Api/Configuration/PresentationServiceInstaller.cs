using GabTrans.Api.Filters;
using GabTrans.Domain.Constants;
using GabTrans.Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Polly;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.RateLimiting;

namespace GabTrans.Api.Configuration
{
    public class PresentationServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication();
            services.AddAuthorization();
            services.AddControllersWithViews();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GabTrans API", Version = "v2" });
                // c.OperationFilter<CustomHeaderFilter>();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
                });
            });

            //services.AddCors(option =>
            //{
            //    option.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyHeader()
            //    .AllowAnyMethod());
            //});

            services.AddCors(options =>
            {
                options.AddPolicy("SecurePolicy", policy =>
                {
                    policy.WithOrigins("https://localhost:3000", "https://10.0.0.1:3000")
                          .WithMethods("GET", "POST","PUT")
                           .WithHeaders("Content-Type", "x-custom-header")
                          .WithHeaders("Content-Type", "Authorization");
                });
            });

            services.AddRazorPages();

            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.AddFixedWindowLimiter("PasswordOtpLimiter", policy =>
                {
                    policy.PermitLimit = 5; // Max 5 requests
                    policy.Window = TimeSpan.FromMinutes(1); // Per minute
                    policy.QueueLimit = 0; // Reject excess requests
                    policy.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                });

                options.AddPolicy(RateLimitPolicyNames.Otp, httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                   partitionKey: httpContext.User.Identity?.Name ?? httpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                   factory: _ => new FixedWindowRateLimiterOptions
                   {
                       PermitLimit = 3,
                       QueueLimit = 0,
                       Window = TimeSpan.FromMinutes(5),
                       QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                   }));

                options.AddPolicy(RateLimitPolicyNames.Concurrency, httpContext =>
                RateLimitPartition.GetConcurrencyLimiter(
                partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                factory: _ => new ConcurrencyLimiterOptions
                {
                    PermitLimit = 5,
                    QueueLimit = 2,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                }));
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = StaticData.JwtIssuer,
                        ValidAudience = StaticData.JwtIssuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(StaticData.Jwtkey)),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });

            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 10 * 1024 * 1024; // 10MB
            });

        }
    }
}
