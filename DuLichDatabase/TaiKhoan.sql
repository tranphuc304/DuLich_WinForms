CREATE TABLE [dbo].[TaiKhoan] (
    [ID_TaiKhoan] NVARCHAR(5) NOT NULL PRIMARY KEY,
    [Email]       NVARCHAR(50)  NOT NULL UNIQUE,
    [MatKhau]     NVARCHAR(255) NOT NULL
);