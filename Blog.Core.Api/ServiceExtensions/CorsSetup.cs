using Blog.Core.Common;
using Blog.Core.Common.Helper;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Blog.Core.ServiceExtensions
{
    /// <summary>
    /// Cors 启动服务
    /// </summary>
    public static class CorsSetup
    {
        public static void AddCorsSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddCors(c =>
            {
                    c.AddPolicy("default", policy =>
                        {

                            policy
                            .WithOrigins(AppSettingsHelper.app(new string[] { "Cors", "IPs" }).Split(','))
                            .AllowAnyHeader()//Ensures that the policy allows any header.
                            .AllowAnyMethod()
                            .AllowCredentials();
                        });

            });
        }
    }
}
