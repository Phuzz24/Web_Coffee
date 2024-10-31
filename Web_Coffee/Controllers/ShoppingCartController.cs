using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Schema;
using Web_Coffee.Models;

namespace Web_Coffee.Controllers
{
    public class ShoppingCartController : Controller
    {
        DBCoffeeEntities database = new DBCoffeeEntities();
        public Cart GetCart()
        {
            Cart cart = Session["Cart"] as Cart;
            if (cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        // add item và giỏ hàng
        public ActionResult AddtoCart(int id)
        {
            var pro = database.SanPham.SingleOrDefault(s => s.IDSanPham == id);
            if (pro != null)
            {
                GetCart().Add(pro);
            }
            return RedirectToAction("ShowToCart", "ShoppingCart");

        }
        //Trang giỏ hàng
        public ActionResult ShowToCart()
        {
            if (Session["Cart"] == null)
                return RedirectToAction("ShowToCart", "ShoppingCart");
            Cart cart = Session["Cart"] as Cart;
            return View(cart);
        }
        public ActionResult Update_Quantity_Cart(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            int id_pro = int.Parse(form["ID_Product"]);
            int quantity = int.Parse(form["Quantity"]);
            cart.Update_Quantity_Shopping(id_pro, quantity);
            return RedirectToAction("ShowToCart", "ShoppingCart");
        }
        private string GetCustomerCode()
        {
            if (Session["User"] is NguoiDung user)
            {
                // Truy xuất khách hàng dựa trên `IDNguoiDung` của người dùng
                var customer = database.KhachHang.SingleOrDefault(k => k.IDNguoiDung == user.IDNguoiDung);

                if (customer != null)
                {
                    return customer.CodeKhachHang;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Không tìm thấy khách hàng với IDNguoiDung: {user.IDNguoiDung}");
                    return null;
                }
            }
            System.Diagnostics.Debug.WriteLine("Session[\"User\"] is null hoặc không phải là kiểu NguoiDung");
            return null;
        }
        // Giả sử có hàm lấy mã khách hàng hiện tại từ phiên
        public ActionResult OrderInformation()
        {
            if (Session["User"] == null)
            {
                return Content("Lỗi: Người dùng chưa đăng nhập.");
            }

            Cart cart = Session["Cart"] as Cart;
            if (cart == null || !cart.Items.Any())
            {
                return RedirectToAction("ShowToCart");
            }

            string codeCustomer = GetCustomerCode();
            if (string.IsNullOrEmpty(codeCustomer))
            {
                System.Diagnostics.Debug.WriteLine("Không thể xác định mã khách hàng vì `codeCustomer` null hoặc rỗng.");
                return Content("Lỗi. Không thể xác định mã khách hàng.");
            }

            var customer = database.KhachHang.SingleOrDefault(k => k.CodeKhachHang == codeCustomer);
            if (customer != null)
            {
                ViewBag.CustomerCode = customer.CodeKhachHang;
                ViewBag.CustomerName = customer.HoTenKH;
                ViewBag.CustomerAddress = customer.DiaChiKH;
                ViewBag.CustomerPhone = customer.SDTKH;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Lỗi: Thông tin khách hàng không hợp lệ.");
                return Content("Lỗi. Thông tin khách hàng không hợp lệ.");
            }

            return View();
        }


        public ActionResult TestSession()
        {
            if (Session["UserID"] != null)
            {
                return Content($"User ID: {Session["UserID"]}, User Name: {Session["UserName"]}, User Role: {Session["UserRole"]}");
            }
            else
            {
                return Content("Session is null");
            }
        }











        [HttpPost]
        public ActionResult CheckOut(FormCollection form)
        {
            try
            {
                // Kiểm tra giỏ hàng có tồn tại không và có sản phẩm nào không
                Cart cart = Session["Cart"] as Cart;
                if (cart == null || !cart.Items.Any())
                {
                    return RedirectToAction("Index", "ProductClient");
                }

                // Kiểm tra mã khách hàng
                string codeCustomer = GetCustomerCode();
                if (string.IsNullOrEmpty(codeCustomer))
                {
                    return Content("Lỗi. Không thể xác định mã khách hàng.");
                }

                // Lấy thông tin khách hàng từ form
                string customerName = form["CustomerName"];
                string customerAddress = form["CustomerAddress"];
                string customerPhone = form["CustomerPhone"];

                // Kiểm tra thông tin khách hàng hợp lệ
                if (string.IsNullOrEmpty(customerName) || string.IsNullOrEmpty(customerAddress) || string.IsNullOrEmpty(customerPhone))
                {
                    return Content("Lỗi. Vui lòng kiểm tra lại thông tin nhập vào.");
                }

                // Tạo đối tượng hoá đơn mới
                HoaDon order = new HoaDon
                {
                    CodeKhachHang = codeCustomer,
                    TenKhachHang = customerName,
                    DiaChi = customerAddress,
                    SDT = customerPhone,
                    NgayBan = DateTime.Now,
                    TongTien = cart.Total_Money(),
                    TrangThai = "Đang xử lí"
                };

                // Thêm hoá đơn vào cơ sở dữ liệu
                database.HoaDon.Add(order);
                database.SaveChanges();

                // Thêm chi tiết hoá đơn cho từng sản phẩm trong giỏ hàng
                foreach (var item in cart.Items)
                {
                    ChiTietHoaDon orderDetail = new ChiTietHoaDon
                    {
                        IDHoaDon = order.IDHoaDon,
                        TongTien = order.TongTien,
                        CodeSanPham = item._shopping_product.CodeSanPham,
                        Gia = item._shopping_product.Gia,
                        SoLuong = item._shopping_quantity
                    };
                    database.ChiTietHoaDon.Add(orderDetail);
                }
                database.SaveChanges();

                // Xoá giỏ hàng sau khi hoàn tất thanh toán
                cart.ClearCart();

                // Chuyển hướng đến trang thành công với mã hoá đơn
                return RedirectToAction("Shopping_Success", new { idHoaDon = order.IDHoaDon });
            }
            catch (Exception ex)
            {
                return Content("Lỗi. Vui lòng kiểm tra lại thông tin. Chi tiết lỗi: " + ex.Message);
            }
        }







        public ActionResult RemoveCart(int ID_Product)
        {
            var cart = (Cart)Session["Cart"];
            if (cart != null)
            {
                cart.Remove_CartItem(ID_Product);
            }
            return RedirectToAction("ShowToCart");
        }
        public PartialViewResult BagCart()
        {
            int _t_item = 0;
            Cart cart = Session["Cart"] as Cart;
            if (cart != null)
            {
                _t_item = cart.Total_Quantity();
            }
            ViewBag.infoCart = _t_item;
            return PartialView("BagCart");
        }
        public ActionResult Shopping_Success(int idHoaDon)
        {
            var hoaDon = database.HoaDon.Find(idHoaDon);
            if (hoaDon == null)
            {
                return HttpNotFound();
            }
            return View(hoaDon);
        }
       
        }
    }
