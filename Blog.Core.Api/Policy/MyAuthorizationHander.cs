using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace Blog.Core.Api.Policy
{
    /// <summary>
    /// jwt授权 策略
    /// </summary>
    public class MyAuthorizationHander : AuthorizationHandler<MyAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MyAuthorizationRequirement requirement)
        {
            // 这里以后加入逻辑，是否有授权访问
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// 授权验证参数
    /// </summary>
    public class MyAuthorizationRequirement : IAuthorizationRequirement
    {
        public string Name { get; set; }
    }
}
