using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
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
                new List<CheckListItem>(db.CheckListItems.Where(c => c.CheckListId == checkListEntity.CheckListEntityId));
            if (checkListEntity.CheckListItems == null)
            {
                return HttpNotFound();
            }

            return View(checkListEntity);
        }

        // GET: CheckLists/Create
        public ActionResult Create()
        {
            SelectList priority = new SelectList(db.Priorities, "PriorityId", "PriorityName");
            ViewBag.Priorities = priority;
            ViewBag.Components = db.Components.ToList();
            return View();
        }

        // POST: CheckLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CheckListEntityId,CheckListName,LastEditionDateTime,Priority")] CheckListEntity checkListEntity, int[] selectedComponents)
        {
            if (ModelState.IsValid)
            {
                checkListEntity.LastEditionDateTime = DateTime.Now;
                checkListEntity.CreatorCheckListUser = db.Users.Find(Repository.CurrentUser.UserId);
                var priority = db.Priorities.FirstOrDefault(r => r.PriorityId == checkListEntity.Priority.PriorityId);
                checkListEntity.Priority = priority;
                if (selectedComponents != null)
                {
                    List<Component> components = new List<Component>();
                    //получаем выбранные компоненты
                    foreach (var c in db.Components.Where(co => selectedComponents.Contains(co.ComponentId)))
                    {
                        components.Add(c);
                    }

                    checkListEntity.Components = components;
                }
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
            ViewBag.Components = db.Components.ToList();
            checkListEntity.CheckListItems =
                new List<CheckListItem>(db.CheckListItems.Where(c => c.CheckListId == checkListEntity.CheckListEntityId));
            if (!checkListEntity.CheckListItems.IsNullOrEmpty())
            {
                foreach (var item in checkListEntity.CheckListItems)
                {
                    item.CheckListTestResult = db.TestResults.FirstOrDefault(c => c.TestResultValue == "Not Executed");
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            if (checkListEntity.CheckListItems == null)
            {
                return HttpNotFound();
            }
            return View(checkListEntity);
        }


        [HttpPost]
        public ActionResult SaveExecute(int id, int resultId)
        {
            var item = db.CheckListItems.Include(c => c.CheckListTestResult).FirstOrDefault(i => i.CheckListItemId == id);
            if (item != null)
            {
                item.LastExecutorCheckListUser = db.Users.Find(Repository.CurrentUser.UserId);
                item.LastExecutionDateTime = DateTime.Now;
                var testResult = db.TestResults.FirstOrDefault(r => r.TestResultId == resultId);
                item.CheckListTestResult = testResult;
                db.SaveChanges();
            }
            return Json(new { result = "success" });
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
            ViewBag.Components = db.Components.ToList();
            SelectList priority = new SelectList(db.Priorities, "PriorityId", "PriorityName");
            ViewBag.Priorities = priority;
            return View(checkListEntity);
        }

        // POST: CheckLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CheckListEntityId,CheckListName,LastEditionDateTime")] CheckListEntity checkListEntity, int[] selectedComponents)
        {
            if (ModelState.IsValid)
            {
                checkListEntity.LastEditionDateTime = DateTime.Now;
                checkListEntity.LastEditorCheckListUser = db.Users.Find(Repository.CurrentUser.UserId);
                var priority = db.Priorities.FirstOrDefault(r => r.PriorityId == checkListEntity.Priority.PriorityId);
                checkListEntity.Priority = priority;
                if (selectedComponents != null)
                {
                    List<Component> components = new List<Component>();
                    //получаем выбранные курсы
                    foreach (var c in db.Components.Where(co => selectedComponents.Contains(co.ComponentId)))
                    {
                        components.Add(c);
                    }

                    checkListEntity.Components = components;
                }
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
            if (checkListEntity != null)
            {
//                foreach (var item in checkListEntity.CheckListItems)
//                {
//                    var it = db.CheckListItems.Find(item);
//                    db.CheckListItems.Remove(it ?? throw new InvalidOperationException());
//
//                }
                db.CheckLists.Remove(checkListEntity);
            }

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
