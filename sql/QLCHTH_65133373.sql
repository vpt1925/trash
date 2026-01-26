IF DB_ID('QLCHTH_65133373') IS NOT NULL
BEGIN
    USE master;
    ALTER DATABASE QLCHTH_65133373 SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE QLCHTH_65133373;
END
GO

CREATE DATABASE QLCHTH_65133373;
GO
USE QLCHTH_65133373;
GO

CREATE SEQUENCE SeqMaDanhMuc START WITH 1 INCREMENT BY 1 CACHE 10;
CREATE SEQUENCE SeqMaSP START WITH 1 INCREMENT BY 1 CACHE 20;
CREATE SEQUENCE SeqMaNCC START WITH 1 INCREMENT BY 1 CACHE 10;
CREATE SEQUENCE SeqMaHD START WITH 1 INCREMENT BY 1 CACHE 50;
CREATE SEQUENCE SeqMaTK START WITH 1 INCREMENT BY 1 CACHE 10;
CREATE SEQUENCE SeqMaKM START WITH 1 INCREMENT BY 1 CACHE 10;
GO

CREATE TABLE NhaCungCap (
    MaNCC INT PRIMARY KEY DEFAULT NEXT VALUE FOR SeqMaNCC,
    TenNCC NVARCHAR(150) NOT NULL,
    DiaChi NVARCHAR(255),
    DienThoai VARCHAR(11),
    Email VARCHAR(100),
    CONSTRAINT UK_NhaC_Ten UNIQUE (TenNCC)
);

CREATE TABLE DanhMuc (
    MaDanhMuc INT PRIMARY KEY DEFAULT NEXT VALUE FOR SeqMaDanhMuc,
    TenDanhMuc NVARCHAR(100) NOT NULL,
    MoTa NVARCHAR(255),
    CONSTRAINT UK_DM_Ten UNIQUE (TenDanhMuc)
);

CREATE TABLE TaiKhoan (
    MaTK INT PRIMARY KEY DEFAULT NEXT VALUE FOR SeqMaTK,
    TenDangNhap NVARCHAR(50) NOT NULL,
    MatKhau NVARCHAR(255) NOT NULL,
    LoaiTaiKhoan TINYINT NOT NULL CHECK (LoaiTaiKhoan IN (0, 1)),
    TenHienThi NVARCHAR(50) NOT NULL,
    NgayTao DATETIME NOT NULL DEFAULT GETDATE(),
    GhiChu NVARCHAR(255),
    CONSTRAINT UK_TK_Ten UNIQUE (TenDangNhap)
);

CREATE TABLE SanPham (
    MaSP INT PRIMARY KEY DEFAULT NEXT VALUE FOR SeqMaSP,
    TenSP NVARCHAR(150) NOT NULL,
    MoTaSP NVARCHAR(255),
    DonViTinh NVARCHAR(30) NOT NULL,
    DonGia DECIMAL(18,2) NOT NULL CHECK (DonGia >0),
    SoLuongTon INT NOT NULL DEFAULT 0 CHECK (SoLuongTon >=0),
    AnhSanPham NVARCHAR(200),
    MaDanhMuc INT NOT NULL,
    MaNCC INT NOT NULL,
    CONSTRAINT UK_SP_Ten UNIQUE (TenSP)
);

CREATE TABLE KhuyenMai (
    MaKM INT PRIMARY KEY DEFAULT NEXT VALUE FOR SeqMaKM,
    TenKM NVARCHAR(200) NOT NULL,
    NgayBatDau DATE NOT NULL,
    NgayKetThuc DATE NOT NULL,
    MoTa NVARCHAR(255),
    CONSTRAINT CHK_KM_Ngay CHECK (NgayKetThuc >= NgayBatDau)
);

CREATE TABLE ChiTietKhuyenMai (
    MaKM INT NOT NULL,
    MaSP INT NOT NULL,
    PhanTramGiam DECIMAL(5,2) NOT NULL CHECK (PhanTramGiam >0 AND PhanTramGiam <=100),
    PRIMARY KEY (MaKM, MaSP)
);

