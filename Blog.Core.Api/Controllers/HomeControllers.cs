using Blog.Core.Common.Helper;
using Blog.Core.Api.ServiceExtensions;
using Blog.Core.RouterAttribute;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Blog.Core.Common.HttpContextUser;
using Blog.Core.IServices;
using Autofac.Features.AttributeFilters;
using AutoMapper;
using Blog.Core.Model.Entity;
using Blog.Core.Model.DTO;
using System.Runtime.InteropServices;
using SqlSugar;
using System.Threading;
using Blog.Core.Model.Entity.Sys;

namespace Blog.Core.Controllers
{
    /// <summary>
    /// 主页控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class HomeControllers : ControllerBase
    {

        readonly IAspNetUser _User = null;
        private readonly IUserServices userServices;
        private readonly IMapper mapper;
        private readonly ISqlSugarClient DBContext;

        public HomeControllers(IAspNetUser user,IUserServices userServices,IMapper mapper, ISqlSugarClient context)
        {
             
            this.userServices = userServices;
            this.mapper = mapper;
            this.DBContext = context;
        }


        /// <summary>
        /// 这是一个注释
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomRoute]
        [Authorize("自定义策略认证")]
        public IActionResult GetUser()
        {
            //var _user = new SysUser() {
            //     Name = "张三",
            //     Age = "22",
            //     Sex = "男",
            //     UserInfo = new SysUserInfo() { 
            //        Addr = "广东"
            //     }
            //};
            // 对象映射
            var userDto = mapper.Map<UserDTO>(null);


            return new JsonResult(userServices.GetUserAll());
        }

        /// <summary>
        /// 测试 SqlSugar
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomRoute]
        public IActionResult GetTest() {

            //建表
            //db.CodeFirst.InitTables<Student>(); 更多看文档迁移   

            DBContext.AsTenant().BeginTran();       // 多住户事物
            DBContext.AsTenant().RollbackTran();    // 回滚

            //查询表的所有
            DBContext.Queryable<SysUser>().ToList();

            //插入
            DBContext.Insertable(new SysUser() { 
               
            }).ExecuteCommand();

            //更新
            DBContext.Updateable(new SysUser() { 
             
            }).ExecuteCommand();

            //删除
            DBContext.Deleteable<SysUser>().Where(it => it.UserId == 2).ExecuteCommand();


            return new JsonResult(new { 
                msg = "ok"
            });
        }

     
    }
}
