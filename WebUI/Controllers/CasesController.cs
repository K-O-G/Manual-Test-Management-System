using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Domain.Concrete;
using Domain.Entities.TestCases;
using Domain.Helpers;
using Newtonsoft.Json;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class CasesController : Controller
    {
        private EFDbContext db = new EFDbContext();


        // GET: Cases/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TestCaseEntity testCaseEntity = db.TestCases.Find(id);
            var @case = new Case() { TestCase = testCaseEntity, TestCaseId = id};

            return View(@case);
        }

        // POST: Cases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(string caseModelJson)
        {
            TestCaseViewModel caseModel = (TestCaseViewModel)JsonConvert.DeserializeObject(caseModelJson, typeof(TestCaseViewModel));
            var dbCase = new Case()
            {
                TestCaseId = caseModel.TestCaseId, CaseId = caseModel.CaseId, CaseSummary = caseModel.CaseSummary,
                CasePreconditions = caseModel.CasePreconditions
            };
            dbCase.TestCase = db.TestCases.Find(dbCase.TestCaseId);
            if (dbCase.TestCase != null)
                dbCase.CaseIdPublic = $"{dbCase.TestCase.TestCaseItemsIdSuffix}{dbCase.TestCase.Cases.Count()+ 1}";
//                db.Users.FirstOrDefault(t=>t.UserId==Repository.CurrentUser.UserId);
            if (dbCase.CaseSteps == null) dbCase.CaseSteps = new List<CaseStep>();
            foreach (var caseModelCaseStep in caseModel.CaseSteps)
            {
                dbCase.CaseSteps.Add(new CaseStep()
                {
                    CaseStepNumber = caseModelCaseStep.CaseStepNumber,
                    CaseStepDescription = caseModelCaseStep.CaseStepDescription,
                    CaseStepExpectedResult = caseModelCaseStep.CaseStepExpectedResult,
                    CaseStepResult = db.TestResults.FirstOrDefault(t => t.TestResultId == 1)
                });
            }
            db.Cases.Add(dbCase);
            db.SaveChanges();
            return Json(new { @url = Url.Action("Details","TestCaseEntities") });
        }

        // GET: Cases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Case @case = db.Cases.Find(id);
            if (@case == null)
            {
                return HttpNotFound();
            }
            return View(@case);
        }

        // POST: Cases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CaseId,TestCaseId,CaseSummary,CasePreconditions,CaseComment,LastExecutionDateTime")] Case @case)
        {
            if (ModelState.IsValid)
            {
                @case.CaseComment = "";
                @case.LastExecutorCaseUser = null;
                @case.LastExecutionDateTime = null;
                db.Entry(@case).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details","TestCaseEntities",new{id=@case.TestCaseId});
            }
            return View(@case);
        }

        // GET: Cases/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Case @case = db.Cases.Find(id);
            if (@case == null)
            {
                return HttpNotFound();
            }
            return View(@case);
        }

        // POST: Cases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Case @case = db.Cases.Find(id);
            var idTestCase = @case.TestCaseId;
            db.Cases.Remove(@case);
            db.SaveChanges();
            return RedirectToAction("Details", "TestCaseEntities", new {id = idTestCase});
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
