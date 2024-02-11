using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess
{
    public interface IRepositoryBase<TEntity>
        where TEntity : class, IEntity
    {
        void Add(TEntity tentity);
        void Update(TEntity Tentity);
        void Delete(TEntity tentity);
        TEntity Get(Expression<Func<TEntity, bool>> expression);
        List<TEntity> GetAll(Expression<Func<TEntity, bool>>? expression = null);

    }   
}
