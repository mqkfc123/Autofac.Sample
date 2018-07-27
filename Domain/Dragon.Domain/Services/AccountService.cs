using Dragon.Core;
using Dragon.Domain.Event;
using Dragon.Domain.IRepositories;
using Dragon.Domain.Model; 
using Dragon.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Domain.Services
{

    //public interface IAccountService : IDependency
    //{

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    /// <param name="accountId"></param>
    //    /// <returns></returns>
    //    void GetAccountInfoDetial(string accountId);
    //}


    //public class AccountService : IAccountService
    //{
    //    private readonly IRepositoryContext _repositoryContext;
    //    private readonly IAccountRepository _accountRepository;
 
    //    private readonly IMemoryEventBusService _memoryEventBusService;
    //    public AccountService(IRepositoryContext repositoryContext,
    //        IAccountRepository accountRepository,
    //        IMemoryEventBusService memoryEventBusService)
    //    {
    //        _repositoryContext = repositoryContext;
    //        _accountRepository = accountRepository;
    //        _memoryEventBusService = memoryEventBusService;
    //    }

    //    /// <summary>
    //    /// 
    //    /// </summary> 
    //    /// <returns></returns>
    //    public void GetAccountInfoDetial(string accountId)
    //    {
    //        //_memoryEventBusService.PublishObtainCounpon("CC593BB7E060449988D6A972FB16348A", "F991D012889D4D0E9D346B890517C1A8", "77433A05F0C1419AB2E676F5F0AAAC16 ");
    //       // var model = _accountRepository.SignInAccount(loginId, pwd);
        

    //        _repositoryContext.Commit(); 
    //    }

    //}

}
