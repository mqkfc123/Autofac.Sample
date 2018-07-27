using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MutualClass.WindowsService.Consumer.Model
{
    public class RewardQueueModel
    {
        /// <summary>
        /// 问卷ID
        /// </summary>
        public string QuesAireMainId { get; set; }

        public string UserId { get; set; }

        /// <summary>
        /// 用户 提交问卷记录ID
        /// </summary>
        public string UserQuesRecordId { get; set; }

    }
}
