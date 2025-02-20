CREATE TABLE [dbo].[DanhGia] (
    [ID_ChuyenDi]   NVARCHAR(50) NOT NULL,
    [ID_TaiKhoan]   NVARCHAR(5) NOT NULL,
    [SoSao]         INT CHECK ([SoSao] BETWEEN 1 AND 5) NOT NULL,
    [BinhLuan]      NVARCHAR(MAX) NULL,
    PRIMARY KEY ([ID_ChuyenDi], [ID_TaiKhoan]),
    FOREIGN KEY ([ID_ChuyenDi]) REFERENCES [dbo].[ChuyenDi]([ID_ChuyenDi])
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY ([ID_TaiKhoan]) REFERENCES [dbo].[TaiKhoan]([ID_TaiKhoan])
        ON DELETE CASCADE ON UPDATE CASCADE
);