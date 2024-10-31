using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Coffee.Models
{
    public class ProductView
    {
        public int IDSanPham{ get; set; }
        public string TenSanPham { get; set; }
        public string TenNCC { get;set; }
        public int SoLuongSanPham { get; set; }
        public double GiaSanPham { get; set; }
        public string CodeSanPham { get; set; }
        public string SizeSanPham { get; set; }
        public string AnhSanPham { get; set; }


    }
}