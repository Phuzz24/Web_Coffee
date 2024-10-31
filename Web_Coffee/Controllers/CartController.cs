using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Coffee.Models;

namespace Web_Coffee.Controllers
{
    public class CartController : Controller
    {
        DBCoffeeEntities database = new DBCoffeeEntities();
        public ActionResult Index()
        {
            ViewBag.customers = database.KhachHang.ToList();
            ViewBag.products = database.SanPham.ToList();
            return View((List<Cart>)HttpContext.Session["cart"]);
        }
    }
}