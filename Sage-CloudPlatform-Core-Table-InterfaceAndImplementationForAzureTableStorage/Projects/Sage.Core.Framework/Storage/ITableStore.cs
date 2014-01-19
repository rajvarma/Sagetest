using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Sage.Core.Framework.Storage
{
    /// <summary>
    /// Represents a Table storage
    /// </summary>
    internal interface ITableStore<TDomainObject> where TDomainObject : TableEntityBase, new()
    {

        /// <summary>
        ///  Get results based upon a given filter. 
        /// </summary>
        /// <param name="filter">Filter expression</param>
        /// <returns>Returns an enumeration of dynamic types</returns>
        IEnumerable<TDomainObject> Get(Expression<Func<TDomainObject, Boolean>> filter);

        /// <summary>
        /// Get results based upon a given filter. 
        /// </summary>
        /// <param name="filter">Filter expression</param>
        /// <param name="pageIndex">Current page number</param>
        /// <param name="resultsPerPage">Number of records per page.</param>
        /// <param name="totalResultsCount">Total number of records.</param>
        /// <returns>Returns an enumeration of dynamic types and results can be limited by specifying the resultsPerPage to return.</returns>
        IEnumerable<TDomainObject> Get(Expression<Func<TDomainObject, bool>> filter, int pageIndex,
                                       int resultsPerPage, out long totalResultsCount);

        /// <summary>
        /// Add  or update the specified entity.
        /// </summary>
        /// <param name="domainObject">The dynamic entity.</param>
        /// <returns>Returns row identity value.</returns>
        string Put(TDomainObject domainObject);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="domainObject">The dynamic entity.</param>
        void Delete(TDomainObject domainObject);

    }
}
