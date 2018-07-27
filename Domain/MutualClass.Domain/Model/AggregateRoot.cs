using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dragon.Domain.Model
{
    /// <summary>
    /// 表示聚合根类型的基类型。
    /// </summary>
    public abstract class AggregateRoot : IAggregateRoot
    {
        #region Protected Fields 
        #endregion

        #region Public Methods
      
        #endregion

        #region IEntity Members
        /// <summary>
        /// 获取当前领域实体类的全局唯一标识。
        /// </summary>
        //public virtual Guid ID { get; set; }

        #endregion
    }
}
