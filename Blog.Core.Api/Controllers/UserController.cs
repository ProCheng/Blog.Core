using Blog.Core.Api.Hubs;
using Blog.Core.Common.Helper;
using Blog.Core.Model.Entity;
using Blog.Core.RouterAttribute;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.AspNetCore.SignalR;

namespace Blog.Core.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /// <summary>
        /// 左边是hub中心，右边是客户端接口，官网称强类型
        /// </summary>
        private readonly IHubContext<ChatHub, IChatClient> hubContext;

        public UserController(IHubContext<ChatHub, IChatClient> hubContext)
        {
            // hub中心，可以给客户端信息
            this.hubContext = hubContext;

        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [CustomRoute(Core.ServiceExtensions.CustomApiVersion.ApiVersions.V1)]
        [HttpPost]
        public IActionResult LoginIn([FromBody] SysUser user) {


            if(ChatHub._connections.Any(i => i.Name == user.Name))
            {
                return BadRequest();
            }
            else
            {
                var res = JwtHelper.IssueJwt(new TokenModelJwt()
                {
                    UID = Math.Abs(user.Name.GetHashCode()),
                    Name = user.Name,
                });
                return new JsonResult(new
                {
                    msg = "登录成功",
                    res = res
                });
            }
           
        }

        [CustomRoute]
        [HttpGet]
        public IActionResult LoginOut() {

            return Ok();
        }
    }
}
