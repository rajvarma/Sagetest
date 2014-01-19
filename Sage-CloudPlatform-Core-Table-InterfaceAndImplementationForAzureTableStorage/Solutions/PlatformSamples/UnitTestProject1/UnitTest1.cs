using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sage.Core.Framework.Storage;
namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            fakeTableContext test = new fakeTableContext();

            Mock<fakeTableContext> morkRepository = new Mock<fakeTableContext>();

            testClass calls = new testClass("sfgwsfgqsfg");
            calls.EmployeeID = Guid.NewGuid();
            calls.FirstName = "testRahj";
            calls.Department = "afagag";
            calls.LastName = "varm,a";
            var mockClass = new testClass( "Test");
            var mockDaoFactory = new Mock<TableStorageRepository<testClass>>("TestTable");
          //  morkRepository.Setup(x => x.Recorder).Returns(morkRepository.Object);

            mockDaoFactory.CallBase =  true;
            mockDaoFactory.Setup(m => m.Put(mockClass)).Returns(calls.RowKey);    
            //  var actualSurveys = store.GetSurveysByTenant(“tenant”);
          
            fakeTableContext  cont = new fakeTableContext ()                 ;
            cont.Put(calls)                             ;
         //   TableContext<testClass> contx = new TableContext<testClass>("Tablenasfgasfg");
            Assert.AreEqual(calls.RowKey, cont.Put(calls));


        }

    }
}
