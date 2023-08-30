using Blog.Core.Api.Policy;
using Blog.Core.Common.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.ServiceExtensions
{
    /// <summary>
    /// 系统 授权 服务
    /// </summary>
    public static class AuthorizationSetup
    {
        /// <summary>
        /// 添加系统授权服务
        /// </summary>
        /// <param name="services"></param>
        public static void AddAuthorizationSetup(this IServiceCollection services)
        {

            #region 配置认证服务

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettingsHelper.app(new string[] { "Jwt", "JwtSecret" })));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 令牌验证参数
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = "Audience：Issuer",//发行人
                ValidateAudience = true,
                ValidAudience = "Audience：Audience",//订阅人
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(0),    // 偏移时间
            };

            //core自带官方JWT认证
            // 开启Bearer认证
            services.AddAuthentication("Bearer")

             // 添加JwtBearer服务
             .AddJwtBearer(o =>
             {
                 o.TokenValidationParameters = tokenValidationParameters;
                 o.Events = new JwtBearerEvents
                 {
                     OnAuthenticationFailed = context =>
                     {
                         // 如果过期，则把<是否过期>添加到，返回头信息中
                         if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                         {
                             context.Response.Headers.Add("Token-Expired", "true");
                         }
                         return Task.CompletedTask;
                     },
                     // 这里是核心 signalR 通过令牌的方式，识别指定的某个用户，并实现相互之间的通讯。
                     OnMessageReceived = context =>
                     {
                         var accessToken = context.Request.Query["access_token"];

                         // If the request is for our hub...
                         var path = context.HttpContext.Request.Path;
                         if (!string.IsNullOrEmpty(accessToken) &&
                             (path.StartsWithSegments("/chat")))
                         {
                             // Read the token out of the query string
                             context.Token = accessToken;
                         }
                         return Task.CompletedTask;
                     },
                 };
             });


            // 自定义授权策略
            services.AddAuthorization(o => {

                //o.AddPolicy("AdminOrUser", o => o.RequireRole("Admin").RequireRole("User") );
                //o.AddPolicy("AdminAndUser", o => o.RequireRole("Admin", "User"));
                o.AddPolicy("自定义策略认证", o => {
                    o.Requirements.Add(new MyAuthorizationRequirement());
                });
            });
            #endregion
        }
    }

}