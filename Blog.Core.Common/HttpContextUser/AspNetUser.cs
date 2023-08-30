using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace Blog.Core.Common.HttpContextUser
{
    /// <summary>
    /// http中的 用户信息 根据token来确认的
    /// </summary>
    public class AspNetUser: IAspNetUser
    {
        private readonly IHttpContextAccessor _accessor;

        public AspNetUser(IHttpContextAccessor accessor)
        {
            _accessor = accessor;

            // 是否有身份? 没有就看有没有token，有就解析token，token也没有，说明无身份

            if (!IsAuthenticated())
            {
                Generate();
            }
        }

        /// <summary>
        /// 判断是否有认证过 （认证后就有身份信息）
        /// </summary>
        /// <returns></returns>
        public bool IsAuthenticated()
        {
            return _accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        /// <summary>
        /// 根据token生成身份
        /// </summary>
        private void Generate()
        {
            var token = _accessor.HttpContext?.Request?.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (!token.IsNullOrEmpty())
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                // token校验
                if (jwtHandler.CanReadToken(token))
                {
                    var jwtToken = jwtHandler.ReadJwtToken(token);

                    var claims = new ClaimsIdentity(jwtToken.Claims);
                    _accessor.HttpContext.User.AddIdentity(claims);
                }               
            }
        }
        public string Name => _accessor.HttpContext.User.Identity.Name;

        public string ID => _accessor.HttpContext.User.Claims.FirstOrDefault(i=>i.Type== JwtRegisteredClaimNames.Jti)?.Value;

    }


    /// <summary>
    /// http上下文中 用户信息的接口
    /// </summary>
    public interface IAspNetUser
    {
        string Name { get; }
        string ID { get; }
        bool IsAuthenticated();

    }
}