CREATE TABLE HoaDon (
    MaHD INT PRIMARY KEY DEFAULT NEXT VALUE FOR SeqMaHD,
    NgayBan DATETIME NOT NULL DEFAULT GETDATE(),
    MaTK INT NOT NULL,
    TongTien DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (TongTien >=0),
    TongGiamGia DECIMAL(18,2) NOT NULL DEFAULT 0,
    ThanhToan DECIMAL(18,2) NOT NULL DEFAULT 0 CHECK (ThanhToan >=0),
    GhiChu NVARCHAR(255)
);

CREATE TABLE ChiTietHoaDon (
    MaHD INT NOT NULL,
    MaSP INT NOT NULL,
    SoLuong INT NOT NULL CHECK (SoLuong >0),
    DonGia DECIMAL(18,2) NOT NULL CHECK (DonGia >0),
    PhanTramGiam DECIMAL(5,2) NOT NULL DEFAULT 0 CHECK (PhanTramGiam BETWEEN 0 AND 100),
    TienGiam DECIMAL(18,2) NOT NULL DEFAULT 0,
    ThanhTien DECIMAL(18,2) NOT NULL CHECK (ThanhTien >=0),
    MaKM INT NULL,
    PRIMARY KEY (MaHD, MaSP)
);
GO

ALTER TABLE SanPham ADD CONSTRAINT FK_SP_DM FOREIGN KEY (MaDanhMuc) REFERENCES DanhMuc(MaDanhMuc);
ALTER TABLE SanPham ADD CONSTRAINT FK_SP_NCC FOREIGN KEY (MaNCC) REFERENCES NhaCungCap(MaNCC);
ALTER TABLE HoaDon ADD CONSTRAINT FK_HD_TK FOREIGN KEY (MaTK) REFERENCES TaiKhoan(MaTK);
ALTER TABLE ChiTietKhuyenMai ADD CONSTRAINT FK_CTKM_KM FOREIGN KEY (MaKM) REFERENCES KhuyenMai(MaKM) ON DELETE CASCADE;
ALTER TABLE ChiTietKhuyenMai ADD CONSTRAINT FK_CTKM_SP FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP);
ALTER TABLE ChiTietHoaDon ADD CONSTRAINT FK_CTHD_HD FOREIGN KEY (MaHD) REFERENCES HoaDon(MaHD) ON DELETE CASCADE;
ALTER TABLE ChiTietHoaDon ADD CONSTRAINT FK_CTHD_SP FOREIGN KEY (MaSP) REFERENCES SanPham(MaSP);
ALTER TABLE ChiTietHoaDon ADD CONSTRAINT FK_CTHD_KM FOREIGN KEY (MaKM, MaSP) REFERENCES ChiTietKhuyenMai(MaKM, MaSP);
GO

CREATE NONCLUSTERED INDEX IX_SP_DM ON SanPham(MaDanhMuc);
CREATE NONCLUSTERED INDEX IX_SP_NCC ON SanPham(MaNCC);
CREATE NONCLUSTERED INDEX IX_SP_Ten ON SanPham(TenSP);
CREATE NONCLUSTERED INDEX IX_HD_Ngay ON HoaDon(NgayBan DESC);
CREATE NONCLUSTERED INDEX IX_HD_TK ON HoaDon(MaTK);
CREATE NONCLUSTERED INDEX IX_CTHD_SP ON ChiTietHoaDon(MaSP);
CREATE NONCLUSTERED INDEX IX_CTKM_SP ON ChiTietKhuyenMai(MaSP);
CREATE NONCLUSTERED INDEX IX_CTKM_KM_INC ON ChiTietKhuyenMai(MaKM) INCLUDE (MaSP, PhanTramGiam);
GO

INSERT INTO DanhMuc (TenDanhMuc, MoTa) VALUES
(N'Bánh kẹo', N'Bánh, kẹo, snack các loại'),
(N'Đồ uống', N'Nước ngọt, trà, sữa, bia...'),
(N'Mì gói - Thực phẩm khô', N'Mì ăn liền, gia vị, mì khô'),
(N'Sữa & sản phẩm từ sữa', N'Sữa tươi, sữa chua, phô mai'),
(N'Dầu ăn & gia vị', N'Dầu ăn, nước mắm, muối, đường'),
(N'Hàng đông lạnh', N'Xúc xích, chả, thịt đông lạnh'),
(N'Hóa mỹ phẩm', N'Dầu gội, xà phòng, kem đánh răng'),
(N'Đồ dùng gia đình', N'Giấy vệ sinh, khăn giấy, túi nilon');
GO

