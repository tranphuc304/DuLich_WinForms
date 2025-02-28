CREATE TABLE [dbo].[DanhSachDuKhach] (
    [ID_ChuyenDi]   NVARCHAR(50) NOT NULL,
    [NgayBatDau]    DATE NOT NULL,
    [CCCD]          NVARCHAR(12) NOT NULL,
    [Ten]           NVARCHAR(100) NOT NULL,
    [SDT]           NVARCHAR(20) NOT NULL,
    [ID_TaiKhoan]   NVARCHAR(5) NOT NULL,
    PRIMARY KEY ([ID_ChuyenDi], [NgayBatDau], [CCCD]),
    FOREIGN KEY ([ID_ChuyenDi]) REFERENCES [dbo].[ChuyenDi]([ID_ChuyenDi])
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY ([ID_TaiKhoan]) REFERENCES [dbo].[TaiKhoan]([ID_TaiKhoan])
        ON DELETE CASCADE ON UPDATE CASCADE
);