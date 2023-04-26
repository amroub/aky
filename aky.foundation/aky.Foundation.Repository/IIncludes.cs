namespace aky.Foundation.Repository
{
    using System;
    using System.Linq;

    public interface IIncludes<T>
    {
        Func<IQueryable<T>, IQueryable<T>> Expression { get; }
    }
}