INSERT INTO NhaCungCap (TenNCC, DiaChi, DienThoai, Email) VALUES
(N'Công ty CP Bibica', N'123 Lê Lợi, Q.1, TP.HCM', '02838291234', 'info@bibica.com.vn'),
(N'Coca-Cola Việt Nam', N'KCN Tân Bình, TP.HCM', '02838123456', NULL),
(N'Công ty Acecook', N'KCN VSIP, Bình Dương', '02743789234', NULL),
(N'Vinamilk', N'10 Tân Trào, Q.7, TP.HCM', '02854123456', 'info@vinamilk.com.vn'),
(N'Tập đoàn Masan', N'Quận 1, TP.HCM', '02838291233', NULL),
(N'Unilever Việt Nam', N'KCN Bình Dương', '02743891234', NULL),
(N'Công ty CP Bánh kẹo Orion Việt Nam', N'KCN VSIP 2, Bình Dương', '02743745678', NULL),
(N'Công ty Trung Nguyên Legend', N'Buôn Ma Thuột, Đắk Lắk', '02623851234', 'info@trungnguyen.com.vn'),
(N'Công ty TNHH Oishi Việt Nam', N'KCN Long Hậu, Long An', '02723895678', NULL),
(N'Công ty CP Hàng tiêu dùng Masan', N'Quận 7, TP.HCM', '02854167890', NULL);
GO

