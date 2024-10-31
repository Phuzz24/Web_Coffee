using System;
using System.Linq;
using System.Web.Mvc;
using Web_Coffee.Models;
using PagedList;

namespace Web_Coffee.Controllers
{
    public class ProductSupplierController : Controller
    {
        private DBCoffeeEntities database = new DBCoffeeEntities();

        public ActionResult DSNCC(string searchString, int? page)
        {
            if (!RoleHelper.IsAdmin() && !RoleHelper.IsEmployee())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var ncc = database.NhaCungCap.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                if (int.TryParse(searchString, out int searchID))
                {
                    ncc = ncc.Where(s => s.IDNCC == searchID);
                }
                else
                {
                    ncc = ncc.Where(s => s.TenNCC.Contains(searchString) ||
                                                       s.DiaChiNCC.Contains(searchString));
                }
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var pagedList = ncc.OrderBy(s => s.TenNCC).ToPagedList(pageNumber, pageSize);
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
        public ActionResult Create(NhaCungCap ncc)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (ModelState.IsValid)
            {
                database.NhaCungCap.Add(ncc);
                database.SaveChanges();
                return RedirectToAction("DSNCC");
            }

            return View(ncc);
        }

        public ActionResult Details(int id)
        {
            if (!RoleHelper.IsAdmin() && !RoleHelper.IsEmployee())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var ncc = database.NhaCungCap.FirstOrDefault(s => s.IDNCC == id);
            if (ncc == null)
            {
                return HttpNotFound();
            }
            return View(ncc);
        }

        public ActionResult Edit(int id)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var ncc = database.NhaCungCap.FirstOrDefault(s => s.IDNCC == id);
            if (ncc == null)
            {
                return HttpNotFound();
            }
            return View(ncc);
        }

        [HttpPost]
        public ActionResult Edit(int id, NhaCungCap ncc)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            database.Entry(ncc).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("DSNCC");
        }

        public ActionResult Delete(int id)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var ncc = database.NhaCungCap.FirstOrDefault(s => s.IDNCC == id);
            if (ncc == null)
            {
                return HttpNotFound();
            }
            return View(ncc);
        }

        [HttpPost]
        public ActionResult Delete(int id, NhaCungCap ncc)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            try
            {
                ncc = database.NhaCungCap.Where(s => s.IDNCC == id).FirstOrDefault();
                database.NhaCungCap.Remove(ncc);
                database.SaveChanges();
                return RedirectToAction("DSNCC");
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