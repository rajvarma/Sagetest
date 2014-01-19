using System;
using System.Text.RegularExpressions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using Sage.Core.Framework.Storage.Exceptions;
using Sage.Core.Utilities.Diagnostics;

namespace Sage.Core.Framework.Storage
{
    /// <summary>
    /// Core implementation of Table Storage
    /// </summary>
    public abstract class TableStore
    {

        /// <summary>
        /// Initializes a new instance of the TableStore class.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        protected TableStore(string tableName)
        {

            TableName = tableName.ToLower();
            SetupStorageContext();
        }

        #region Abstract methods

        /// <summary>
        /// Saves the context changes.
        /// </summary>
        public abstract void SaveChanges();

        #endregion
        
        #region Private methods

        /// <summary>
        /// Setup azure table context.
        /// </summary>
        private void SetupStorageContext()
        {
            //validate table name
            ValidateTableProperties(TableName, "Table name", TableResource.TableConfigurationError, TableNameRegularExpression);

            // Create the table client.
            TableClient = _account.CreateCloudTableClient();

            //Create table service context.
            Context = new TableServiceContext(TableClient);

            ArgumentValidator.ValidateNonNullReference(Context, "Table name", TableResource.TableConfigurationError);

            if (Context != null)
            {
                try
                {
                    //Gets a reference to the specified table name. 
                    Table = TableClient.GetTableReference(TableName.ToLower());

                    //Create the table if it doesn't exist.
                    //Table.CreateIfNotExists();
                }
                catch (TableStorageException storageException)
                {
                    throw new Exception(TableResource.TableConfigurationError, storageException);
                }
                catch (Exception)
                {
                    throw new Exception(TableResource.TableConfigurationError);
                }
            }

        }

        #endregion

        #region Protected methods
       
        /// <summary>
        /// To validate table properties.
        /// </summary>
        /// <param name="propertieName">Table property Name.</param>
        /// <param name="value">Property value.</param>
        /// <param name="source">The source of the validation check.</param>
        /// <param name="regularExpression">Regular expression to validate.</param>
        protected void ValidateTableProperties(string propertieName, string value, string source, string regularExpression = null)
        {
            // To validate empty string.
            ArgumentValidator.ValidateNonEmptyString(value, propertieName, source);

            if (!string.IsNullOrEmpty(regularExpression))
            {
                //Validate table properties by given expression. 
                ArgumentValidator.ValidateStringIsMatchForRegularExpression(propertieName, value, source,
                                                                            regularExpression, RegexOptions.Compiled);
            }
        }

        #endregion

        #region  Properties

        #region Private properties

        /// <summary>
        ///Retrieve the storage account information.
        /// </summary>
        private readonly CloudStorageAccount _account = CloudStorageAccount.DevelopmentStorageAccount;

        /// <summary>
        /// Azure table client.
        /// </summary>
        private CloudTableClient TableClient { get; set; }

        /// <summary>
        /// Regular expression for table name.
        /// </summary>
        private const string TableNameRegularExpression = @"^[A-Za-z][A-Za-z0-9]{2,62}$";

        #endregion

        #region Public properties

        /// <summary>
        /// Name of the table.
        /// </summary>
        public readonly string TableName;

        /// <summary>
        /// Azure  table context.
        /// </summary>
        public TableServiceContext Context { get; set; }

        /// <summary>
        ///Azure table.
        /// </summary>
        public CloudTable Table;

        #endregion

        #endregion

    }
}
