using Dragon.Core.MySql.Dapper;
using Dapper;
using Dragon.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Dragon.Domain;
using Dragon.Domain.IRepositories;

namespace Dragon.Repository
{
    public class DapperRepository<TAggregateRoot> : Repository<TAggregateRoot>
        where TAggregateRoot : class, IAggregateRoot
    {
        private readonly IDapperRepositoryContext  _dpContext;
        public DapperRepository(IRepositoryContext context) : base(context)
        {
            _dpContext = context as IDapperRepositoryContext;
        }
      
    }


}
