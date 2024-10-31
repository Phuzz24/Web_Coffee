using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web_Coffee
{
    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            // Đăng ký tất cả các khu vực và routes khi ứng dụng bắt đầu.
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
