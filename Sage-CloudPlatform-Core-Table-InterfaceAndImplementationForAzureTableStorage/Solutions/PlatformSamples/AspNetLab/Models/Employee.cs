using System;
using System.Collections.Generic;
using Sage.Core.Framework.Storage;

namespace AspNetLab.Models
{
    public class Employee 
    {
    

        public Guid EmployeeID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Department { get; set; }


        public  string PartitionKey
        {
            get { return Department; }
            set { Department = value; }
        }

        public  string RowKey
        {
            get { return EmployeeID != Guid.Empty ? EmployeeID.ToString() : string.Empty; }
            set { EmployeeID = Guid.Parse(value); }
        }

        protected internal List<Employee> EmployeeList { get; set; }
    }

}