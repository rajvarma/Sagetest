using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Sage.Core.Utilities.Diagnostics;


namespace Sage.Core.Framework.Storage
{
    /// <summary>
    /// Platform implementation of Table Storage.
    /// </summary>
    /// <typeparam name="TClass">Dynamic class</typeparam>
    public class TableContext<TClass> : TableStorageRepository<TClass>
        where TClass : TableEntityBase, new()
    {

        /// <summary>
        /// Initializes a new instance of the TableContext class.
        /// </summary>
        /// <param name="tablename">Name of the table.</param>
        public TableContext(string tablename)
            : base(tablename)
        {
            ArgumentValidator.ValidateNonEmptyString(tablename, "Table name", "TableContext");
        }

        #region Public methods

        /// <summary>
        /// Gets results based upon a given by filter instance.
        /// </summary>
        /// <param name="filter">Filter expression</param>
        /// <returns>Results enumeration of specified dynamic entity</returns>
        public new IEnumerable<TClass> Get(Expression<Func<TClass, bool>> filter = null)
        {
            return base.Get(filter);
        }

        /// <summary>
        ///  Gets results based upon a given filter.
        /// </summary>
        /// <param name="filter">Filter expression</param>
        /// <param name="pageIndex">Current page number</param>
        /// <param name="maxResultsCount">Number of results per page</param>
        /// <param name="totalResultsCount">Total results count</param>
        /// <returns>Results enumeration of specified dynamic entity and can be limited by specifying the maxResultsCount to return</returns>
        public IEnumerable<TClass> Get(int pageIndex, int maxResultsCount,
                                       out long totalResultsCount, Expression<Func<TClass, bool>> filter = null)
        {
            ArgumentValidator.ValidateMaxIntegerValue(maxResultsCount, "maxResultsCount", "TableContext.Get", 1);
            return Get(filter, pageIndex, maxResultsCount, out totalResultsCount);

        }

        /// <summary>
        /// Insert or update entity.
        /// </summary>
        /// <param name="domainObject">The entity whose contents are being Put</param>
        /// <returns>Returns row identity value.</returns>
        public new  string Put(TClass domainObject)
        {
            ArgumentValidator.ValidateNonNullReference(domainObject, "domainObject", "TableContext.Put");
            return base.Put(domainObject);
        }

        /// <summary>
        /// Delete entity.
        /// </summary>
        /// <param name="domainObject">The entity to be deleted from the table.</param>
        /// <returns></returns>
        public new void Delete(TClass domainObject)
        {
            ArgumentValidator.ValidateNonNullReference(domainObject, "domainObject", "TableContext.Delete");
            base.Delete(domainObject);
        }

        #endregion

    }
}