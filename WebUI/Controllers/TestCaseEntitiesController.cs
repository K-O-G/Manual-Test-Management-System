using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Domain.Concrete;
using Domain.Entities;
using Domain.Entities.TestCases;
using Domain.Helpers;
using Newtonsoft.Json;

namespace WebUI.Controllers
{
    public class TestCaseEntitiesController : Controller
    {
        private EFDbContext db = new EFDbContext();

        // GET: TestCaseEntities
        public ActionResult Index()
        {
            return View(db.TestCases.ToList());
        }

        // GET: TestCaseEntities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestCaseEntity testCaseEntity = db.TestCases.Find(id);
            if (testCaseEntity == null)
            {
                return HttpNotFound();
            }
            ViewBag.Components = db.Components.ToList();
            testCaseEntity.Cases =
                new List<Case>(db.Cases.Where(c => c.TestCaseId == testCaseEntity.TestCaseEntityId));
            foreach (var @case in testCaseEntity.Cases)
            {
                @case.CaseSteps = new List<CaseStep>(db.CaseSteps.Where(cs => cs.CaseId == @case.CaseId));
            }
            //            if (!checkListEntity.CheckListItems.IsNullOrEmpty())
            //            {
            //                foreach (var item in checkListEntity.CheckListItems)
            //                {
            //                    item.CheckListTestResult = db.TestResults.FirstOrDefault(c => c.TestResultValue == "Not Executed");
            //                    db.Entry(item).State = EntityState.Modified;
            //                    db.SaveChanges();
            //                }
            //            }
            if (testCaseEntity.Cases == null)
            {
                return HttpNotFound();
            }

            foreach (var csCase in testCaseEntity.Cases)
            {
                csCase.CaseSteps.OrderBy(cstep => cstep.CaseStepId);
                if (csCase.CaseSteps != null)
                {
                    foreach (var item in csCase.CaseSteps)
                    {
                        var resultId = item.CaseStepResult.TestResultId;
                        if (item.CaseStepResult != null)
                            item.CaseStepResult = db.TestResults.FirstOrDefault(r => r.TestResultId == resultId);
                    }
                }
            }
            return View(testCaseEntity);
        }

        // GET: TestCaseEntities/Create
        public ActionResult Create()
        {
            SelectList priority = new SelectList(db.Priorities, "PriorityId", "PriorityName");
            ViewBag.Priorities = priority;
            ViewBag.Components = db.Components.ToList();
            return View();
        }

