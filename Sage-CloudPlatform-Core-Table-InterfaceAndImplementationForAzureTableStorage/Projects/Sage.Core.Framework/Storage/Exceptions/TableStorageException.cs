using System;
using System.Globalization;
using System.Runtime.Serialization;
using Microsoft.WindowsAzure.Storage;

namespace Sage.Core.Framework.Storage.Exceptions
{
    /// <summary>
    /// Wrapper exception thrown by the Table.
    /// Inner exceptions will include the specific provider error
    /// </summary>
    [Serializable]
    public class TableStorageException : BaseStorageException
    {
        /// <summary>
        /// Name of the table
        /// </summary>
        private string TableName { get; set; }

         public TableStorageException(string tableName)
         {
             TableName = tableName;
         }

        public TableStorageException(String messsage, string tableName)
           : base(messsage)
        {
            TableName = tableName;
        }

        public TableStorageException(String messsage, StorageException innerException, string tableName)
            : base(messsage, innerException)
        {
            TableName = tableName;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, TableResource.TableNameExpected, Message, TableName);
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}
