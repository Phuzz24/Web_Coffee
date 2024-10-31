using System;
using System.Linq;
using System.Web.Mvc;
using Web_Coffee.Models;
using PagedList;

namespace Web_Coffee.Controllers
{
    public class CategoryController : Controller
    {
        private DBCoffeeEntities database = new DBCoffeeEntities();

        public ActionResult Index(string searchString, int? page)
        {
            if (!RoleHelper.IsAdmin() && !RoleHelper.IsEmployee())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var cate = database.Category.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                if (int.TryParse(searchString, out int searchID))
                {
                    cate = cate.Where(s => s.IDCategory == searchID);
                }
                cate = cate.Where(s => s.TenLoai.Contains(searchString));
                                                
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var pagedList = cate.OrderBy(s => s.TenLoai).ToPagedList(pageNumber, pageSize);
            return View(pagedList);
        }

        public ActionResult Create()
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category cate)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (ModelState.IsValid)
            {
                database.Category.Add(cate);
                database.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cate);
        }

        public ActionResult Details(int id)
        {
            if (!RoleHelper.IsAdmin() && !RoleHelper.IsEmployee())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var cate = database.Category.FirstOrDefault(s => s.IDCategory == id);
            if (cate == null)
            {
                return HttpNotFound();
            }
            return View(cate);
        }

        public ActionResult Edit(int id)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var cate = database.Category.FirstOrDefault(s => s.IDCategory == id);
            if (cate == null)
            {
                return HttpNotFound();
            }
            return View(cate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category cate)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (ModelState.IsValid)
            {
                database.Entry(cate).State = System.Data.Entity.EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cate);
        }

        public ActionResult Delete(int id)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var cate = database.Category.FirstOrDefault(s => s.IDCategory == id);
            if (cate == null)
            {
                return HttpNotFound();
            }
            return View(cate);
        }
        [HttpPost]
        public ActionResult Delete(int id, Category cate)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            try
            {
                cate = database.Category.Where(s => s.IDCategory == id).FirstOrDefault();
                database.Category.Remove(cate);
                database.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                database.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}