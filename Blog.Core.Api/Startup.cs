using Autofac;
using Autofac.Extras.DynamicProxy;
using Blog.Core.Api.Policy;
using Blog.Core.Api.ServiceExtensions;
using Blog.Core.IServices;
using Blog.Core.Middlewares;
using Blog.Core.ServiceExtensions;
using log4net.Config;
using log4net;
using log4net.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Blog.Core.Api.Filter;
using Blog.Core.Common.Helper;
using AspNetCoreRateLimit;
using Blog.Core.Common.Log;
using Blog.Core.Api.AOP;
using Blog.Core.Api.Hubs;
using Blog.Core.Extensions;
using Blog.Core.Api.HostedService;

namespace Blog.Core
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;  // 生成配置信息的类
         
        }



        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(i=> {                               // 开启控制器服务


                i.Filters.Add(typeof(GlobalExceptionsFilter));          // 捕获全局异常过滤器

            });

            services.AddSingleton(new AppSettingsHelper(Configuration));    // 配置文件
            services.AddSingleton<ILoggerHelper, LogHelper>();              // 日志

            services.AddSqlsugarSetup();                                    // Sqlsugar 服务
            services.AddSwaggerSetup();                                     // swagger服务
            services.AddAuthorizationSetup();                               // 授权服务
         
            services.AddHttpContextSetup();                                 // http管道相关的一些注册

            services.AddAutoMapper(Assembly.Load("Blog.Core.Model"));       // 注入 AutoMapper

            services.AddCorsSetup();                                        // 注入Cors服务

            services.AddIpPolicyRateLimitSetup(Configuration);              // 注入限流服务
                
            services.AddMiniProfilerSetup();                                // 注入性能监控服务

            services.AddSignalR().AddNewtonsoftJsonProtocol(i => {
                var settings = new Newtonsoft.Json.JsonSerializerSettings();
                settings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();

                i.PayloadSerializerSettings = settings;
            });  // 添加signalR服务，添加扩展包解决序列化数据问题

            services.AddInitializationHostServiceSetup();    // 程序初始化
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseIpRateLimiting();                // 开启限流中间件

            if (env.IsDevelopment())
            {   // 是开发模式
                app.UseDeveloperExceptionPage();    // 显示错误页面
            }
            app.UseStaticFiles();       // 静态文件
            app.UseSwaggerMiddle();     // swagger 中间件

            app.UseCors("default");     // cors
            app.UseRouting();           // 开启路由中间件 解析url，http生成路由对象
            app.UseAuthentication();    // 认证中间件
            app.UseAuthorization();     // 授权中间件

            app.UseMiniProfiler();      // 性能监控


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // 自动匹配对应控制器的方法

                endpoints.MapHub<ChatHub>("/chat");  // signalR 地址
            });



        }



        /// <summary>
        /// 注入服务 使用的是 autofac
        /// </summary>
        /// <param name="builder"></param>
        public void ConfigureContainer(ContainerBuilder builder) {

            //直接注册某一个类和接口
            //左边的是实现类，右边的As是接口
           // builder.RegisterType<UserServices>().As<IUserServices>();

            var servicesDllFile1 = Path.Combine(AppContext.BaseDirectory, "Blog.Core.Services.dll");//获取注入项目绝对路径
            var assemblysServices1 = Assembly.LoadFile(servicesDllFile1);//直接采用加载文件的方法

            var servicesDllFile2 = Path.Combine(AppContext.BaseDirectory, "Blog.Core.Repository.dll");//获取注入项目绝对路径
            var assemblysServices2 = Assembly.LoadFile(servicesDllFile2);//直接采用加载文件的方法
            
            builder.RegisterType<LogAOP>();


            builder.RegisterAssemblyTypes(assemblysServices2, assemblysServices1)
              .AsImplementedInterfaces()
              .InstancePerLifetimeScope()
              .EnableInterfaceInterceptors()
              .InterceptedBy(typeof(LogAOP));
              

        }
    }






}
