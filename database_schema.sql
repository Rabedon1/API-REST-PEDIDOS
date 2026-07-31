--  Tablas para API REST Pedidos

-- 1. Tabla de Cabecera de Pedidos
CREATE TABLE PedidoCabecera (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ClienteId INT NOT NULL,
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    Total DECIMAL(18, 2) NOT NULL,
    Usuario NVARCHAR(100) NOT NULL,
    Estado NVARCHAR(50) NOT NULL DEFAULT 'Registrado' 
);
GO

-- 2. Tabla de Detalle de Pedidos
CREATE TABLE PedidoDetalle (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PedidoCabeceraId INT NOT NULL,
    ProductoId INT NOT NULL,
    Cantidad INT NOT NULL,
    Precio DECIMAL(18, 2) NOT NULL,
    Subtotal DECIMAL(18, 2) NOT NULL,
    CONSTRAINT FK_PedidoDetalle_PedidoCabecera FOREIGN KEY (PedidoCabeceraId) REFERENCES PedidoCabecera(Id) ON DELETE CASCADE
);
GO

-- 3. Tabla de Auditoría y Logs
CREATE TABLE LogAuditoria (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Fecha DATETIME NOT NULL DEFAULT GETDATE(),
    Evento NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(MAX) NOT NULL,
    Nivel NVARCHAR(20) NOT NULL DEFAULT 'Info', -- Info, Warning, Error
    Usuario NVARCHAR(100) NULL
);
GO
