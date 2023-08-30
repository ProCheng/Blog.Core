using Blog.Core.Model.Entity;
using System;
using System.Collections.Generic;

namespace Blog.Core.IRepository
{
    public interface IUserRepository
    {
        IEnumerable<SysUser> GetUser();
    }
}
