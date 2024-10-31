using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_Coffee.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View("Home");
        }

        public ActionResult GioiThieu_NguonGoc()
        {
            return View();
        }

        public ActionResult GioiThieu_DichVu()
        {
            return View();
        }

        public ActionResult GioiThieu_NgheNghiep()
        {
            return View();
        }

        public ActionResult DieuKhoan_SuDung()
        {
            return View();
        }

        public ActionResult ChinhSach_BaoMat()
        {
            return View();
        }

        public ActionResult TinTuc()
        {
            return View();
        }

        public ActionResult LienHe()
        {
            return View();
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        public ActionResult Home()
        {
            return View();
        }
        public ActionResult KhuyenMai()
        {
            return View();
        }
        public ActionResult GioiThieuSanPham()
        {
            return View();
        }
        public ActionResult AccessDenied()
        {
            return View();
        }
        public ActionResult DanhGia()
        {
            return View();
        }
        public ActionResult DanhGiaThanhCong()
        {
            return View();
        }
    }
}