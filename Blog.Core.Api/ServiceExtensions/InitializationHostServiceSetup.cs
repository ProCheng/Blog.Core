using System;
using System.Linq;
using System.Reflection;
using Blog.Core.Common.Helper;
using Blog.Core.ServiceExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SqlSugar;

namespace Blog.Core.Api.HostedService
{
    public static class InitializationHostServiceSetup
    {
        /// <summary>
        /// 应用初始化服务注入
        /// </summary>
        /// <param name="services"></param>
        public static void AddInitializationHostServiceSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddHostedService<SeedDataHostedService>(); // 初始化数据库

           


        }
    }
}

