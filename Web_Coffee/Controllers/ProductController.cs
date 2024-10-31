using System;
using System.Linq;
using System.Web.Mvc;
using Web_Coffee.Models;
using PagedList;
using System.IO;

namespace Web_Coffee.Controllers
{
    public class ProductController : Controller
    {
        private DBCoffeeEntities database = new DBCoffeeEntities();

        public ActionResult DSSanPham(string searchString, int? page)
        {
            if (!RoleHelper.IsAdmin() && !RoleHelper.IsEmployee())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var sanphams = database.SanPham.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                if (int.TryParse(searchString, out int searchID))
                {
                    sanphams = sanphams.Where(s => s.IDSanPham == searchID);
                }
                else if (double.TryParse(searchString, out double searchPrice))
                {
                    sanphams = sanphams.Where(s => s.Gia == searchPrice);
                }
                sanphams = sanphams.Where(s => s.CodeSanPham.Contains(searchString) ||
                                                 s.TenSanPham.Contains(searchString) ||
                                                 s.HinhAnh.Contains(searchString));             }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var pagedList = sanphams.OrderBy(s => s.TenSanPham).ToPagedList(pageNumber, pageSize);
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
        public ActionResult Create(SanPham sanPham)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            try
            {
                if (sanPham.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(sanPham.ImageUpload.FileName);
                    string extension = Path.GetExtension(sanPham.ImageUpload.FileName);
                    fileName = fileName + extension;
                    sanPham.HinhAnh = "~/Image/" + fileName;
                    sanPham.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Image/"), fileName));
                }
                database.SanPham.Add(sanPham);
                database.SaveChanges();
                return RedirectToAction("DSSanPham");
            }
            catch
            {
                return View();
            }
        }



        public ActionResult Details(int id)
        {
            if (!RoleHelper.IsAdmin() && !RoleHelper.IsEmployee())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var sanPham = database.SanPham.FirstOrDefault(s => s.IDSanPham == id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }

        public ActionResult Edit(int id)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var sanPham = database.SanPham.FirstOrDefault(s => s.IDSanPham == id);
            if (sanPham == null)
            {
                return HttpNotFound();
            }
            return View(sanPham);
        }


        [HttpPost]
        public ActionResult Edit(int id, SanPham sanPham)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            database.Entry(sanPham).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("DSSanPham");
        }

        public ActionResult Delete(int id)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var product = database.SanPham.FirstOrDefault(s => s.IDSanPham == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost]
        public ActionResult Delete(int id, SanPham sanPham)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            try
            {
                sanPham = database.SanPham.Where(s => s.IDSanPham == id).FirstOrDefault();
                database.SanPham.Remove(sanPham);
                database.SaveChanges();
                return RedirectToAction("DSSanPham");
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