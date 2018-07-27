
using Dragon.Core.Log4net;
using Quartz;
using System;
using System.Threading.Tasks;

namespace MutualClass.WindowsService.Consumer.Job
{
 
    public class MutualClassJob: IJob
    {  
        private readonly ILog _Logger = LogHelper.GetLogger(typeof(MutualClassJob));
      
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Task.Run(() =>
                {
                    //_MutualClassJobService.ExecuteService();
                });
            }
            catch (Exception ex)
            {
                _Logger.Error("MutualClassJob Execute异常：", ex);
            }
        }
 
    }

}
