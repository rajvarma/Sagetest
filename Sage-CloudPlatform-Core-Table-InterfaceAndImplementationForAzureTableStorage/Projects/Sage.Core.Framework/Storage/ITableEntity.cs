

using System.Data.Services.Common;
namespace Sage.Core.Framework.Storage
{
    /// <summary>
    ///  An interface required for table entity types.
    /// </summary>
    public interface ITableEntity
    {

        /// <summary>
        /// Gets or sets the entity's partition key.
        /// Partition key is a unique identifier for the partition within a given table.
        /// </summary>
       
        string PartitionKey { get; set; }

        /// <summary>
        /// Gets or sets the entity's row key.
        /// Row key is a unique identifier for an entity within a given partition.
        /// </summary>
        string RowKey { get; set; }

    }
}
