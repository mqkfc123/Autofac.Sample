using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surevy.Domain.Model
{
    public class ZXSJ_QA_CompanyProfitsAssign
    {
        public string Id { get; set; }

        /// <summary>
        /// 金额（如1000） 单位 分
        /// </summary>
        public int CompanyPrice { get; set; }

        /// <summary>
        /// 折扣 0表示不打折
        /// </summary>
        public int Discount { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 开户类型
        /// </summary>
        public EnumLevel Level { get; set; }

        /// <summary>
        /// 条件公式  1:1:1:0:0:0
        /// </summary>
        public string ConditionFormulas { get; set; }
        /// <summary>
        /// 分配公式 10:0:0:0
        /// </summary>
        public string ProportionFormulas { get; set; }

        public int IsDelete { get; set; }

        public DateTime CreateTime { get; set; }

        public string CretaeUser { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

    }
}
