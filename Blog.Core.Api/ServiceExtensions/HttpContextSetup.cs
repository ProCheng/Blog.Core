using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System;
using Blog.Core.Api.Policy;
using Microsoft.AspNetCore.Authorization;
using Blog.Core.Common.HttpContextUser;

namespace Blog.Core.Api.ServiceExtensions
{
    /// <summary>
    /// HttpContext 相关服务
    /// </summary>
    public static class HttpContextSetup
    {
        /// <summary>
        /// 添加 HttpContext 相关服务 授权需要User信息
        /// </summary>
        /// <param name="services"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddHttpContextSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            // 授权处理
            services.AddSingleton<IAuthorizationHandler, MyAuthorizationHander>();

            // 获取http请求上下文的信息
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // 授权认证后的 用户信息
            services.AddScoped<IAspNetUser, AspNetUser>();
        }
    }
}
