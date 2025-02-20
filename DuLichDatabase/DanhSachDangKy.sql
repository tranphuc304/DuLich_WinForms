CREATE TABLE [dbo].[DanhSachDangKy] (
    [ID_TaiKhoan]   NVARCHAR(5) NOT NULL,
    [ID_ChuyenDi]   NVARCHAR(50) NOT NULL,
    [NgayBatDau]    DATE NOT NULL,
    [SoLuong]       INT NOT NULL,
    [TrangThai]     NVARCHAR(50) NOT NULL,
    PRIMARY KEY ([ID_TaiKhoan], [ID_ChuyenDi], [NgayBatDau]),
    FOREIGN KEY ([ID_TaiKhoan]) REFERENCES [dbo].[TaiKhoan]([ID_TaiKhoan]) 
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY ([ID_ChuyenDi]) REFERENCES [dbo].[ChuyenDi]([ID_ChuyenDi])
        ON DELETE CASCADE ON UPDATE CASCADE
);