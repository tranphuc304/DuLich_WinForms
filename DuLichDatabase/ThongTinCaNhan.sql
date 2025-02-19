CREATE TABLE [dbo].[ThongTinCaNhan] (
    [ID_TaiKhoan]  INT            NOT NULL PRIMARY KEY,
    [HoTen]        NVARCHAR (100) NOT NULL,
    [SDT]          NVARCHAR (20)  NOT NULL,
    [DiaChi]       NVARCHAR (255) NOT NULL,
    CONSTRAINT FK_ThongTinCaNhan_TaiKhoan 
        FOREIGN KEY ([ID_TaiKhoan]) 
        REFERENCES [dbo].[TaiKhoan] ([ID_TaiKhoan]) 
        ON DELETE CASCADE ON UPDATE CASCADE
);