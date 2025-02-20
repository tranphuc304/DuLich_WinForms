CREATE TABLE [dbo].[ChuyenDi] (
    [ID_ChuyenDi]   NVARCHAR(50) NOT NULL PRIMARY KEY,
    [TenChuyenDi]   NVARCHAR(255) NOT NULL,
    [HanhTrinh]     NVARCHAR(255) NOT NULL,
    [SoNgayDi]      INT NOT NULL,
    [Gia]           INT NOT NULL,
    [SoLuong]       INT NOT NULL,
    [ChiTiet]       NVARCHAR(MAX) NOT NULL
);