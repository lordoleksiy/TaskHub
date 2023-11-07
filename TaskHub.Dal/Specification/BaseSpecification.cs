using System.Linq.Expressions;
using TaskHub.Dal.Interfaces;

namespace TaskHub.Dal.Specification
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        protected BaseSpecification()
        {}
        public Expression<Func<T, bool>> Criteria { get; protected set; }
        public List<Expression<Func<T, object>>> Includes { get; protected set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; protected set; }

        public int Take { get; protected set; }
        public int Skip { get; protected set; }
        public bool IsPagingEnabled { get; protected set; } = false;
        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }


        protected virtual void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }

        protected virtual void ApplyOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
    }
}
