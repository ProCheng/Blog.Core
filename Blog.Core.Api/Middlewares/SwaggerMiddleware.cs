using Blog.Core.Common.Helper;
using log4net;
using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using static Blog.Core.ServiceExtensions.CustomApiVersion;

namespace Blog.Core.Middlewares
{
    /// <summary>
    /// Swagger中间件
    /// </summary>
    public static class SwaggerMiddleware
    {
        /// <summary>
        /// 开启 Swagger中间件
        /// </summary>
        /// <param name="app"></param>
        public static void UseSwaggerMiddle(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                //根据版本名称倒序 遍历展示
                var apiName = AppSettingsHelper.app(new string[] { "ApiName" });
                typeof(ApiVersions).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version => { c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{apiName} {version}"); });

                // 路径配置，设置为空，表示直接在根域名（localhost:8001）访问该文件,注意localhost:8001/swagger是访问不到的，去launchSettings.json把launchUrl去掉，如果你想换一个路径，直接写名字即可，比如直接写c.RoutePrefix = "doc";
                c.RoutePrefix = "";
                c.HeadContent = @"<script async='async' 
	                id='mini-profiler' 
	                src='/profiler/includes.min.js?v=4.2.22+4563a9e1ab' 
	                data-version='4.2.22+4563a9e1ab' 
	                data-path='/profiler/' 
	                data-current-id='f6b88311-015a-44ed-bde0-cdab4c2d0d9b' 
	                data-ids='ac204d24-e2df-486e-a135-8167336643b7,f6b88311-015a-44ed-bde0-cdab4c2d0d9b' 
	                data-position='Left' ' 
	                data-scheme='Light' 
	                data-authorized='true' 
	                data-max-traces='15' 
	                data-toggle-shortcut='Alt+P' 
	                data-trivial-milliseconds='2.0' 
	                data-ignored-duplicate-execute-types='Open,OpenAsync,Close,CloseAsync'></script>";

            });
        }
    }
}