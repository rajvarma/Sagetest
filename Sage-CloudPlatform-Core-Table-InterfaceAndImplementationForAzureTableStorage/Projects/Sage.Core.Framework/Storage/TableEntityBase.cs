using System;
using System.Data.Services;
using System.Data.Services.Common;

namespace Sage.Core.Framework.Storage
{
    /// <summary>
    /// Base class of table storage.
    /// </summary>
    [DataServiceKey("PartitionKey", "RowKey")]
    public class TableEntityBase
    {
        public TableEntityBase(string partitionKey)
        {
            _PartitionKey = partitionKey;
            _RowKey = Guid.NewGuid().ToString();
        }

        #region ITableEntity methods

        /// <summary>
        /// Gets or sets the entity's partition key.Partition key is a unique 
        /// identifier for the partition within a given table.
        /// </summary>

        private string _PartitionKey;

        /// <summary>
        /// Gets or sets the entity's row key.Row key is a unique identifier for an entity 
        /// within a given partition.
        /// </summary>

        private string _RowKey;


        #endregion

        #region  Properties

        /// <summary>
        /// Gets or sets the entity's timestamp.
        /// Timestamp allows the service to keep track of when an entity was last modified. 
        /// </summary>

        internal DateTime Timestamp { get; set; }

        #endregion



        public string PartitionKey
        {
            get
            {
                return _PartitionKey;
            }
            set
            {
                value = _PartitionKey;
            }
        }

        public string RowKey
        {
            get
            {
                return _RowKey;
            }
            set
            {
                value = _RowKey;
            }
        }
    }
}

