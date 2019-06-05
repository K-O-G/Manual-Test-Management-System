using Domain.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain.Concrete;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private EFDbContext db = new EFDbContext();
        // GET: Home
        public ActionResult Index()
        {
            List<DataPoint> checkLists = new List<DataPoint>();
            var allCountCL = db.CheckListItems.Count();
            foreach (var testResult in db.TestResults)
            {
                var count = db.CheckListItems
                    .Count(t => t.CheckListTestResult.TestResultId == testResult.TestResultId);
                count = (count * 100) / allCountCL;
                checkLists.Add(new DataPoint(testResult.TestResultValue, count));
            }
            ViewBag.CheckLists = JsonConvert.SerializeObject(checkLists);

            List<DataPoint> testCases = new List<DataPoint>();

            var allCountTC = db.CaseSteps.Count(); 
            foreach (var testResult in db.TestResults)
            {
                var count = db.CaseSteps
                    .Count(t => t.CaseStepResult.TestResultId == testResult.TestResultId);
                count = (count * 100) / allCountTC;
                testCases.Add(new DataPoint(testResult.TestResultValue, count));
            }
            ViewBag.TestCases = JsonConvert.SerializeObject(testCases);
            return View();
        }
    }
}