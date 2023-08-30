using System;
using System.ComponentModel.DataAnnotations;
using Blog.Core.Model.Entity.RootTkey;
using SqlSugar;

namespace Blog.Core.Model.Entity.Sys
{
    /// <summary>
    /// 用户表
    /// </summary> 
    public class SysUser: UserRoot<long>
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// 出生日期
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime Birth { get; set; } = DateTime.Now;

        /// <summary>
        /// 性别 0：男，1：女
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public GenderEnum Gender { get; set; } = GenderEnum.Man;

        [Navigate(NavigateType.OneToOne, nameof(UserId))]
        public SysUserInfo UserInfo { get; set; }


        /// <summary>
        /// 性别
        /// </summary>
        public enum GenderEnum
        {
            Man, Woman
        }

    }
   
}
