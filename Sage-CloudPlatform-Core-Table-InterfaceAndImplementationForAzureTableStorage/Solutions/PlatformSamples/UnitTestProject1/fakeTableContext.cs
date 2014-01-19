using Sage.Core.Framework.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestProject1
{
    class fakeTableContext : TableContext<testClass>

    {
        public fakeTableContext()
            : base("TestTable")
        {
        }
    }
}
