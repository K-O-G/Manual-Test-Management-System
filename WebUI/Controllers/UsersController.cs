using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Domain.Concrete;
using Domain.Entities;
using Domain.Helpers;

namespace WebUI.Controllers
{
    public class UsersController : Controller
    {
        private EFDbContext db = new EFDbContext();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }


        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,UserName,UserEmail,UserPassword,UserAdmin,UserTestCreator,UserTestExecutor")] User user)
        {
            if (ModelState.IsValid)
            {
                UserSecurity security = new UserSecurity();
                user.UserPassword = security.CalculateMD5Hash(user.UserPassword);
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,UserName,UserEmail,UserPassword,UserAdmin,UserTestCreator,UserTestExecutor")] User user)
        {
            if (ModelState.IsValid)
            {
                UserSecurity security = new UserSecurity();
                user.UserPassword = security.CalculateMD5Hash(user.UserPassword);
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User model)
        {
            if (ModelState.IsValid)
            {
                // поиск пользователя в бд
                User user = null;
                UserSecurity security = new UserSecurity();
                string pass = security.CalculateMD5Hash(model.UserPassword);
                using (db)
                {
                    user = db.Users.FirstOrDefault(u =>
                        ((u.UserName.ToUpper() == model.UserName.ToUpper() ||
                          u.UserEmail.ToUpper() == model.UserName.ToUpper()) && pass == u.UserPassword));

                }
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, true);
                    Repository.CurrentUser = user;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Cannot find user with this name and password");
                }
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            Repository.CurrentUser = null;
            return RedirectToAction("Login");
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
