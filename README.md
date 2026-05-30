para ejecutar la base de datos debes:
1- buscar explorador de objetos sql server
2- en localDb MSSSQLLOCALDB debes dar click derecho (nueva consulta)
3- copiar y pegar esto: 
CREATE DATABASE [GestionInventarioWeb];
GO
4- con esto estara creada la db ahora deben crear las tablas con estos comandos:
PRIMERO TIENENE Q DAR CLICK DERECHO A LA BASE DE DATOS CREADA Y DARLE A "nueva consulta"

CREATE TABLE Proveedores (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Telefono NVARCHAR(20),
    Rut NVARCHAR(20),
    Correo NVARCHAR(100),
    Empresa NVARCHAR(100),
    Direccion NVARCHAR(200),
    estado BIT DEFAULT 1
);

CREATE TABLE Productos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    PrecioVenta DECIMAL(18, 2),
    estado BIT DEFAULT 1
);

CREATE TABLE Ventas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreCliente NVARCHAR(100),
    Fecha DATETIME DEFAULT GETDATE(),
    IVA DECIMAL(18, 2),
    Total DECIMAL(18, 2)
);

CREATE TABLE Usuario (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL,
    Contrasena NVARCHAR(100) NOT NULL
);

-- 2. Crear tablas dependientes (con llaves foráneas)
CREATE TABLE Insumos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    StockActual INT,
    UnidadMedida NVARCHAR(20),
    PrecioUnitario DECIMAL(18, 2),
    FechaCaducidad DATE,
    Id_Proveedor INT,
    Estado BIT DEFAULT 1,
    CONSTRAINT FK_Insumos_Proveedores FOREIGN KEY (Id_Proveedor) REFERENCES Proveedores(Id)
);

CREATE TABLE Recetas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Id_Producto INT,
    Id_Insumo INT,
    CantidadRequerida DECIMAL(18, 2),
    CONSTRAINT FK_Recetas_Productos FOREIGN KEY (Id_Producto) REFERENCES Productos(Id),
    CONSTRAINT FK_Recetas_Insumos FOREIGN KEY (Id_Insumo) REFERENCES Insumos(Id)
);

CREATE TABLE DetalleVenta (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    IdVenta INT,
    IdProducto INT,
    Producto NVARCHAR(100),
    Precio DECIMAL(18, 2),
    Cantidad INT,
    Subtotal DECIMAL(18, 2),
    CONSTRAINT FK_DetalleVenta_Ventas FOREIGN KEY (IdVenta) REFERENCES Ventas(Id),
    CONSTRAINT FK_DetalleVenta_Productos FOREIGN KEY (IdProducto) REFERENCES Productos(Id)
);
