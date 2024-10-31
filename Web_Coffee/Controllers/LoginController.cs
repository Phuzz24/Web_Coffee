using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Coffee.Models;

namespace Web_Coffee.Controllers
{
    public class LoginController : Controller
    {
        private DBCoffeeEntities database = new DBCoffeeEntities();

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult AuthenLogin(NguoiDung nguoiDung)
        {
            // Kiểm tra tên đăng nhập và mật khẩu
            if (string.IsNullOrEmpty(nguoiDung.UserName))
            {
                ViewBag.ErrorName = "Vui lòng nhập tên đăng nhập.";
                return View("Login");
            }

            if (string.IsNullOrEmpty(nguoiDung.Pass))
            {
                ViewBag.ErrorPass = "Vui lòng nhập mật khẩu.";
                return View("Login");
            }

            try
            {
                // Kiểm tra thông tin đăng nhập từ cơ sở dữ liệu
                var user = database.NguoiDung
                    .FirstOrDefault(s => s.UserName == nguoiDung.UserName && s.Pass == nguoiDung.Pass);

                if (user == null)
                {
                    ViewBag.ErrorMessage = "Sai thông tin đăng nhập. Xin kiểm tra lại.";
                    return View("Login");
                }
                else
                {
                    // Thiết lập các giá trị Session cho người dùng
                    Session["User"] = user; // Lưu toàn bộ đối tượng `NguoiDung` vào Session
                    Session["UserID"] = user.IDNguoiDung;
                    Session["UserName"] = user.UserName;
                    Session["UserRole"] = user.Role;

                    // Điều hướng dựa trên vai trò của người dùng
                    switch (user.Role)
                    {
                        case "Admin":
                            return RedirectToAction("DSSanPham", "Product");
                        case "Nhân viên":
                            return RedirectToAction("DSSanPham", "Product");
                        case "Khách hàng":
                            return RedirectToAction("Index", "ProductClient");
                        default:
                            ViewBag.ErrorMessage = "Vai trò người dùng không xác định.";
                            return View("Login");
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Có lỗi xảy ra trong quá trình đăng nhập. Chi tiết: " + ex.Message;
                return View("Login");
            }
        }


        public ActionResult Register()
        {
            return View();
        }

        public ActionResult AuthenRegister(NguoiDung nguoiDung)
        {
            try
            {
                database.NguoiDung.Add(nguoiDung);
                database.SaveChanges();
                return RedirectToAction("Login");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult TestSession()
        {
            if (Session["User"] != null)
            {
                var user = Session["User"] as NguoiDung;
                return Content($"User ID: {user.IDNguoiDung}, User Name: {user.UserName}, User Role: {user.Role}");
            }
            else
            {
                return Content("Session is null");
            }
        }

        // Logout function
        public ActionResult Logout()
        {
            // Clear user session
            Session.Clear();

            // Redirect to login page
            return RedirectToAction("Login");
        }
    }
}
