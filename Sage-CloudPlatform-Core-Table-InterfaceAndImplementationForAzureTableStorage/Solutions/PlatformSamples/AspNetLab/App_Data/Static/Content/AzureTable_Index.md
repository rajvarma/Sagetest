##Sage Core Table Storage usage and Sample

Sage Table Storage service is an extension to Windows Azure Table Storage, with transient fault handling. The initial implementation provides APIs for Get, Put, Delete. Following are the steps involved to use the Table Storage service.

1. Install Sage.Core.Framework.Storage NuGet package from Sage NuGet gallery.
2. Add a new configuration in azure configuration with the name SystemStorageConnectionString and set the storage connection string.
3. You can DI TableContext or manually instantiate the object and use it.

####Usage

Inherent azure TableContext in your DomainRepository

    DomainRepository : TableContext<TClass>

Constructor of your DomainRepository should be defined as following:

	 public DomainRepository()
            : base("TableName")
        {
        }
		Note: 
		i.	 Table names must be unique within an account.
		ii.	 Table names may contain only alphanumeric characters.
		iii. Table names cannot begin with a numeric character.
		iv.  Table names are case-insensitive.
		v.	 Table names must be from 3 to 63 characters long.


 Assume that the "Employee" is an table entity and it should be defined as following:

	Employee : TableEntityBase
	{

	}


And in Employee table entity should override PartitionKey and RowKey methods as following:

		public override string PartitionKey
        {
            get { return Department; }
        }

        public override string RowKey
        {
            get { return EmployeeID != Guid.Empty ? EmployeeID.ToString() : string.Empty; }
            set { EmployeeID = Guid.Parse(value); }
        }
		
		Note: 
		i.   A Partition key is a unique identifier for the partition within a given table.
		ii.	 A RowKey is your "primary key" within a partition.Within one PartitionKey,you can only have unique RowKeys.
		iii. You must include Partition key and RowKey property in every insert, update, and delete operation.

To Inserted or Update a record from table

    DomainRepository.Put(TClass domainObject);
	
	Note:
	i.  The domainObject can be class whose contents are being inserted or update.
	
To Get records from table

    DomainRepository.Get(Expression<Func<TClass, Boolean>> filter = null);
	
	Note:
	i.	 The filter is a function to test each element for a condition.

	DomainRepository.Get(Expression<Func<TClass, bool>> filter = null,int pageIndex, int maxResultsCount,out long totalResultsCount);

	Note:
	i.	 The filter is a function to test each element for a condition.  
	ii.	 The pageIndex is the current page number.
	iii. The maxResultsCount is the maximum results per page.
	iv.  The totalResultsCount is the Total records in the a table.

	DomainRepository.Get(int pageIndex, int maxResultsCount,out long totalResultsCount);

	Note:
	i.	 The filter is a function to test each element for a condition.  
	ii.	 The pageIndex is the current page number.
	iii. The maxResultsCount is the maximum results per page.
	iv.  The totalResultsCount is the Total records in the a table.
	
To Delete record from table

	DomainRepository.Delete(TClass domainObject);
	
	Note:
	i. The domainObject can be class whose contents to be deleted from the table.
	