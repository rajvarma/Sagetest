using System.Web.Mvc;
using System;
using System.Linq;
using AspNetLab.Models;
using AspNetLab.TableRepository;


namespace AspNetLab.Controllers
{
    public class TableStorageController : Controller
    {
        private readonly EmployeeRepository _employeeRepository; 

        public TableStorageController()
        {
            _employeeRepository = new EmployeeRepository();
        }

        [System.Web.Http.HttpGet]
        public ActionResult Index()
        {
            ViewBag.IsGet = false;
          //  var emp = new Employee {EmployeeList = _employeeRepository.Get().ToList()};
           return View();
        }


        public JsonResult Put(FormCollection collection)
        {

            var emp = new Employee
                {
                    FirstName = Convert.ToString(collection["FirstName"]),
                    LastName = Convert.ToString(collection["LastName"]),
                    Department = Convert.ToString(collection["Department"]),
                    EmployeeID = string.IsNullOrEmpty(Convert.ToString(collection["EmployeeID"])) ?
                                                        Guid.Parse(collection["EmployeeID"]) : new Guid() 
                };

            //_employeeRepository.Put(emp);
            return null;
        }

        public JsonResult Remove(string employeeID)
        {
            
          //  var emp = new Employee {EmployeeList = _employeeRepository.Get(m => m.RowKey == employeeID).ToList()};
           
            //foreach (var employee in emp.EmployeeList)
            //{
            //  //  _employeeRepository.Delete(employee);
            //}
            return null;
        }

    }
}
