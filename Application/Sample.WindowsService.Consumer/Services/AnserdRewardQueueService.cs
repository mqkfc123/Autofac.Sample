using DQueue;
using DQueue.Event;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using MutualClass.WindowsService.Consumer.Model;
using Dragon.Core.Log4net;
using Dragon.Domain.IRepositories;
using Dragon.Infrastructure.Services;

namespace MutualClass.WindowsService.Consumer.Services
{

    /// <summary>
    /// 有奖问卷
    /// </summary>
    public class MutualClassRewardQueueService
    {
        private static readonly ILog _Logger = LogHelper.GetLogger(typeof(MutualClassRewardQueueService));

        private readonly IRepositoryContext _repositoryContext;
        private readonly IMemoryEventBusService _memoryEventBusService;
        public MutualClassRewardQueueService(IRepositoryContext repositoryContext,
            IMemoryEventBusService memoryEventBusService)
        {
            _repositoryContext = repositoryContext;
            _memoryEventBusService = memoryEventBusService;
        }


        public void InitMessRabbitMQ()
        {
            try
            {
                var thread = new ThreadStart(() =>
                {
                    using (var rabRead = (Rabbitmq)MQFactory.CreateMessageQueue(MQFactory.MQType.RabbitMQ))
                    {
                        rabRead.QueueIP = ConfigurationManager.AppSettings["QueueUrl"];
                        rabRead.QueueName = "MutualClass_Reward_Queue";  //队列名称
                        rabRead.VirtualHost = "5672";   //本地端口  外网端口15672
                        rabRead.ExchangeName = "RewardExchangeName"; //交换机
                        rabRead.UserName = "zxsj";
                        rabRead.Password = "zxsj";
                        rabRead.AutoAck = false;
                        rabRead.onReceive += imq_onReceive;
                        rabRead.RType = Rabbitmq.TypeName.Direct;

                        rabRead.Init();
                        rabRead.SubscribeQueue();
                        rabRead.AddListening();
                        rabRead.ReceiveMQMessage();
                        //  结束时使用
                        rabRead.IsReceOver = true;

                    }
                });
                thread.BeginInvoke(null, null);
            }
            catch (Exception ex)
            {
                _Logger.Error($"有奖问卷 MutualClassRewardJob 订阅初始化异常", ex);
            }

        }

        private void imq_onReceive(object src, ReceiveEventArgs e)
        {
            //var mm = e.MessageObj; 980904a14a5411e78d88704d7b6ab057
            //_Logger.Info("imq_onReceive接收奖励问卷/问题参数：" + Convert.ToString(e.MessageObj));

            var time = Stopwatch.StartNew();
            try
            {
               
                
            }
            catch (Exception ex)
            {
                _repositoryContext.Rollback();
                _Logger.Error($"MutualClassRewardQueueService有奖问卷/问题 imq_onReceive messObj:{e.MessageObj} ,Customer异常：", ex);
            }
            finally
            {
                time.Stop();
                if (time.ElapsedMilliseconds > 500)
                    _Logger.Info("imq_onReceive接收奖励问卷/问题参数：" + Convert.ToString(e.MessageObj) + " time:" + time.ElapsedMilliseconds);
            }
        }
        
 
        ///// <summary>
        ///// 是否命中红包
        ///// </summary>
        ///// <param name="probabilityArray"></param>
        ///// <returns></returns>
        //private void Probability(IEnumerable<MutualClass_Bonus> probabilityArray)
        //{
        //    var random = new Random();
        //    //var randomNumber = random.NextDouble();
        //    //randomNumber = Math.Round(randomNumber, 15); //保留15位小数， 更精确
        //    var randomNumber = random.Next(0, 1000);
        //    int[] array = new int[1000];  //概率区间
        //    var index = 0;
        //    for (var el = 0; el < probabilityArray.Count(); el++)
        //    {
        //        var item = probabilityArray.ToArray()[el];
        //        for (var i = 0; i < (item.Probability * 10); i++)
        //        {
        //            array[index] = el;
        //            index++;
        //        }
        //        #region
        //        //1-1000 
        //        //var isHit = _probabilityService.IsHit(item.Probability, random.NextDouble);
        //        //if (ishit)
        //        //{
        //        //    return item;
        //        //}
        //        //else
        //        //{
        //        //    continue;
        //        //}
        //        //var probabilityPercentage = item.Probability / 100;
        //        //if (probabilityPercentage > randomNumber)
        //        //{
        //        //    return item;
        //        //}
        //        #endregion 
        //    }
        //    var hitNum = array[randomNumber];
        //    return probabilityArray.ToArray()[hitNum];
        //}
         

    }

}
