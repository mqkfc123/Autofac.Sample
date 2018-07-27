using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surevy.Domain.Model
{
    /// <summary>
    /// 角色
    /// </summary>
    public class ZXSJ_QA_Role
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        /// <summary>
        /// 1启用 0禁用
        /// </summary>
        public int State { get; set; }
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 1正常 0删除
        /// </summary>
        public int IsDelete { get; set; }

        /// <summary>
        /// 角色归属
        /// 账户等级 1系统，2城市代理商，3 区县代理商，4折扣代理商，5企业
        /// </summary>     
        public EnumLevel RoleLevel { get; set; }
         

        /// <summary>
        /// 是否用户角色
        /// </summary>
        public virtual bool IsAccountRole { get; set; }

        /// <summary> 
        /// 角色权限列表
        /// </summary>
        public virtual IEnumerable<ZXSJ_QA_RolePowerRelation> RolePowerItem { get; set; }

    }
}
