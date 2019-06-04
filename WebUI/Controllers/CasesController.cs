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

        // GET: Cases
        public ActionResult Index()
        {
            return View(db.Cases.ToList());
        }

        // GET: Cases/Details/5
        public ActionResult Details(int? id)
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

        // GET: Cases/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TestCaseEntity testCaseEntity = db.TestCases.Find(id);
            var @case = new Case() { TestCase = testCaseEntity };
            return View(@case);
        }

        // POST: Cases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(string caseModelJson)
        {
            TestCaseViewModel caseModel = (TestCaseViewModel)JsonConvert.DeserializeObject(caseModelJson, typeof(TestCaseViewModel));
            var dbCase = new Case() { TestCaseId = caseModel.TestCaseId, CaseId = caseModel.CaseId, CaseSummary = caseModel.CaseSummary, CasePreconditions = caseModel.CasePreconditions };
            dbCase.LastExecutionDateTime = DateTime.Now;
            dbCase.LastExecutorCaseUser = Repository.CurrentUser;
            if (dbCase.CaseSteps == null) dbCase.CaseSteps = new List<CaseStep>();
            foreach (var caseModelCaseStep in caseModel.CaseSteps)
            {
                dbCase.CaseSteps.Add(new CaseStep() { CaseStepNumber = caseModelCaseStep.CaseStepNumber, CaseStepDescription = caseModelCaseStep.CaseStepDescription, CaseStepExpectedResult = caseModelCaseStep.CaseStepExpectedResult });
            }
            db.Cases.Add(dbCase);
            db.SaveChanges();
            return Json(new { @url = Url.Action("Index") });
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
                db.Entry(@case).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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
            db.Cases.Remove(@case);
            db.SaveChanges();
            return RedirectToAction("Index");
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
