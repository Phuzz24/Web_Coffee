using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Coffee.Models;
using PagedList;
namespace Web_Coffee.Controllers
{
    public class UserController : Controller
    {
        DBCoffeeEntities database = new DBCoffeeEntities();

        public ActionResult DSNguoiDung(string searchString, int? page)
        {
            if (!RoleHelper.IsAdmin() && !RoleHelper.IsEmployee())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var nguoiDungs = database.NguoiDung.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                // Chuyển đổi searchString thành số nguyên
                if (int.TryParse(searchString, out int searchID))
                {
                    nguoiDungs = nguoiDungs.Where(u => u.IDNguoiDung == searchID);
                }

                nguoiDungs = nguoiDungs.Where(u => u.UserName.Contains(searchString) ||
                                                    u.Role.Contains(searchString) ||
                                                    u.Pass.Contains(searchString));
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            var pagedList = nguoiDungs.OrderBy(u => u.UserName).ToPagedList(pageNumber, pageSize);

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
        public ActionResult Create(NguoiDung nguoiDung)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            database.NguoiDung.Add(nguoiDung);
            database.SaveChanges();
            return RedirectToAction("DSNguoiDung");
        }

        public ActionResult Details(int id)
        {
            if (!RoleHelper.IsAdmin() && !RoleHelper.IsEmployee())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            return View(database.NguoiDung.Where(s => s.IDNguoiDung == id).FirstOrDefault());
        }

        public ActionResult Edit(int id)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            return View(database.NguoiDung.Where(s => s.IDNguoiDung == id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult Edit(int id, NguoiDung nguoiDung)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            database.Entry(nguoiDung).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("DSNguoiDung");
        }

        public ActionResult Delete(int id)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            return View(database.NguoiDung.Where(s => s.IDNguoiDung == id).FirstOrDefault());
        }

        [HttpPost]
        public ActionResult Delete(int id, NguoiDung nguoiDung)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            nguoiDung = database.NguoiDung.Where(s => s.IDNguoiDung == id).FirstOrDefault();
            database.NguoiDung.Remove(nguoiDung);
            database.SaveChanges();
            return RedirectToAction("DSNguoiDung");
        }
        // Action xem lịch sử đơn hàng
        public ActionResult OrderHistory()
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Login"); // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
            }

            // Lấy ID của người dùng hiện tại từ Session
            int userId = (int)Session["UserID"];

            // Lấy mã khách hàng tương ứng với người dùng
            var customer = database.KhachHang.SingleOrDefault(k => k.IDNguoiDung == userId);
            if (customer == null)
            {
                return Content("Lỗi: Không tìm thấy thông tin khách hàng.");
            }

            // Lấy danh sách đơn hàng của khách hàng
            var orders = database.HoaDon
                .Where(o => o.CodeKhachHang == customer.CodeKhachHang)
                .OrderByDescending(o => o.NgayBan)
                .ToList();

            return View(orders);
        }
        public ActionResult OrderDetails(int id)
        {
            // Lấy thông tin đơn hàng theo ID
            var order = database.HoaDon.SingleOrDefault(o => o.IDHoaDon == id);

            if (order == null)
            {
                return HttpNotFound("Không tìm thấy đơn hàng.");
            }

            // Lấy chi tiết các sản phẩm trong đơn hàng
            var orderDetails = database.ChiTietHoaDon
                .Where(d => d.IDHoaDon == id)
                .ToList();

            ViewBag.Order = order;
            return View(orderDetails);
        }

    }
}

