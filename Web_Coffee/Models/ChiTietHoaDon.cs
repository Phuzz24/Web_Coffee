//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web_Coffee.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ChiTietHoaDon
    {
        public int ID { get; set; }
        public double TongTien { get; set; }
        public int SoLuong { get; set; }
        public double Gia { get; set; }
        public string CodeSanPham { get; set; }
        public int IDHoaDon { get; set; }
    
        public virtual SanPham SanPham { get; set; }
        public virtual HoaDon HoaDon { get; set; }
    }
}
