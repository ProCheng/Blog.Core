using AutoMapper;
using Blog.Core.Model.DTO;
using Blog.Core.Model.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Core.Model.AutoMapper
{
    /// <summary>
    /// AutoMapper配置映射关系
    /// </summary>
    public class CustomProfile:Profile
    {
        public CustomProfile()
        {
            CreateMap<SysUser, UserDTO>()
                .ForMember(i=> i.Addr,o=> o.MapFrom(j=> j.UserInfo.Addr));  // 地址映射

            CreateMap<UserDTO, SysUser>();
        }
    }
}
