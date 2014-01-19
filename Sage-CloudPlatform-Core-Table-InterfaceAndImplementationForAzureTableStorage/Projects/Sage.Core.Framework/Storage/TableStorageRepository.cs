using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.StorageClient;
using Sage.Core.Framework.Storage.Exceptions;


namespace Sage.Core.Framework.Storage
{
    /// <summary>
    /// Core implementation of Table storage. 
    /// Azure Table Storage related implementation.
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public class TableStorageRepository<TDomainObject> : TableStore, 
                                                         ITableStore<TDomainObject> 
        where TDomainObject : TableEntityBase, new()
    {

        /// <summary>
        /// Initializes a new instance of the TableStorageRepository class.
        /// </summary>
        /// <param name="tablename">Name of table.</param>
        protected TableStorageRepository(string tablename)
            : base(tablename)
        {
            Tablename = tablename;
        }

   

        #region Public methods

        /// <summary>
        /// Get table rows based on given filter expression
        /// </summary>
        /// <param name="filter">Expression</param>
        /// <returns>Returns an enumeration of dynamic table types.</returns>
        public IEnumerable<TDomainObject> Get(Expression<Func<TDomainObject, bool>> filter)
        {
            return GetBaseQuery(filter, null);
        }

        /// <summary>
        /// Get results based on given filter expression. 
        /// </summary>
        /// <param name="filter">Expression</param>
        /// <param name="pageIndex">Current page number</param>
        /// <param name="resultsPerPage">Number of records per page.</param>
        /// <param name="totalResultsCount">Total number of records.</param>
        /// <returns>Returns an enumeration of dynamic table types and results can be limited by specifying the resultsPerPage to return.</returns>
        public IEnumerable<TDomainObject> Get(Expression<Func<TDomainObject, bool>> filter, int pageIndex,
                                              int resultsPerPage, out long totalResultsCount)
        {

            var result = GetBaseQuery(filter, null);

            totalResultsCount = result.AsEnumerable().LongCount();

            return result.Skip(pageIndex).Take(resultsPerPage).ToList();
        }
        
        /// <summary>
        /// Add or merged the specified entity.
        /// </summary>
        /// <param name="domainObject">The entity whose contents are being inserted or merged.</param>
        /// <returns>Returns row key value.</returns>
        public virtual  string Put(TDomainObject domainObject)
        {
            //To validate Partition Key.
            //ValidateTableProperties(domainObject.PartitionKey, "Partition Key", "Put", TableKeyRegularExpression);

            //To validate Row Key.
           // ValidateTableProperties(domainObject.RowKey, "Row Key", "Put", TableKeyRegularExpression);

            //To keep track of when an entity was last modified.
            domainObject.Timestamp = DateTime.UtcNow;
            try
            {

                //To perform an Insert or Merge operation on that entity,you must detach and reattach the entity.
                Context.Detach(domainObject);
                 Context.AttachTo(TableName, domainObject);

                //Change the state of the entity to "Modified".
                Context.UpdateObject(domainObject);
            }
            catch (TableStorageException storageException)
            {
                throw new Exception(TableResource.TableSaveError, storageException);
            }
            return domainObject.RowKey;
        }

        /// <summary>
        ///  Deletes the specified entity.
        /// </summary>
        /// <param name="domainObject">The entity to be deleted from the table.</param>
        public void Delete(TDomainObject domainObject)
        {
            //To validate Partition Key.
            ValidateTableProperties(domainObject.PartitionKey, "Partition Key", "Delete", TableKeyRegularExpression);

            //To validate Row Key.
            ValidateTableProperties(domainObject.RowKey, "Row Key", "Delete", TableKeyRegularExpression);

            // Create a retrieve operation that expects a customer entity.
            var retrieveOperation = TableOperation.Retrieve(domainObject.PartitionKey, domainObject.RowKey);

            // Execute the operation.
            var retrievedResult = Table.Execute(retrieveOperation);

            // Assign the result to a TDomainObject object.
            var deleteEntity = (TDomainObject) retrievedResult.Result;

            // Create the Delete TableOperation.
            if (deleteEntity != null)
            {
                try
                {
                    //Creates a new table operation to  delete entity.
                    //Deleted the entity from the table.
                    Context.DeleteObject(domainObject);
                }
                catch (TableStorageException storageException)
                {
                    throw new Exception(TableResource.TableDeleteError, storageException);
                }

            }
        }
        
        #endregion

        #region TableStore methods

        /// <summary>
        /// Save changes.
        /// </summary>
        public override void SaveChanges()
        {
            // No SaveChangeOptions is used, which indicates that a MERGE verb will be used. 
            //This set of steps will result in an InsertOrMerge command to be sent to Windows Azure Table.
            Context.SaveChanges();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Queries against a table and returns based on filters expression.
        /// Order the returns based on order direction.
        /// </summary>
        /// <param name="filter">Expression</param>
        /// <param name="orderBy">orderBy direction</param>
        /// <returns>Finds results based upon a given expression</returns>
        private IQueryable<TDomainObject> GetBaseQuery(Expression<Func<TDomainObject, Boolean>> filter,
                                                       IEnumerable<OrderBy> orderBy)
        {
            try
            {
                // Create the cloud table query.
                var query = filter != null
                                ? Context.CreateQuery<TDomainObject>(Tablename)
                                         .Where(filter)
                                         .AsTableServiceQuery()
                                : Context.CreateQuery<TDomainObject>(TableName)
                                         .AsTableServiceQuery();

                //To allow retry behavior when exception occurs based on The number of retries for the operation
                //and the interval until the next retry.
                query.RetryPolicy = RetryPolicies.RetryExponential(5, new TimeSpan(0, 0, 2));

                //Executing query to get the results. 
                var result = query.Execute();

                //Create order by clauses.
                var orderByClauses = orderBy != null ? orderBy.ToList() : new List<OrderBy>();

                //TODO: Implement default sorting.
                result = orderByClauses.Any() ? result.OrderBy(orderByClauses) : result.OrderBy(m => m.Timestamp);
           
                return (IQueryable<TDomainObject>) result;
            }
            catch (TableStorageException storageException)
            {
                throw new Exception(TableResource.TableGetError, storageException);
            }
        }

        #endregion

        #region  Properties

        /// <summary>
        /// Name of the table.
        /// </summary>
        public readonly string Tablename;

        /// <summary>
        /// Regular expression for Partition key and Row Key.
        /// </summary>
        protected  string TableKeyRegularExpression = @"^[^/\\#?]{0,1024}$";
      

        #endregion
    }
}