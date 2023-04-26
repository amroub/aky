namespace aky.Foundation.Utility
{
    using System.Linq;
    using System.Linq.Expressions;

    public static class ParameterReplacer
    {
        public static Expression<T> Replace<T>(Expression<T> expression, ParameterExpression source, ParameterExpression target)
        {
            return ParameterReplacer.Replace<T, T>(expression, source, target);
        }

        public static Expression<TOutput> Replace<TInput, TOutput>(Expression<TInput> expression, ParameterExpression source, ParameterExpression target)
        {
            return new ParameterReplacer.ParameterReplacerVisitor<TOutput>(source, target)
                .VisitAndConvert(expression);
        }

        private class ParameterReplacerVisitor<TOutput> : ExpressionVisitor
        {
            private readonly ParameterExpression _source;

            private readonly ParameterExpression _target;

            public ParameterReplacerVisitor(ParameterExpression source, ParameterExpression target)
            {
                this._source = source;
                this._target = target;
            }

            internal Expression<TOutput> VisitAndConvert<T>(Expression<T> root)
            {
                return (Expression<TOutput>)this.VisitLambda(root);
            }

            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                // Leave all parameters alone except the one we want to replace.
                var parameters = node.Parameters.Select(p => p == this._source ? this._target : p);

                return Expression.Lambda<TOutput>(this.Visit(node.Body), parameters);
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                // Replace the source with the target, visit other params as usual.
                return node == this._source ? this._target : base.VisitParameter(node);
            }
        }
    }
}
