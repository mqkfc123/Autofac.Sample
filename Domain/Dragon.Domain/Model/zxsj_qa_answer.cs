using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surevy.Domain.Model
{
    public class ZXSJ_QA_Answer
    {
        public string AnswerId { get; set; }
        /// <summary>
        /// 所属问卷主体
        /// </summary>
        public string QuesAireMainId { get; set; }
        /// <summary>
        /// 答案	
        /// 如：[{“问题类型”:”” , “问题ID”:”” , “答案”:”在选项表内则调用对应ID，反之如单行或多行则直接写入文本内容”}]
        /// </summary>
        public string AnswerJson { get; set; }
        /// <summary>
        /// 1正常 0删除
        /// </summary>
        public int IsDelete { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
