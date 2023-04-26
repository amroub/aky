namespace aky.Foundation.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IRepository<Entity>
        where Entity : class
    {
        Task<IEnumerable<Entity>> AddRangeAsync(IEnumerable<Entity> entities);

        Task<Entity> AddAsync(Entity entity);

        Task<int> CountAsync(Expression<Func<Entity, bool>> filter = null);

        Task<int> DeleteAsync(Entity entity);

        void Dispose();

        Task<ICollection<Entity>> FindAllAsync(Expression<Func<Entity, bool>> match, IIncludes<Entity> includes = null);

        Task<Entity> FindAsync(Expression<Func<Entity, bool>> match, IIncludes<Entity> includes = null);

        Task<ICollection<Entity>> GetAllAsync(IIncludes<Entity> includes = null);

        [Obsolete("GetAllIncludingAsync is deprecated, please use GetAllAsync instead.")]
        Task<ICollection<Entity>> GetAllIncludingAsync(params Expression<Func<Entity, object>>[] includeProperties);

        Task<Entity> GetAsync(int id);

        Task<Entity> UpdateAsync(Entity t, object key);
    }
}
