namespace aky.Foundation.Repository.EF
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public abstract class AbstractRepository<Entity> : IRepository<Entity>
        where Entity : class
    {
        protected DbContext context;

        public virtual async Task<ICollection<Entity>> GetAllAsync(IIncludes<Entity> includes = null)
        {
            return await this.Select(null, null, includes?.Expression, null, null).ToListAsync();
        }

        public virtual async Task<Entity> GetAsync(int id)
        {
            return await this.context.Set<Entity>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<Entity>> AddRangeAsync(IEnumerable<Entity> entities)
        {
            await this.context.Set<Entity>().AddRangeAsync(entities);
            await this.context.SaveChangesAsync();
            return entities;
        }

        public virtual async Task<Entity> AddAsync(Entity entity)
        {
            this.context.Set<Entity>().Add(entity);
            await this.context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<Entity> FindAsync(Expression<Func<Entity, bool>> match, IIncludes<Entity> includes = null)
        {
            return await this.Select(match, null, includes?.Expression, null, null).SingleOrDefaultAsync();
        }

        public async Task<ICollection<Entity>> FindAllAsync(Expression<Func<Entity, bool>> match, IIncludes<Entity> includes = null)
        {
            return await this.Select(match, null, includes?.Expression, null, null).ToListAsync();
        }

        public virtual async Task<int> DeleteAsync(Entity entity)
        {
            this.context.Set<Entity>().Remove(entity);
            return await this.context.SaveChangesAsync();
        }

        public virtual async Task<Entity> UpdateAsync(Entity t, object key)
        {
            if (t == null)
            {
                return null;
            }

            Entity exist = await this.context.Set<Entity>().FindAsync(key);
            if (exist != null)
            {
                this.context.Entry(exist).CurrentValues.SetValues(t);
                await this.context.SaveChangesAsync();
            }

            return exist;
        }

        public async Task<int> CountAsync(Expression<Func<Entity, bool>> filter = null)
        {
            return await this.Select(filter, null, null, null, null).CountAsync();
        }

        public async Task<ICollection<Entity>> GetAllIncludingAsync(params Expression<Func<Entity, object>>[] includeProperties)
        {
            IQueryable<Entity> queryable = this.Queryable();
            foreach (Expression<Func<Entity, object>> includeProperty in includeProperties)
            {
                queryable = queryable.Include<Entity, object>(includeProperty);
            }

            return await queryable.ToListAsync();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal IQueryable<Entity> Select(
            Expression<Func<Entity, bool>> filter = null,
            Func<IQueryable<Entity>, IOrderedQueryable<Entity>> orderBy = null,
            Func<IQueryable<Entity>, IQueryable<Entity>> includes = null,
            int? page = null,
            int? pageSize = null)
        {
            IQueryable<Entity> query = this.context.Set<Entity>();

            if (includes != null)
            {
                query = includes(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return query;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.context.Dispose();
                }

                this.disposed = true;
            }
        }

        protected IQueryable<Entity> Queryable()
        {
            return this.context.Set<Entity>().AsQueryable<Entity>();
        }
    }
}
