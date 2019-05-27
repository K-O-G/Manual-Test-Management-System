using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Domain.Concrete;
using Domain.Entities.CheckLists;

namespace WebUI.Controllers
{
    public class CheckListItemsController : Controller
    {
        private EFDbContext db = new EFDbContext();

        // GET: CheckListItems
        public ActionResult Index()
        {

            return View(db.CheckListItems.ToList());
        }

        // GET: CheckListItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckListItem checkListItem = db.CheckListItems.Find(id);
            if (checkListItem == null)
            {
                return HttpNotFound();
            }
            return View(checkListItem);
        }

        // GET: CheckListItems/Create
        public ActionResult Create()
        {
            SelectList checkList = new SelectList(db.CheckLists, "CheckListEntityId", "CheckListName");
            ViewBag.CheckLists = checkList;
            return View();
        }

        // POST: CheckListItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CheckListItemId,CheckListId,Procedure,ExpectedResult,CheckListComment,LastExecutionDateTime")] CheckListItem checkListItem)
        {
            if (ModelState.IsValid)
            {
                checkListItem.LastExecutionDateTime = DateTime.Now;
                db.CheckListItems.Add(checkListItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(checkListItem);
        }

        // GET: CheckListItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SelectList testResults = new SelectList(db.TestResults, "TestResultId", "TestResultValue");
            ViewBag.TestResults = testResults;
            CheckListItem checkListItem = db.CheckListItems.Find(id);
            if (checkListItem == null)
            {
                return HttpNotFound();
            }
            return View(checkListItem);
        }

        // POST: CheckListItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CheckListItemId,CheckListId,Procedure,ExpectedResult,CheckListComment,LastExecutionDateTime")] CheckListItem checkListItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(checkListItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(checkListItem);
        }

        // GET: CheckListItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckListItem checkListItem = db.CheckListItems.Find(id);
            if (checkListItem == null)
            {
                return HttpNotFound();
            }
            return View(checkListItem);
        }

        // POST: CheckListItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CheckListItem checkListItem = db.CheckListItems.Find(id);
            db.CheckListItems.Remove(checkListItem);
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
