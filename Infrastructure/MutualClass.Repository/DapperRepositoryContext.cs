using Dragon.Domain.IRepositories;
using Dragon.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dragon.Repository
{
    public class DapperRepositoryContext : RepositoryContext, IDapperRepositoryContext
    {

        //private readonly ThreadLocal<BaseRepository> localCtx = new ThreadLocal<BaseRepository>(() => new BaseRepository());

        public IDbConnection Context
        {
            get; set;
        }

        public IDbTransaction Trans
        {
            get; set;
        }


        public DapperRepositoryContext() {
            Context = BaseRepository.GetConnect();
            Trans = Context.BeginTransaction();
        }

        public override bool DistributedTransactionSupported
        {
            get { return false; }
        }

        public override void Rollback()
        {
            Trans.Rollback();
            Context.Close();
            Context.Dispose();
            Committed = true;
        }

        public override void RegisterNewTrans()
        {
            if (Committed)
            {
                Context.Open();
                Trans = Context.BeginTransaction();
                Committed = false;
            }
        }

        protected override void DoCommit()
        {
            if (!Committed)
            {
                Trans.Commit();
                Context.Close();
                Context.Dispose();
                Committed = true;
            }
        }
         
    }
}
