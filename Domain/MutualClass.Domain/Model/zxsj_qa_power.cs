using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surevy.Domain.Model
{
    public class ZXSJ_QA_Power
    {
        public string PowerId { get; set; }
        /// <summary>
        /// 权限类型 1菜单 2操作
        /// </summary>
        public string PowerType { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string PowerName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PowerCode { get; set; }
        public string ParentId { get; set; }
        public string Url { get; set; }
        /// <summary>
        ///
        /// </summary>
        public string Icon { get; set; }

        public string IconVice { get; set; }
        /// <summary>
        ///  1启用 0禁用
        /// </summary>
        public int State { get; set; }
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 1正常 0删除
        /// </summary>
        public int IsDelete { get; set; }

        public int Sort { get; set; }

    }

}