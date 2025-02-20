CREATE TABLE [dbo].[LichTrinh] (
    [ID_ChuyenDi]   NVARCHAR(50) NOT NULL,
    [NgayBatDau]    DATE NOT NULL,
    [ID_HDV]        NVARCHAR(5) NOT NULL,
    PRIMARY KEY ([ID_ChuyenDi], [NgayBatDau], [ID_HDV]),
    FOREIGN KEY ([ID_ChuyenDi]) REFERENCES [dbo].[ChuyenDi]([ID_ChuyenDi])
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY ([ID_HDV]) REFERENCES [dbo].[HuongDanVien]([ID_HDV])
        ON DELETE CASCADE ON UPDATE CASCADE
);