using Castle.DynamicProxy;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Api.AOP
{
    public class LogAOP : BaseAOP
    {
        public override void Intercept(IInvocation invocation)
        {
            MiniProfiler.Current.Step($"执行Service方法：{invocation.Method.Name}() -> ");

            Console.WriteLine("开始执行："+ invocation.InvocationTarget.GetType().FullName+"-"+invocation.Method.Name);
            invocation.Proceed();
            Console.WriteLine("执行完毕：" + invocation.InvocationTarget.GetType().FullName + "-" + invocation.Method.Name);

        }
    }
}
