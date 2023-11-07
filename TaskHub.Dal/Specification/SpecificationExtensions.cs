using TaskHub.Dal.Entities;
using TaskHub.Dal.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TaskHub.Dal.Specification
{
    public static class SpecificationExtensions
    {
        public static IQueryable<TEntity> Specify<TEntity>(this IQueryable<TEntity> inputQuery, ISpecification<TEntity> specification) where TEntity : class
        {
            var query = inputQuery;

            if (specification == null)
            {
                return query;
            }

            query = ApplyCriteria(query, specification);
            query = ApplyIncludes(query, specification);
            query = ApplyOrderBy(query, specification);
            query = ApplyPaging(query, specification);

            return query;
        }

        private static IQueryable<TEntity> ApplyCriteria<TEntity>(IQueryable<TEntity> query, ISpecification<TEntity> specification)
        {
            return specification.Criteria != null ? query.Where(specification.Criteria) : query;
        }

        private static IQueryable<TEntity> ApplyIncludes<TEntity>(IQueryable<TEntity> query, ISpecification<TEntity> specification) where TEntity : class
        {
            foreach (var includeExpression in specification.Includes)
            {
                query = query.Include(includeExpression);
            }
            return query;
        }

        private static IQueryable<TEntity> ApplyOrderBy<TEntity>(IQueryable<TEntity> query, ISpecification<TEntity> specification)
        {
            return specification.OrderBy != null ? query.OrderBy(specification.OrderBy) : query;
        }

        private static IQueryable<TEntity> ApplyPaging<TEntity>(IQueryable<TEntity> query, ISpecification<TEntity> specification)
        {
            return specification.IsPagingEnabled? query.Skip(specification.Skip).Take(specification.Take) : query;
        }
    }
}
