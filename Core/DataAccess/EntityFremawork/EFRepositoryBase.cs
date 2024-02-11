using Core.DataAccess;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.EntityFremawork
{
    public class EFRepositoryBase<TEntity, TContext> : IRepositoryBase<TEntity>
        where TEntity : class, IEntity
        where TContext : DbContext, new()
    {
        public void Add(TEntity tentity)
        {
            using var context = new TContext();
            var addEntity = context.Entry(tentity);
            addEntity.State = EntityState.Added;
            context.SaveChanges();
        }

        public void Delete(TEntity tentity)
        {
            using var context = new TContext();
            var deleteEntity = context.Entry(tentity);
            deleteEntity.State = EntityState.Deleted;
            context.SaveChanges();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> expression)
        {
            using var context = new TContext();

            return context.Set<TEntity>().SingleOrDefault(expression);
        }

        public List<TEntity> GetAll(Expression<Func<TEntity, bool>>? expression = null)
        {
            using var context = new TContext();

            return expression == null ?
                context.Set<TEntity>().ToList() :
                context.Set<TEntity>().Where(expression).ToList();

        }

        public void Update(TEntity tentity)
        {
            using var context = new TContext();
            var updateEntity = context.Entry(tentity);
            updateEntity.State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
