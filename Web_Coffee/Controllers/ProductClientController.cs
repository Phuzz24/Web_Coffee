using System.Web.Mvc;
using Web_Coffee.Models;
using System.Linq;

namespace Web_Coffee.Controllers
{
    public class ProductClientController : Controller
    {
        DBCoffeeEntities database = new DBCoffeeEntities();

        public ActionResult Index()
        {
            var categories = database.Category.ToList();
            ViewBag.Categories = categories;

            var products = database.SanPham.ToList();
            return View(products);
        }
    }
}
