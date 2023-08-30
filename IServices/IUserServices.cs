using Blog.Core.Model.Entity.Sys;
using System;
using System.Collections.Generic;

namespace Blog.Core.IServices
{
    public interface IUserServices
    {
        public IEnumerable<SysUser> GetUserAll();
    }
}
