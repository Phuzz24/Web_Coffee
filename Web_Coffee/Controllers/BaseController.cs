using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_Coffee.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            var userRole = Session["UserRole"]?.ToString();

            if (userRole == "Admin" && userRole=="Nhân viên" || userRole == "Khách hàng")
            {
                ViewBag.Layout = "/View/Shared/_AdminLayout";
            }
            else
            {
                ViewBag.Layout = "/Views/Shared/_HomeLayout";
            }
        }
    }

}