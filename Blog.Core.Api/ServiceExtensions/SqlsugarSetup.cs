using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System;
using Blog.Core.Common.Helper;

namespace Blog.Core.Extensions
{
    /// <summary>
    /// SqlSugar 启动服务
    /// </summary>
    public static class SqlsugarSetup
	{
		private static readonly MemoryCache Cache = new MemoryCache(new MemoryCacheOptions());

		public static void AddSqlsugarSetup(this IServiceCollection services)
		{
			if (services == null) throw new ArgumentNullException(nameof(services));

			
			
			// SqlSugarScope是线程安全，可使用单例注入
			// 参考：https://www.donet5.com/Home/Doc?typeId=1181
			services.AddSingleton<ISqlSugarClient>(s =>
			{
                SqlSugarScope sqlSugar = new SqlSugarScope(new ConnectionConfig()
				{
					DbType = SqlSugar.DbType.SqlServer,
					ConnectionString = AppSettingsHelper.app(new string[] { "DBS", "SqlServer" }),
					IsAutoCloseConnection = true,
				},
				   db =>
				   {
					   //单例参数配置，所有上下文生效

					   db.Aop.OnLogExecuting = (sql, pars) =>
					   {
						   Console.WriteLine("sql：" + sql);
						   //获取IOC对象不要求在一个上下文
						   //vra log = s.GetService<Log>()

						  // 获取IOC对象要求在一个上下文
						   //var appServive = s.GetService<IHttpContextAccessor>();
						   //var log = appServive?.HttpContext?.RequestServices.GetService<Log>();
					   };
				   });
				return sqlSugar;



			});
		}

		private static string GetWholeSql(SugarParameter[] paramArr, string sql)
		{
			foreach (var param in paramArr)
			{
				sql.Replace(param.ParameterName, param.Value.ToString());
			}

			return sql;
		}

		private static string GetParas(SugarParameter[] pars)
		{
			string key = "【SQL参数】：";
			foreach (var param in pars)
			{
				key += $"{param.ParameterName}:{param.Value}\n";
			}

			return key;
		}
	}
}