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
        public ActionResult Create([Bind(Include = "TestCaseEntityId,TestCaseName,TestCaseDescription,LastEditionDateTime")] TestCaseEntity testCaseEntity, int[] selectedComponents)
        {
            if (ModelState.IsValid)
            {
                testCaseEntity.LastEditionDateTime = DateTime.Now;
                testCaseEntity.CreatorCaseUser = db.Users.Find(Repository.CurrentUser.UserId);
                if (selectedComponents != null)
                {
                    List<Component> components = new List<Component>();
                    //получаем выбранные курсы
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
        public ActionResult Edit([Bind(Include = "TestCaseEntityId,TestCaseName,TestCaseDescription,LastEditionDateTime")] TestCaseEntity testCaseEntity, int[] selectedComponents)
        {
            if (ModelState.IsValid)
            {
                if (selectedComponents != null)
                {
                    List<Component> components = new List<Component>();
                    //получаем выбранные курсы
                    foreach (var c in db.Components.Where(co => selectedComponents.Contains(co.ComponentId)))
                    {
                        components.Add(c);
                    }

                    testCaseEntity.Components = components;
                }
                testCaseEntity.LastEditionDateTime= DateTime.Now;
                testCaseEntity.LastEditorCaseUser = db.Users.Find(Repository.CurrentUser.UserId);
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
            db.TestCases.Remove(testCaseEntity);
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
