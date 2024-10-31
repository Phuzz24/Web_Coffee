using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_Coffee.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult DangNhap()
        {
            return View();
        }

        public ActionResult DangKy()
        {
            return View();
        }

        public ActionResult QuenMatKhau()
        {
            return View();
        }
    }
}