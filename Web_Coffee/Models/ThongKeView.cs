using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Coffee.Models
{
    namespace Web_Coffee.Models
    {
        public class ThongKeView
        {
            public int TongSLKhachHang { get; set; }
            public decimal TongDoanhThu { get; set; }
            public List<ThongKeSanPham> DSMonNuocDuocMua { get; set; }
            public ThongKeSanPham DSNuocDuocMuaNhieuNhat { get; set; }
        }

        public class ThongKeSanPham
        {
            public string TenSanPham { get; set; }
            public int SoLuongBan { get; set; }
        }
    }


}
