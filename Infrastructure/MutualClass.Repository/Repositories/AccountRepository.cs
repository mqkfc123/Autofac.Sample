using Dragon.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dragon.Domain.Model;
using Dapper;
using Dragon.Core.MySql.Dapper;

namespace Dragon.Repository.Repositories
{
    //public class AccountRepository : IAccountRepository
    //{
    //    private readonly IDapperRepositoryContext _dpContext;

    //    public AccountRepository(IRepositoryContext context)
    //    {
    //        _dpContext = context as IDapperRepositoryContext;
    //    }

    //    /// <summary>
    //    /// 新增账户
    //    /// </summary>
    //    /// <param name="entity"></param>
    //    public void AddAccount(Dragon_Account entity)
    //    {
    //        try
    //        {
    //            string cmd = @"INSERT INTO Dragon_account (
    //                            AccountId,
    //                            Level,
    //                            AccountType,
    //                            TenantType,
    //                            LoginId,
    //                            Pwd,
    //                            ParentId,
    //                            State,
    //                            StartTime,
    //                            EndTime,
    //                            CreateTime,
    //                            ModifyTime,
    //                            IsDelete  
    //                        )VALUES (
    //                            ?AccountId,
    //                            ?Level,
    //                            ?AccountType,
    //                            ?TenantType,
    //                            ?LoginId,
    //                            ?Pwd,
    //                            ?ParentId,
    //                            ?State,
    //                            ?StartTime,
    //                            ?EndTime,
    //                            ?CreateTime,
    //                            ?ModifyTime,
    //                            ?IsDelete  );";

    //            string cmd2 = @"INSERT INTO Dragon_accountinfo (
    //                            Id,
    //                            AccountId,
    //                            Price, 
    //                            PromotePrice,
    //                            BlockedPrice,
    //                            RealName,
    //                            AccountName,
    //                            NickName,
    //                            FaceImg,
    //                            Phone,
    //                            ProvinceId,
    //                            ProvinceName,
    //                            CityId,
    //                            CityName,
    //                            AreaId,
    //                            AreaName,
    //                            Remark,
    //                            CreateUser,
    //                            ModifyUser,
    //                            WorkTime,
    //                            Address,
    //                            BusinessLcense,
    //                            Thumbnails,
    //                            IsDelete,
    //                            Sex,
    //                            CreateTime,
    //                            ModifyTime,
    //                            Longitude,
    //                            Latitude,
    //                            MerchantName,
    //                            AgentPrice
    //                        )VALUES (
    //                            ?Id,
    //                            ?AccountId,
    //                            ?Price,
    //                            ?PromotePrice,
    //                            ?BlockedPrice,
    //                            ?RealName,
    //                            ?AccountName,
    //                            ?NickName,
    //                            ?FaceImg,
    //                            ?Phone,
    //                            ?ProvinceId,
    //                            ?ProvinceName,
    //                            ?CityId,
    //                            ?CityName,
    //                            ?AreaId,
    //                            ?AreaName,
    //                            ?Remark,
    //                            ?CreateUser,
    //                            ?ModifyUser,
    //                            ?WorkTime,
    //                            ?Address,
    //                            ?BusinessLcense,
    //                            ?Thumbnails,
    //                            ?IsDelete,
    //                            ?Sex,
    //                            ?CreateTime,
    //                            ?ModifyTime,
    //                            ?Longitude,
    //                            ?Latitude,
    //                            ?MerchantName,
    //                            ?AgentPrice );";

    //            _dpContext.Context.Execute(cmd, entity);
    //            _dpContext.Context.Execute(cmd2, entity.AccountInfo);
    //        }
    //        catch (Exception ex)
    //        {
    //            _dpContext.Rollback();
    //            throw new NotImplementedException(ex.Message);
    //        }

    //    }

    //}


}
