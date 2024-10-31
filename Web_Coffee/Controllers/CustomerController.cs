using System;
using System.Linq;
using System.Web.Mvc;
using Web_Coffee.Models;
using PagedList;

namespace Web_Coffee.Controllers
{
    public class CustomerController : Controller
    {
        private DBCoffeeEntities database = new DBCoffeeEntities();

        public ActionResult DSKhachHang(string searchString, int? page)
        {
            if (!RoleHelper.IsAdmin() && !RoleHelper.IsEmployee())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var khachHangs = database.KhachHang.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                if (int.TryParse(searchString, out int searchID))
                {
                    khachHangs = khachHangs.Where(s => s.IDKhachHang == searchID);
                }
                else
                {
                    khachHangs = khachHangs.Where(s => s.HoTenKH.Contains(searchString) ||
                                                       s.DiaChiKH.Contains(searchString) ||
                                                       s.CodeKhachHang.Contains(searchString) ||
                                                       s.GmailKH.Contains(searchString) ||
                                                       s.SDTKH.Contains(searchString));
                }
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var pagedList = khachHangs.OrderBy(s => s.HoTenKH).ToPagedList(pageNumber, pageSize);
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
        public ActionResult Create(KhachHang khachHang)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            if (ModelState.IsValid)
            {
                database.KhachHang.Add(khachHang);
                database.SaveChanges();
                return RedirectToAction("DSKhachHang");
            }

            return View(khachHang);
        }

        public ActionResult Details(int id)
        {
            if (!RoleHelper.IsAdmin() && !RoleHelper.IsEmployee())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var khachHang = database.KhachHang.FirstOrDefault(s => s.IDKhachHang == id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        public ActionResult Edit(int id)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var khachHang = database.KhachHang.FirstOrDefault(s => s.IDKhachHang == id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        [HttpPost]
        public ActionResult Edit(int id, KhachHang khachHang)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            database.Entry(khachHang).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("DSKhachHang");
        }

        public ActionResult Delete(int id)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var khachHang = database.KhachHang.FirstOrDefault(s => s.IDKhachHang == id);
            if (khachHang == null)
            {
                return HttpNotFound();
            }
            return View(khachHang);
        }

        [HttpPost]
        public ActionResult Delete(int id, KhachHang khachHang)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            try
            {
                khachHang = database.KhachHang.Where(s => s.IDKhachHang == id).FirstOrDefault();
                database.KhachHang.Remove(khachHang);
                database.SaveChanges();
                return RedirectToAction("DSKhachHang");
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