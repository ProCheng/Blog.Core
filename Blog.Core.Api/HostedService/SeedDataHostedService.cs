using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Blog.Core.Common;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace Blog.Core.Api.HostedService
{
    public sealed class SeedDataHostedService : IHostedService
    {
        private readonly ISqlSugarClient _myContext;

        public SeedDataHostedService(ISqlSugarClient myContext)
        {
            _myContext = myContext;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await DoWork();
        }

        private async Task DoWork()
        {
            try
            {
                Type[] types = Assembly
                .Load ("Blog.Core.Model")//如果 .dll报错，可以换成 xxx.exe 有些生成的是exe 
                .GetTypes().Where(f => f.FullName.Contains(".Entity") && !f.FullName.Contains(".RootTkey")) //命名空间过滤，当然你也可以写其他条件过滤
                .ToArray();//断点调试一下是不是需要的Type，不是需要的在进行过滤

                _myContext.DbMaintenance.CreateDatabase();
                _myContext.CodeFirst.InitTables(types);//根据types创建表


            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }


}

