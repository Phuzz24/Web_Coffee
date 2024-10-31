using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Coffee.Models
{
    public static class RoleHelper
    {
        public static bool IsAdmin()
        {
            return HttpContext.Current.Session["UserRole"] != null &&
                   HttpContext.Current.Session["UserRole"].ToString() == "Admin";
        }

        public static bool IsEmployee()
        {
            return HttpContext.Current.Session["UserRole"] != null &&
                   HttpContext.Current.Session["UserRole"].ToString() == "Nhân viên";
        }

        public static bool IsCustomer()
        {
            return HttpContext.Current.Session["UserRole"] != null &&
                   HttpContext.Current.Session["UserRole"].ToString() == "Khách hàng";
        }
    }

}