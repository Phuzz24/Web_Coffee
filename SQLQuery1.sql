use DBCoffee
CREATE TABLE NguoiDung
(
  IDNguoiDung INT identity(1,1),
  Role NVARCHAR(30) NOT NULL,
  UserName NVARCHAR(50) NOT NULL,
  Pass NVARCHAR(50) NOT NULL,
  PRIMARY KEY (IDNguoiDung)
);

CREATE TABLE NhaCungCap
(
  IDNCC INT identity(1,1),
  TenNCC NVARCHAR(50) NOT NULL,
  DiaChiNCC NVARCHAR(50) NOT NULL,
  PRIMARY KEY (IDNCC)
);

CREATE TABLE Category
(
  IDCategory INT identity(1,1),
  TenLoai NVARCHAR(50) NOT NULL,
  PRIMARY KEY (IDCategory)
);

CREATE TABLE KhachHang
(
  IDKhachHang INT identity(1,1),
  CodeKhachHang CHAR(20) NOT NULL,
  HoTenKH NVARCHAR(50) NOT NULL,
  DiaChiKH NVARCHAR(50) NOT NULL,
  GmailKH NVARCHAR(50) NOT NULL,
  SDTKH CHAR(11) NOT NULL,
  IDNguoiDung INT NOT NULL,
  PRIMARY KEY (CodeKhachHang),
  FOREIGN KEY (IDNguoiDung) REFERENCES NguoiDung(IDNguoiDung)
);

CREATE TABLE Admin1
(
  IDAdmin INT identity(1,1),
  HoTenAdmin NVARCHAR(50) NOT NULL,
  SDTAdmin CHAR(11) NOT NULL,
  DiaChiAdmin NVARCHAR(50) NOT NULL,
  GmailAdmin NVARCHAR(50) NOT NULL,
  IDNguoiDung INT NOT NULL,
  PRIMARY KEY (IDAdmin),
  FOREIGN KEY (IDNguoiDung) REFERENCES NguoiDung(IDNguoiDung)
);

CREATE TABLE NhanVien
(
  IDNhanVien INT identity(1,1),
  HoTenNV NVARCHAR(50) NOT NULL,
  DiaChiNV NVARCHAR(50) NOT NULL,
  RoleNV NVARCHAR(50) NOT NULL,
  GmaiNV NVARCHAR(50) NOT NULL,
  SdtNV CHAR(11) NOT NULL,
  IDNguoiDung INT NOT NULL,
  PRIMARY KEY (IDNhanVien),
  FOREIGN KEY (IDNguoiDung) REFERENCES NguoiDung(IDNguoiDung)
);

CREATE TABLE SanPham
(
  IDSanPham INT identity(1,1),
  CodeSanPham CHAR(20) NOT NULL,
  TenSanPham NVARCHAR(50) NOT NULL,
  Gia FLOAT NOT NULL,
  HinhAnh NVARCHAR(100) NOT NULL,
  Size CHAR(5) NOT NULL,
  IDCategory INT NOT NULL,
  IDNCC INT NOT NULL,
  PRIMARY KEY (CodeSanPham),
  FOREIGN KEY (IDCategory) REFERENCES Category(IDCategory),
  FOREIGN KEY (IDNCC) REFERENCES NhaCungCap(IDNCC)
);

CREATE TABLE HoaDon
(
  IDHoaDon INT identity(1,1),
  NgayBan DATE NOT NULL,
  TongTien FLOAT NOT NULL,
  IDNhanVien INT NOT NULL,
  CodeKhachHang CHAR(20) NOT NULL,
  PRIMARY KEY (IDHoaDon),
  FOREIGN KEY (IDNhanVien) REFERENCES NhanVien(IDNhanVien),
  FOREIGN KEY (CodeKhachHang) REFERENCES KhachHang(CodeKhachHang)
);

CREATE TABLE ChiTietHoaDon
(
  ID INT identity(1,1),
  TongTien FLOAT NOT NULL,
  SoLuong INT NOT NULL,
  Gia FLOAT NOT NULL,
  CodeSanPham CHAR(20) NOT NULL,
  IDHoaDon INT NOT NULL,
  PRIMARY KEY (ID),
  FOREIGN KEY (CodeSanPham) REFERENCES SanPham(CodeSanPham),
  FOREIGN KEY (IDHoaDon) REFERENCES HoaDon(IDHoaDon)
);

CREATE TABLE PhieuGiamGia
(
  IDPhieuGiam INT identity(1,1),
  TongTienGiam FLOAT NOT NULL,
  NgayHetHan DATE NOT NULL,
  IDHoaDon INT NOT NULL,
  PRIMARY KEY (IDPhieuGiam),
  FOREIGN KEY (IDHoaDon) REFERENCES HoaDon(IDHoaDon)
);
