CREATE TABLE HoaDon (
    ID_HoaDon INT IDENTITY(1,1) PRIMARY KEY,
    ID_TaiKhoan NVARCHAR(5) NOT NULL,
    ID_ChuyenDi NVARCHAR(50) NOT NULL,
    NgayBatDau DATE NOT NULL,
    SoLuong INT NOT NULL CHECK (SoLuong > 0),
    TongTien INT NOT NULL CHECK (TongTien >= 0),
    TrangThai NVARCHAR(50) COLLATE Vietnamese_CI_AS NOT NULL,
    FOREIGN KEY (ID_TaiKhoan) REFERENCES TaiKhoan(ID_TaiKhoan),
    FOREIGN KEY (ID_ChuyenDi, NgayBatDau) REFERENCES LichTrinh(ID_ChuyenDi, NgayBatDau)
);