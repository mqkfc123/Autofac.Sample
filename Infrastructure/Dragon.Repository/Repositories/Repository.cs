using Dragon.Domain;
using Dragon.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Repository.Repositories
{

    public abstract class Repository<TAggregateRoot> : IRepository<TAggregateRoot> where TAggregateRoot : class, IAggregateRoot
    {
        #region Private Fields
        private readonly IRepositoryContext _context;
        #endregion
         
        public Repository(IRepositoryContext context)
        {
            _context = context;
        }

        public IRepositoryContext Context
        {
            get { return _context; }
        }

        #region Protected Methods
        
        #endregion 

    }
}
