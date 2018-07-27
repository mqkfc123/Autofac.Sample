using Dragon.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Repository
{
    public interface IDapperRepositoryContext : IRepositoryContext
    {
        #region Properties
        /// <summary>
        /// 获取当前仓储上下文所使用的Dapper的实例。
        /// </summary>
        IDbConnection Context { get; set; }


        IDbTransaction Trans { get; set; }

        #endregion

    }
}
