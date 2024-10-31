using System;
using System.Linq;
using System.Web.Mvc;
using Web_Coffee.Models;
using PagedList;

namespace Web_Coffee.Controllers
{
    public class StaffController : Controller
    {
        private DBCoffeeEntities database = new DBCoffeeEntities();

        public ActionResult DSNhanVien(string searchString, int? page)
        {
            if (!RoleHelper.IsAdmin() && !RoleHelper.IsEmployee())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var nhanViens = database.NhanVien.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                if (int.TryParse(searchString, out int searchID))
                {
                    nhanViens = nhanViens.Where(s => s.IDNhanVien == searchID);
                }
                nhanViens = nhanViens.Where(s => s.HoTenNV.Contains(searchString) ||
                                                 s.RoleNV.Contains(searchString) ||
                                                 s.DiaChiNV.Contains(searchString) ||
                                                 s.GmaiNV.Contains(searchString) ||
                                                 s.SdtNV.Contains(searchString));
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var pagedList = nhanViens.OrderBy(s => s.HoTenNV).ToPagedList(pageNumber, pageSize);
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
        public ActionResult Create(NhanVien nhanVien)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (ModelState.IsValid)
            {
                database.NhanVien.Add(nhanVien);
                database.SaveChanges();
                return RedirectToAction("DSNhanVien");
            }

            return View(nhanVien);
        }

        public ActionResult Details(int id)
        {
            if (!RoleHelper.IsAdmin() && !RoleHelper.IsEmployee())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var nhanVien = database.NhanVien.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        public ActionResult Edit(int id)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var nhanVien = database.NhanVien.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NhanVien nhanVien)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (ModelState.IsValid)
            {
                database.Entry(nhanVien).State = System.Data.Entity.EntityState.Modified;
                database.SaveChanges();
                return RedirectToAction("DSNhanVien");
            }
            return View(nhanVien);
        }

        public ActionResult Delete(int id)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var nhanVien = database.NhanVien.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var nhanVien = database.NhanVien.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            database.NhanVien.Remove(nhanVien);
            database.SaveChanges();
            return RedirectToAction("DSNhanVien");
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