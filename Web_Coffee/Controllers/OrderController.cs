using System;
using System.Linq;
using System.Web.Mvc;
using Web_Coffee.Models;
using PagedList;

namespace Web_Coffee.Controllers
{
    public class OrderController : Controller
    {
        DBCoffeeEntities database = new DBCoffeeEntities();

        // Danh sách đơn hàng với tìm kiếm
        public ActionResult DSOrder(string searchString, int? page)
        {
            // Kiểm tra quyền truy cập: admin và nhân viên được phép
            if (!RoleHelper.IsAdmin() && !RoleHelper.IsEmployee())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var orders = database.HoaDon.AsQueryable();

            // Tìm kiếm theo mã đơn hàng hoặc tên khách hàng
            if (!string.IsNullOrEmpty(searchString))
            {
                if (int.TryParse(searchString, out int searchID))
                {
                    orders = orders.Where(o => o.IDHoaDon == searchID);
                }
                else
                {
                    orders = orders.Where(o => o.TenKhachHang.Contains(searchString) ||
                                               o.CodeKhachHang.Contains(searchString));
                }
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            // Chuyển đổi `orders` thành kiểu `IPagedList`
            var pagedList = orders.OrderByDescending(o => o.NgayBan).ToPagedList(pageNumber, pageSize);

            return View(pagedList);
        }


        // Xem chi tiết đơn hàng
        public ActionResult Details(int id)
        {
            if (!RoleHelper.IsAdmin() && !RoleHelper.IsEmployee())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var order = database.HoaDon.SingleOrDefault(o => o.IDHoaDon == id);
            if (order == null)
            {
                return HttpNotFound("Không tìm thấy đơn hàng.");
            }

            var orderDetails = database.ChiTietHoaDon.Where(d => d.IDHoaDon == id).ToList();
            ViewBag.Order = order;
            return View(orderDetails);
        }

        // Tạo mới đơn hàng (chỉ admin được phép)
        public ActionResult Create()
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(HoaDon order)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            order.NgayBan = DateTime.Now;
            database.HoaDon.Add(order);
            database.SaveChanges();
            return RedirectToAction("DSOrder");
        }

        // Chỉnh sửa đơn hàng (chỉ admin được phép)
        public ActionResult Edit(int id)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var order = database.HoaDon.SingleOrDefault(o => o.IDHoaDon == id);
            if (order == null)
            {
                return HttpNotFound("Không tìm thấy đơn hàng.");
            }

            return View(order);
        }

        [HttpPost]
        public ActionResult Edit(int id, HoaDon order)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            database.Entry(order).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("DSOrder");
        }

        // Xóa đơn hàng (chỉ admin được phép)
        public ActionResult Delete(int id)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var order = database.HoaDon.SingleOrDefault(o => o.IDHoaDon == id);
            if (order == null)
            {
                return HttpNotFound("Không tìm thấy đơn hàng.");
            }

            return View(order);
        }

        [HttpPost]
        public ActionResult Delete(int id, HoaDon order)
        {
            if (!RoleHelper.IsAdmin())
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            order = database.HoaDon.SingleOrDefault(o => o.IDHoaDon == id);
            database.HoaDon.Remove(order);
            database.SaveChanges();
            return RedirectToAction("DSOrder");
        }
    }
}
