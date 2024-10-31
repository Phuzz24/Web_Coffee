using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web_Coffee.Models;
using Web_Coffee.Models.Web_Coffee.Models;

public class ThongKeController : Controller
{
    DBCoffeeEntities database = new DBCoffeeEntities();
    public ActionResult Index()
    {
        var model = new ThongKeView();

        // Lấy tổng số khách hàng
        model.TongSLKhachHang = database.KhachHang.Count();

        // Lấy tổng doanh thu và ép kiểu từ double sang decimal
        model.TongDoanhThu = (decimal)database.HoaDon.Sum(h => h.TongTien);

        // Lấy thống kê đồ uống được mua
        model.DSMonNuocDuocMua = database.ChiTietHoaDon
            .GroupBy(c => c.SanPham.TenSanPham)
            .Select(g => new ThongKeSanPham
            {
                TenSanPham = g.Key,
                SoLuongBan = g.Sum(c => c.SoLuong)
            })
            .ToList();

        // Lấy đồ uống được mua nhiều nhất
        model.DSNuocDuocMuaNhieuNhat = model.DSMonNuocDuocMua
            .OrderByDescending(d => d.SoLuongBan)
            .FirstOrDefault();

        return View(model);
    }
}