INSERT INTO TaiKhoan (TenDangNhap, MatKhau, LoaiTaiKhoan, TenHienThi) VALUES
('admin', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 0, N'Quản trị viên'),
('thungan1', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 1, N'Thu ngân 1'),
('thungan2', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', 1, N'Thu ngân 2');
GO

INSERT INTO SanPham (TenSP, DonViTinh, DonGia, SoLuongTon, AnhSanPham, MaDanhMuc, MaNCC, MoTaSP) VALUES
(N'Bánh Oreo vị socola', N'Gói', 15000, 120, 'oreo.jpg', 1, 1, N'Bánh quy Oreo kem socola thơm ngon, giòn tan'),
(N'Kẹo Chupa Chups', N'Viến', 3000, 500, 'chupa.jpg', 1, 1, N'Kẹo mút trái cây Chupa Chups chính hãng'),
(N'Snack khoai tây Lays', N'Gói', 12000, 80, 'lays.jpg', 1, 6, N'Khoai tây chiên giòn rụm Lays Classic'),
(N'Coca Cola 390ml', N'Lon', 10000, 200, 'coca.jpg', 2, 2, N'Nước ngọt có gas Coca Cola lon cao'),
(N'Pepsi 330ml', N'Lon', 9000, 150, 'pepsi.jpg', 2, 2, N'Pepsi vị cola đậm đà'),
(N'Trà xanh C2', N'Chai', 8000, 180, 'c2.jpg', 2, 8, N'Trà xanh C2 hương chanh đào'),
(N'Sting dâu', N'Chai', 10000, 220, 'sting.jpg', 2, 2, N'Nước tăng lực Sting dâu đỏ'),
(N'Sữa tươi Vinamilk có đường 1L', N'Hộp', 35000, 100, 'suatuoivinamilk.jpg', 4, 4, N'Sữa tươi tiệt trùng Vinamilk 100%'),
(N'Sữa chua Vinamilk', N'Hộp 4', 28000, 90, 'suachua.jpg', 4, 4, N'Sữa chua ăn Vinamilk hộp 4 hũ'),
(N'Mì Hảo Hảo tôm chua cay', N'Gói', 4500, 500, 'haohao.jpg', 3, 3, N'Mì Hảo Hảo tôm chua cay quốc dân'),
(N'Mì Omachi xốt bò', N'Gói', 9000, 300, 'omachi.jpg', 3, 3, N'Mì cao cấp Omachi sợi dai'),
(N'Mì Kokomi đại', N'Gói', 5500, 400, 'kokomi.jpg', 3, 3, N'Mì Kokomi tôm chua cay gói lớn'),
(N'Dầu ăn Tường An', N'Chai 1L', 45000, 80, 'dautuongan.jpg', 5, 5, N'Dầu ăn cao cấp Tường An'),
(N'Nước mắm Nam Ngư', N'Chai', 32000, 120, 'nammuong.jpg', 5, 8, N'Nước mắm Nam Ngư độ đạm 40N'),
(N'Xúc xích CP', N'Gói 5 cây', 25000, 150, 'xucxichcp.jpg', 6, 5, N'Xúc xích heo CP 5 cây/gói'),
(N'Chả lụa Vissan', N'Kg', 180000, 30, 'chalua.jpg', 6, 5, N'Chả lụa truyền thống Vissan'),
(N'Dầu gội Head & Shoulders', N'Chai', 95000, 60, 'headshoulders.jpg', 7, 6, N'Dầu gội Head & Shoulders trị gàu'),
(N'Xà bông Lifebuoy', N'Bánh', 15000, 200, 'lifebuoy.jpg', 7, 6, N'Xà bông Lifebuoy bảo vệ 10 giờ'),
(N'Giấy vệ sinh Premier', N'Cuộn', 8000, 300, 'giayvesinh.jpg', 8, 6, N'Giấy vệ sinh Premier mềm mịn'),
(N'Khăn giấy Pulppy', N'Gói', 22000, 180, 'pulppy.jpg', 8, 6, N'Khăn giấy Pulppy cao cấp'),
(N'Bánh gạo One One', N'Gói', 18000, 140, 'oneone.jpg', 1, 1, N'Bánh gạo lứt One One ăn kiêng'),
(N'Trà Ô Long Tea+', N'Chai', 11000, 160, 'tea+.jpg', 2, 8, N'Trà ô long Tea+ không đường'),
(N'Sữa đậu nành Fami', N'Hộp', 6000, 300, 'fami.jpg', 4, 4, N'Sữa đậu nành Fami Canxi'),
(N'Mì 3 Miền Gold', N'Gói', 6500, 280, '3mien.jpg', 3, 3, N'Mì 3 Miền Gold tôm chua cay'),
(N'Bia Sài Gòn Export', N'Lon', 16000, 250, 'saigon.jpg', 2, 8, N'Bia Sài Gòn Export vị đậm'),
(N'Red Bull', N'Lon', 18000, 100, 'redbull.jpg', 2, 2, N'Nước tăng lực Red Bull'),
(N'Nước suối Aquafina', N'Chai 500ml', 6000, 400, 'aquafina.jpg', 2, 2, N'Nước tinh khiết Aquafina'),
(N'Bánh ChocoPie', N'Hộp 12 cái', 45000, 90, 'chocopie.jpg', 1, 7, N'Bánh ChocoPie Orion nhân marshmallow'),
(N'Kem đánh răng P/S', N'Tuýp', 28000, 120, 'ps.jpg', 7, 6, N'Kem đánh răng P/S ngừa sâu răng'),
(N'Bim bim Oishi', N'Gói', 5000, 350, 'oishi.jpg', 1, 9, N'Snack tôm Oishi ăn vặt'),
(N'Sữa đặc Ông Thọ', N'Lon', 22000, 200, 'ongtho.jpg', 4, 4, N'Sữa đặc Ông Thọ nhãn xanh'),
(N'Cà phê G7 hòa tan', N'Hộp 16 gói', 38000, 110, 'g7.jpg', 2, 10, N'Cà phê G7 3in1 Trung Nguyên'),
(N'Nước rửa chén Sunlight', N'Túi', 25000, 150, 'sunlight.jpg', 7, 6, N'Nước rửa chén Sunlight chanh'),
(N'Bột giặt Omo', N'Túi 3kg', 95000, 70, 'omo.jpg', 7, 6, N'Bột giặt Omo sạch sâu thơm lâu'),
(N'Thùng mì Omachi 30 gói', N'Thùng', 250000, 25, 'thungomachi.jpg', 3, 3, N'Thùng 30 gói mì Omachi tiết kiệm');
GO