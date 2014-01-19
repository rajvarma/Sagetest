using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AspNetLab.Models;
using Sage.Core.Cache;
using Sage.Core.Framework.Storage;


namespace AspNetLab.Controllers
{
    public class AzureTableStorageController : BaseController
    {
       // private ITableRepository<Employee> _tableRepository;
        public AzureTableStorageController(ICache cache) 
            : base(cache)
        {
          // _tableRepository = ;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return null;
        }

        public ActionResult Get()
        {
            return null;
        }

        public ActionResult GetPaged()
        {
            return null;
        }

        public ActionResult FirstOrDefault()
        {
            return null;
        }

        public ActionResult Put()
        {
            return null;
        }

        public ActionResult Delete()
        {
            return null;
        }
    }
}
