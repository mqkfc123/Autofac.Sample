
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sample.Management.Web.Models
{
    public class UserInfo
    {
        public string AccountParentId { get; set; }
        public string AccountId { get; set; } 

        public string JxShopsId { get; set; }

        public string ShopsId { get; set; } 

        /// <summary>
        /// 登入账户
        /// </summary>
        public string LoginId { get; set; }

        
        /// <summary>
        /// 别称
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 推广费用
        /// </summary>
        public int PromotePrice { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public IEnumerable<UserPower> PowerData { get; set; }

        /// <summary>
        /// 获取指定角色下所有ID集合
        /// </summary>
        public string AllAccountIds { get; set; }

        /// <summary>
        /// 获取指定角色下与自己平级的所有ID集合
        /// </summary>
        public string LateralAccountIds { get; set; }


        public static UserInfo CreateUserInfo()
        {
            return new UserInfo();
        }

    }

    /// <summary>
    /// 用户权限
    /// </summary>
    public class UserPower
    {
        public string PowerId { get; set; }
        /// <summary>
        /// 权限类型 1菜单 2操作
        /// </summary>
        public int PowerType { get; set; }

        public string PowerName { get; set; }

        public string PowerCode { get; set; }

        public string ParentId { get; set; }

        public string Url { get; set; }

        public string Icon { get; set; }
        public string IconVice { get; set; }

        public int Sort { get; set; }

    }

 

}