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

namespace WebUI.Controllers
{
    public class CheckListsController : Controller
    {
        private EFDbContext db = new EFDbContext();

        // GET: CheckLists
        public ActionResult Index()
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
            checkListEntity.CheckListItems =
                db.CheckListItems.Where(c => c.CheckListId == checkListEntity.CheckListEntityId);
            if (checkListEntity == null)
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
                return RedirectToAction("Index");
            }

            return View(checkListEntity);
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
                return RedirectToAction("Index");
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
            CheckListEntity checkListEntity = db.CheckLists.Find(id);
            db.CheckLists.Remove(checkListEntity);
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
