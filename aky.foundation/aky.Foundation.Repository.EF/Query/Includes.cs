namespace aky.Foundation.Repository.EF.Query
{
    using System;
    using System.Linq;

    public class Includes<T> : IIncludes<T>
    {
        public Includes()
        {
        }

        public Includes(Func<IQueryable<T>, IQueryable<T>> expression)
        {
            this.Expression = expression;
        }

        public Func<IQueryable<T>, IQueryable<T>> Expression { get; private set; }

    }
}
