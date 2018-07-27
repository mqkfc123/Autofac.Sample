using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surevy.Domain.Model
{
    /// <summary>
    /// 用户提交的问卷答案表
    /// </summary>
    public class ZXSJ_QA_UserAnswer
    {
        public string UserAnswerId { get; set; }

        /// <summary>
        /// 问卷ID
        /// </summary>
        public string QuesAireMainId { get; set; }

        /// <summary>
        /// 用户提交的问卷 记录ID
        /// </summary> 
        public string UserQuesRecordId { get; set; }
        /// <summary>
        /// 问题ID
        /// </summary>
        public string QuesId { get; set; }
        /// <summary>
        /// 答案	在选项表内则调用对应ID，反之如单行或多行则直接写入文本内容
        /// </summary>
        public string Answer { get; set; }
        /// <summary>
        /// 1正常 0删除
        /// </summary>
        public int IsDelete { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

    }
}
