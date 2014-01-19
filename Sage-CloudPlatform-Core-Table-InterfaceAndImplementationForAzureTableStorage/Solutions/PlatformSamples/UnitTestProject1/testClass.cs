using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sage.Core.Framework;
using Sage.Core.Framework.Storage;


namespace UnitTestProject1
{
    class testClass : TableEntityBase
        
    {

        public testClass(string partitionKey)
            : base(partitionKey)
        {
        }

        public testClass()
            : base(string.Empty)
        {

        }

        public Guid EmployeeID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Department { get; set; }


        
      ////PartitionKey.
      //  public override  string PartitionKey
      //  {
      //      get { return Department; }
      //      set {   }
      //  }

      //  public override string RowKey
      //  {
      //      get { return EmployeeID != Guid.Empty ? EmployeeID.ToString() : string.Empty; }
      //      set { EmployeeID = Guid.Parse(value); }
      //  }

     
    }
}
