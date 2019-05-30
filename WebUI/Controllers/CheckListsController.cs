using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Internal;
using Domain.Concrete;
using Domain.Entities;
using Domain.Entities.CheckLists;
using Domain.Helpers;

namespace WebUI.Controllers
{
    public class CheckListsController : Controller
    {
        private EFDbContext db = new EFDbContext();

        // GET: CheckLists
        public ActionResult CheckLists()
        {
            return View(db.CheckLists.ToList());
        }

        // GET: CheckLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckListEntity checkListEntity = db.CheckLists.Find(id);
            if (checkListEntity == null)
            {
                return HttpNotFound();
            }
            checkListEntity.CheckListItems =
                db.CheckListItems.Where(c => c.CheckListId == checkListEntity.CheckListEntityId);
            if (checkListEntity.CheckListItems == null)
            {
                return HttpNotFound();
            }
//            checkListEntity.CheckListItems.GetEnumerator().Current.CheckListTestResult =
//                db.TestResults.Find(checkListEntity.CheckListItems.GetEnumerator().Current.CheckListTestResult
//                    .TestResultId);

            return View(checkListEntity);
        }

        // GET: CheckLists/Create
        public ActionResult Create()
        {
            SelectList priority = new SelectList(db.Priorities, "PriorityId", "PriorityName");
            ViewBag.Priorities = priority;
            return View();
        }

        // POST: CheckLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CheckListEntityId,CheckListName,LastEditionDateTime")] CheckListEntity checkListEntity, User user)
        {
            if (ModelState.IsValid)
            {
                checkListEntity.LastEditionDateTime = DateTime.Now;
                checkListEntity.LastEditorCheckListUser = user;
                db.CheckLists.Add(checkListEntity);
                db.SaveChanges();
                return RedirectToAction("CheckLists");
            }

            return View(checkListEntity);
        }

        public ActionResult Execute(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckListEntity checkListEntity = db.CheckLists.Find(id);
            if (checkListEntity == null)
            {
                return HttpNotFound();
            }
            SelectList testResult = new SelectList(db.TestResults, "TestResultId", "TestResultValue");
            ViewBag.TestResults = testResult;
            checkListEntity.CheckListItems =
                db.CheckListItems.Where(c => c.CheckListId == checkListEntity.CheckListEntityId);
            if (checkListEntity.CheckListItems == null)
            {
                return HttpNotFound();
            }
            return View(checkListEntity);
        }

        // POST: CheckLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Execute([Bind(Include = "CheckListEntityId,CheckListName")] CheckListEntity checkListEntity, User user)
        {
            //TODO exception 
            if (ModelState.IsValid)
            {
//                checkListEntity.CheckListItems =
//                    db.CheckListItems.Where(c => c.CheckListId == checkListEntity.CheckListEntityId);
                if (checkListEntity.CheckListItems == null)
                {
                    return HttpNotFound();
                }
                checkListEntity.CheckListItems.GetEnumerator().Current.LastExecutionDateTime = DateTime.Now;
                checkListEntity.CheckListItems.GetEnumerator().Current.LastExecutorCheckListUser = user;
                db.Entry(checkListEntity.CheckListItems.GetEnumerator().Current).State = EntityState.Modified;
                db.SaveChanges();
//                return View(checkListEntity);
            }

            return View(checkListEntity);
        }

        [HttpPost]
        public ActionResult SaveExecute(int id, int resultId)
        {
           var item = db.CheckListItems.Include(c=>c.CheckListTestResult).FirstOrDefault(i => i.CheckListItemId == id);
           if (item != null)
           {
               item.LastExecutorCheckListUser = Repository.CurrentUser;
               item.LastExecutorCheckListUser.UserId = Repository.CurrentUser.UserId;
               item.LastExecutionDateTime = DateTime.Now;
               var testResult = db.TestResults.FirstOrDefault(r => r.TestResultId == resultId);
               item.CheckListTestResult = testResult;
               db.SaveChanges();
           }
           return Json(new {result = "success"});
        }
        // GET: CheckLists/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckListEntity checkListEntity = db.CheckLists.Find(id);
            
            if (checkListEntity == null)
            {
                return HttpNotFound();
            }
            return View(checkListEntity);
        }

        // POST: CheckLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CheckListEntityId,CheckListName,LastEditionDateTime")] CheckListEntity checkListEntity, User user)
        {
            if (ModelState.IsValid)
            {
                checkListEntity.LastEditionDateTime = DateTime.Now;
                checkListEntity.LastEditorCheckListUser = user;
                db.Entry(checkListEntity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("CheckLists");
            }
            return View(checkListEntity);
        }

        // GET: CheckLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckListEntity checkListEntity = db.CheckLists.Find(id);
            if (checkListEntity == null)
            {
                return HttpNotFound();
            }
            return View(checkListEntity);
        }

        // POST: CheckLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //todo recursively delete items?
            CheckListEntity checkListEntity = db.CheckLists.Find(id);
            checkListEntity.CheckListItems =
                db.CheckListItems.Where(c => c.CheckListId == checkListEntity.CheckListEntityId);
            if (!checkListEntity.CheckListItems.IsNullOrEmpty())
            {
                foreach (var item in checkListEntity.CheckListItems)
                {
                    db.CheckListItems.Remove(checkListEntity.CheckListItems.GetEnumerator().Current);
                }
            }
            db.CheckLists.Remove(checkListEntity);
            db.SaveChanges();
            return RedirectToAction("CheckLists");
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
