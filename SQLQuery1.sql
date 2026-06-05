USE GestionInventarioWeb;
GO

-- Crea la tabla solo si no existe en la base de datos de su compu
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Usuarios' and xtype='U')
BEGIN
    CREATE TABLE Usuarios (
        Id INT PRIMARY KEY IDENTITY(1,1),
        NombreUsuario NVARCHAR(100) NOT NULL,
        Contrasena NVARCHAR(100) NOT NULL,
        Rol NVARCHAR(50) DEFAULT 'Admin'
    );

    INSERT INTO Usuarios (NombreUsuario, Contrasena, Rol) 
    VALUES ('admin', 'admin123', 'Admin');
END
GO