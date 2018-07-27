using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surevy.Domain.Model
{
    public class ZXSJ_QA_AgentProfitsAssign
    {
        public string Id { get; set; }
        /// <summary>
        /// 代理费类型 1城市 2 区域 3 折扣
        /// </summary>
        public EnumAgentType AgentType { get; set; }
        /// <summary>
        /// 代理费金额 单位：分
        /// </summary>
        public int AgentPrice { get; set; }
        public string ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public string CityId { get; set; }
        public string CityName { get; set; }
        public string AreaId { get; set; }
        public string AreaName { get; set; }
        public int IsDelete { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateUser { get; set; }
        public DateTime ModifyTime { get; set; }
        public string ModifyUser { get; set; }
    }

    /// <summary>
    /// 代理费类型
    /// </summary>
    public enum EnumAgentType
    {
        /// <summary>
        /// 2城市代理商
        /// </summary>
        CityProxy = 1,
        /// <summary>
        /// 区县代理商
        /// </summary>
        AreaProxy = 2,
        /// <summary>
        /// 折扣代理商
        /// </summary>
        DiscountProxy = 3 
    }

}
