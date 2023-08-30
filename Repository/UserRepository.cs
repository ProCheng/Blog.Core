using Blog.Core;
using Blog.Core.IRepository;
using Blog.Core.Model;
using Blog.Core.Model.Entity;
using System;
using System.Collections.Generic;

namespace Blog.Core.Repository
{
    public class UserRepository : IUserRepository
    {
        public IEnumerable<SysUser> GetUser()
        {
            return new List<SysUser>() {
                new SysUser(){ 
                    Name = "张三"
                },
                new SysUser()
                {
                    Name = "李四"
                }
            };
        }
    }
}
