
using Dragon.Core.Log4net;
using DQueue;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Domain
{

    public class RabbitMQClient
    {
        private static readonly ILog _Logger = LogHelper.GetLogger(typeof(RabbitMQClient));
        private static Dictionary<string, IMessageQueue> LstMqs = new Dictionary<string, IMessageQueue>();
        private static readonly object Obj = new object();
        /// <summary>
        /// 实例化队列对象
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        private static IMessageQueue CreateInstance(string queueName, string exchange)
        {
            IMessageQueue iQueue = null;
            if (!LstMqs.ContainsKey(queueName))
            {
                lock (Obj)
                {
                    if (LstMqs.ContainsKey(queueName))
                    {
                        iQueue = LstMqs[queueName];
                        return iQueue;
                    }
                    var mq = (Rabbitmq)MQFactory.CreateMessageQueue(MQFactory.MQType.RabbitMQ);
                    mq.QueueIP = ConfigurationManager.AppSettings["QueueUrl"];
                    mq.VirtualHost = "5672";
                    mq.QueueName = queueName;
                    mq.ExchangeName = exchange;
                    mq.AutoAck = false; 
                    mq.UserName = "zxsj";
                    mq.Password = "zxsj";
                    mq.RType = Rabbitmq.TypeName.Direct;

                    mq.Init();
                    iQueue = mq;
                    if (!LstMqs.ContainsKey(queueName))
                    {
                        LstMqs.Add(queueName, iQueue);
                    }
                }
            }
            iQueue = LstMqs[queueName];
            return iQueue;
        }

        /// <summary>
        /// 放入队列
        /// </summary>
        /// <param name="dicString"></param>
        /// <param name="queueName"></param>
        public static void InsertQueue(string dicString, string queueName,string exchange)
        {
            try
            {
                CreateInstance(queueName, exchange).SendMQMessage(dicString);
            }
            catch (Exception ex)
            {
                _Logger.Error($"{queueName} 入队列异常：",ex);
            }
        }

    }


}
