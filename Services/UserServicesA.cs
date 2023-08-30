using Blog.Core.IRepository;
using Blog.Core.IServices;
using Blog.Core.Model.Entity.Sys;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Core.Services
{
    public class UserServicesA : IUserServices
    {
        public IUserRepository dal;
        public UserServicesA(IUserRepository userRepository)
        {
            dal = userRepository;
        }
         
        public IEnumerable<SysUser> GetUserAll()
        {
            var res = dal.GetUser().Select((item,i)=> {

                return new SysUser() { 
                    Name = item.Name+i+"A",
                };
            });
            return res;
        }
    }
}
