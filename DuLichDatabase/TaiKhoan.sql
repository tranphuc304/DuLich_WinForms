CREATE TABLE [dbo].[TaiKhoan] (
    [Email]       NVARCHAR (50)  NOT NULL PRIMARY KEY,
    [MatKhau]     NVARCHAR (255) NOT NULL,
    [ID_TaiKhoan] INT IDENTITY(1,1) NOT NULL UNIQUE
);