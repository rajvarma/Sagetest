using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;


namespace Sage.Core.Framework.Storage
{
    public static class OrderByHelper
    {
        /// <summary>
        /// Apply order by clauses to an IEnumerable
        /// </summary>
        /// <typeparam name="T">The IEnumerable type</typeparam>
        /// <param name="enumerable">The IEnumerable</param>
        /// <param name="orderByClauses">The order by clauses</param>
        /// <returns>An ordered IEnumerable</returns>
        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> enumerable, IEnumerable<OrderBy> orderByClauses)
        {
            return enumerable.AsQueryable().OrderBy(orderByClauses).AsEnumerable();
        }

        /// <summary>
        /// Apply order by clauses to an <see cref="IQueryable"/> 
        /// </summary>
        /// <typeparam name="T">The <see cref="IQueryable"/> type</typeparam>
        /// <param name="queryable">The <see cref="IQueryable"/></param>
        /// <param name="orderByClauses">The order by clauses</param>
        /// <returns>An ordered <see cref="IQueryable"/></returns>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable, IEnumerable<OrderBy> orderByClauses)
        {
            return ParseOrderByClauses(orderByClauses).Aggregate(queryable, ApplyOrderByInfo);
        }

        /// <summary>
        /// Parses the clauses
        /// </summary>
        /// <param name="orderByClauses">The clauses to parse</param>
        /// <returns>The enumerable</returns>
        private static IEnumerable<OrderByInfo> ParseOrderByClauses(IEnumerable<OrderBy> orderByClauses)
        {
            if (orderByClauses == null)
            {
                yield break;
            }

            bool initial = true;
            foreach (var clause in orderByClauses)
            {
                if (string.IsNullOrEmpty(clause.PropertyName))
                {
                    yield break;
                }

                yield return new OrderByInfo { PropertyName = clause.PropertyName, SortDirection = clause.SortDirection, Initial = initial };
                initial = false;
            }
        }

        /// <summary>
        /// Applies the info
        /// </summary>
        /// <typeparam name="T">The <see cref="IQueryable"/> type</typeparam>
        /// <param name="queryable">The <see cref="IQueryable"/></param>
        /// <param name="orderByInfo">The order by info</param>
        /// <returns>An ordered <see cref="IQueryable"/></returns>
        private static IQueryable<T> ApplyOrderByInfo<T>(IQueryable<T> queryable, OrderByInfo orderByInfo)
        {
            var properties = orderByInfo.PropertyName.Split('.');
            var type = typeof(T);

            var arg = Expression.Parameter(type, "x");
            Expression expr = arg;
            foreach (var prop in properties)
            {
                var pi = type.GetProperty(prop);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }

            var delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            var lambda = Expression.Lambda(delegateType, expr, arg);
            string methodName;

            if (!orderByInfo.Initial && queryable is IOrderedQueryable<T>)
            {
                methodName = orderByInfo.SortDirection == SortDirection.Ascending ? "ThenBy" : "ThenByDescending";
            }
            else
            {
                methodName = orderByInfo.SortDirection == SortDirection.Ascending ? "OrderBy" : "OrderByDescending";
            }

          
            return (IOrderedQueryable<T>)typeof(Queryable).GetMethods().Single(
                method => method.Name == methodName
                        && method.IsGenericMethodDefinition
                        && method.GetGenericArguments().Length == 2
                        && method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), type)
                .Invoke(null, new object[] { queryable, lambda });
        }

        /// <summary>
        /// OrderBy info
        /// </summary>
        private class OrderByInfo
        {
            /// <summary>
            /// Gets or sets the property name
            /// </summary>
            public string PropertyName { get; set; }

            /// <summary>
            /// Gets or sets the sort direction
            /// </summary>
            public SortDirection SortDirection { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether this clause is the first one
            /// </summary>
            public bool Initial { get; set; }
        }
    }
}