        // POST: TestCaseEntities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TestCaseEntityId,TestCaseItemsIdSuffix,TestCaseName,TestCaseDescription,LastEditionDateTime")] TestCaseEntity testCaseEntity, int[] selectedComponents)
        {
            if (ModelState.IsValid)
            {
                testCaseEntity.LastEditionDateTime = DateTime.Now;
                testCaseEntity.CreatorCaseUser = db.Users.FirstOrDefault(t => t.UserId == Repository.CurrentUser.UserId);
                if (selectedComponents != null)
                {
                    List<Component> components = new List<Component>();
                    //получаем выбранные компоненты
                    foreach (var c in db.Components.Where(co => selectedComponents.Contains(co.ComponentId)))
                    {
                        components.Add(c);
                    }

                    testCaseEntity.Components = components;
                }
                db.TestCases.Add(testCaseEntity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(testCaseEntity);
        }

        // GET: TestCaseEntities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestCaseEntity testCaseEntity = db.TestCases.Find(id);
            SelectList priority = new SelectList(db.Priorities, "PriorityId", "PriorityName");
            ViewBag.Priorities = priority;
            ViewBag.Components = db.Components.ToList();
            if (testCaseEntity == null)
            {
                return HttpNotFound();
            }
            return View(testCaseEntity);
        }

        // POST: TestCaseEntities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TestCaseEntityId,TestCaseItemsIdSuffix,TestCaseName,TestCaseDescription,LastEditionDateTime")] TestCaseEntity testCaseEntity, int[] selectedComponents)
        {
            if (ModelState.IsValid)
            {
                if (selectedComponents != null)
                {
                    List<Component> components = new List<Component>();
                    //получаем выбранные компоненты
                    foreach (var c in db.Components.Where(co => selectedComponents.Contains(co.ComponentId)))
                    {
                        components.Add(c);
                    }

                    testCaseEntity.Components = components;
                }

                testCaseEntity.Cases = db.Cases.Where(tc => tc.TestCaseId==testCaseEntity.TestCaseEntityId).ToList();
                if (testCaseEntity.Cases != null)
                {
                    for (int i = 0; i < testCaseEntity.Cases.Count; i++)
                    {
                        testCaseEntity.Cases[i].CaseIdPublic = $"{testCaseEntity.TestCaseItemsIdSuffix}{i + 1}";
                        db.Entry(testCaseEntity.Cases[i]).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                testCaseEntity.LastEditionDateTime = DateTime.Now;
                testCaseEntity.LastEditorCaseUser = db.Users.FirstOrDefault(t => t.UserId == Repository.CurrentUser.UserId);
                db.Entry(testCaseEntity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(testCaseEntity);
        }

        // GET: TestCaseEntities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestCaseEntity testCaseEntity = db.TestCases.Find(id);
            if (testCaseEntity == null)
            {
                return HttpNotFound();
            }
            return View(testCaseEntity);
        }

        // POST: TestCaseEntities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TestCaseEntity testCaseEntity = db.TestCases.Find(id);
            db.TestCases.Remove(testCaseEntity ?? throw new InvalidOperationException());
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Execute(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TestCaseEntity testCaseEntity = db.TestCases.Find(id);
            if (testCaseEntity == null)
            {
                return HttpNotFound();
            }
            SelectList testResult = new SelectList(db.TestResults, "TestResultId", "TestResultValue");
            ViewBag.TestResults = testResult;
            ViewBag.Components = db.Components.ToList();
            testCaseEntity.Cases =
                new List<Case>(db.Cases.Where(c => c.TestCaseId == testCaseEntity.TestCaseEntityId));
            foreach (var @case in testCaseEntity.Cases)
            {
                @case.CaseSteps = new List<CaseStep>(db.CaseSteps.Where(cs => cs.CaseId == @case.CaseId));
            }
            //            if (!checkListEntity.CheckListItems.IsNullOrEmpty())
            //            {
            //                foreach (var item in checkListEntity.CheckListItems)
            //                {
            //                    item.CheckListTestResult = db.TestResults.FirstOrDefault(c => c.TestResultValue == "Not Executed");
            //                    db.Entry(item).State = EntityState.Modified;
            //                    db.SaveChanges();
            //                }
            //            }
            if (testCaseEntity.Cases == null)
            {
                return HttpNotFound();
            }

            foreach (var csCase in testCaseEntity.Cases)
            {
                csCase.CaseSteps.OrderBy(cstep => cstep.CaseStepId);
            }
            return View(testCaseEntity);
        }
        [HttpPost]
        public ActionResult SaveExecute(int caseId, string stepResultsJson, string comment)
        {
            Dictionary<int, int> stepResults = (Dictionary<int, int>)JsonConvert.DeserializeObject(stepResultsJson, typeof(Dictionary<int, int>));
            var @case = db.Cases.Include(c => c.CaseSteps).FirstOrDefault(i => i.CaseId == caseId);

            if (@case != null)
            {
                List<CaseStep> caseSteps = @case.CaseSteps.ToList();
                @case.LastExecutorCaseUser = db.Users.FirstOrDefault(t => t.UserId == Repository.CurrentUser.UserId);
                @case.LastExecutionDateTime = DateTime.Now;
                var stepResultsResult = new Dictionary<int, string>();
                foreach (var stepResult in stepResults)
                {
                    var testResult = db.TestResults.FirstOrDefault(r => r.TestResultId == stepResult.Value);
                    var caseStep = caseSteps.FirstOrDefault(c => c.CaseStepId == stepResult.Key);
                    if (caseStep != null)
                    {
                        caseStep.CaseStepResult = testResult;
                        stepResultsResult.Add(stepResult.Key, testResult.TestResultColor);
                    }
                }
                @case.CaseComment = comment;
                db.SaveChanges();
                return Json(new { stepResultsResult = JsonConvert.SerializeObject(stepResultsResult) });
            }
            return Json(new { result = "failed" });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
