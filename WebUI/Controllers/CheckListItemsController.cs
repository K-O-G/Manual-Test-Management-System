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
using Domain.Entities.CheckLists;
using Domain.Helpers;

namespace WebUI.Controllers
{
    public class CheckListItemsController : Controller
    {
        private EFDbContext db = new EFDbContext();


        // GET: CheckListItems/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CheckListEntity checkListEntity = db.CheckLists.Find(id);
            var checkListItem = new CheckListItem() { CheckListEntity = checkListEntity ,CheckListId =checkListEntity.CheckListEntityId};
            return View(checkListItem);
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
                checkListItem.CheckListTestResult = db.TestResults.FirstOrDefault(t => t.TestResultId == 1);
                //                checkListItem.LastExecutionDateTime = null;
                checkListItem.CheckListEntity = db.CheckLists.Find(checkListItem.CheckListId);
                if (checkListItem.CheckListEntity != null)
                {
                    checkListItem.CheckListEntity.CheckListItems = db.CheckListItems
                        .Where(cl => cl.CheckListId == checkListItem.CheckListId).ToList();
                    checkListItem.CheckListItemIdPublic =
                        $"{checkListItem.CheckListEntity.CheckListItemIdSuffix}{checkListItem.CheckListEntity.CheckListItems.Count() + 1}";
                    db.CheckListItems.Add(checkListItem);
                    db.SaveChanges();
                    return RedirectToAction("Details", "CheckLists",
                        new { id = checkListItem.CheckListEntity.CheckListEntityId });
                }
            }

            return RedirectToAction("Details", "CheckLists", new { id = checkListItem.CheckListId });
        }

        // GET: CheckListItems/Edit/5
        public ActionResult Edit(int? id)
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
            var checkList = db.CheckLists.FirstOrDefault(c=>c.CheckListEntityId==checkListItem.CheckListId);
            checkListItem.CheckListEntity = checkList;
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
                checkListItem.CheckListTestResult =
                    db.TestResults.FirstOrDefault(t => t.TestResultId == 1);
                checkListItem.CheckListComment = "";
                checkListItem.LastExecutionDateTime = null;
                checkListItem.CheckListItemIdPublic =
                    $"{checkListItem.CheckListEntity.CheckListItemIdSuffix}{checkListItem.CheckListEntity.CheckListItems.Count()+1}";
                db.Entry(checkListItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "CheckLists", new { id = checkListItem.CheckListId });
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

            checkListItem.CheckListEntity =
                db.CheckLists.FirstOrDefault(c => c.CheckListEntityId == checkListItem.CheckListId);
            var resultId = checkListItem.CheckListTestResult.TestResultId;
            if (checkListItem.CheckListTestResult != null)
                checkListItem.CheckListTestResult = db.TestResults.FirstOrDefault(r => r.TestResultId == resultId);
            return View(checkListItem);
        }

        // POST: CheckListItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CheckListItem checkListItem = db.CheckListItems.Find(id);
            if (checkListItem != null)
            {
                var idCheckList = checkListItem.CheckListId;
                db.CheckListItems.Remove(checkListItem);
                db.SaveChanges();
                return RedirectToAction("Details", "CheckLists", new { id = idCheckList });
            }
            return RedirectToAction("CheckLists", "CheckLists");
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
