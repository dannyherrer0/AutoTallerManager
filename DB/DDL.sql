
CREATE DATABASE IF NOT EXISTS AutoTallerDB
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;

USE AutoTallerDB;


CREATE TABLE Usuarios (
    UsuarioId INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Correo VARCHAR(100) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    Rol ENUM('Admin', 'Mecánico', 'Recepcionista') NOT NULL,
    Activo BOOLEAN DEFAULT TRUE,
    FechaCreacion DATETIME DEFAULT CURRENT_TIMESTAMP,
    FechaActualizacion DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_correo (Correo),
    INDEX idx_rol (Rol)
) ENGINE=InnoDB;

CREATE TABLE Clientes (
    ClienteId INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL,
    Telefono VARCHAR(20),
    Correo VARCHAR(100),
    Direccion VARCHAR(200),
    FechaRegistro DATETIME DEFAULT CURRENT_TIMESTAMP,
    FechaActualizacion DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    Activo BOOLEAN DEFAULT TRUE,
    INDEX idx_nombre (Nombre),
    INDEX idx_correo (Correo),
    INDEX idx_telefono (Telefono)
) ENGINE=InnoDB;


CREATE TABLE Vehiculos (
    VehiculoId INT AUTO_INCREMENT PRIMARY KEY,
    ClienteId INT NOT NULL,
    Marca VARCHAR(50) NOT NULL,
    Modelo VARCHAR(50) NOT NULL,
    Anio INT NOT NULL,
    VIN VARCHAR(17) NOT NULL UNIQUE,
    NumeroPlaca VARCHAR(20),
    Kilometraje INT DEFAULT 0,
    Color VARCHAR(30),
    FechaRegistro DATETIME DEFAULT CURRENT_TIMESTAMP,
    FechaActualizacion DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    Activo BOOLEAN DEFAULT TRUE,
    FOREIGN KEY (ClienteId) REFERENCES Clientes(ClienteId) ON DELETE RESTRICT,
    INDEX idx_cliente (ClienteId),
    INDEX idx_vin (VIN),
    INDEX idx_placa (NumeroPlaca),
    INDEX idx_marca_modelo (Marca, Modelo)
) ENGINE=InnoDB;

CREATE TABLE Repuestos (
    RepuestoId INT AUTO_INCREMENT PRIMARY KEY,
    Codigo VARCHAR(50) NOT NULL UNIQUE,
    Descripcion VARCHAR(200) NOT NULL,
    Categoria VARCHAR(50),
    CantidadStock INT DEFAULT 0,
    StockMinimo INT DEFAULT 5,
    PrecioUnitario DECIMAL(10, 2) NOT NULL,
    Proveedor VARCHAR(100),
    FechaCreacion DATETIME DEFAULT CURRENT_TIMESTAMP,
    FechaActualizacion DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    Activo BOOLEAN DEFAULT TRUE,
    INDEX idx_codigo (Codigo),
    INDEX idx_categoria (Categoria),
    INDEX idx_stock_bajo (CantidadStock, StockMinimo),
    CHECK (PrecioUnitario >= 0),
    CHECK (CantidadStock >= 0)
) ENGINE=InnoDB;

CREATE TABLE OrdenesServicio (
    OrdenServicioId INT AUTO_INCREMENT PRIMARY KEY,
    NumeroOrden VARCHAR(20) NOT NULL UNIQUE,
    VehiculoId INT NOT NULL,
    MecanicoId INT,
    RecepcionistaId INT,
    TipoServicio ENUM('Mantenimiento Preventivo', 'Reparación', 'Diagnóstico', 'Revisión General', 'Otro') NOT NULL,
    Descripcion TEXT,
    Estado ENUM('Pendiente', 'En Proceso', 'Completada', 'Cancelada', 'Facturada') DEFAULT 'Pendiente',
    FechaIngreso DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FechaEstimadaEntrega DATETIME,
    FechaEntregaReal DATETIME,
    KilometrajeIngreso INT,
    CostoManoObra DECIMAL(10, 2) DEFAULT 0.00,
    ObservacionesMecanico TEXT,
    FechaCreacion DATETIME DEFAULT CURRENT_TIMESTAMP,
    FechaActualizacion DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (VehiculoId) REFERENCES Vehiculos(VehiculoId) ON DELETE RESTRICT,
    FOREIGN KEY (MecanicoId) REFERENCES Usuarios(UsuarioId) ON DELETE SET NULL,
    FOREIGN KEY (RecepcionistaId) REFERENCES Usuarios(UsuarioId) ON DELETE SET NULL,
    INDEX idx_numero_orden (NumeroOrden),
    INDEX idx_vehiculo (VehiculoId),
    INDEX idx_mecanico (MecanicoId),
    INDEX idx_estado (Estado),
    INDEX idx_fecha_ingreso (FechaIngreso),
    INDEX idx_tipo_servicio (TipoServicio),
    CHECK (CostoManoObra >= 0)
) ENGINE=InnoDB;

-- ============================================
-- TABLA: DetalleOrdenes
-- Relación entre órdenes y repuestos utilizados
-- ============================================
CREATE TABLE DetalleOrdenes (
    DetalleOrdenId INT AUTO_INCREMENT PRIMARY KEY,
    OrdenServicioId INT NOT NULL,
    RepuestoId INT NOT NULL,
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(10, 2) NOT NULL,
    Subtotal DECIMAL(10, 2) GENERATED ALWAYS AS (Cantidad * PrecioUnitario) STORED,
    FechaCreacion DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (OrdenServicioId) REFERENCES OrdenesServicio(OrdenServicioId) ON DELETE CASCADE,
    FOREIGN KEY (RepuestoId) REFERENCES Repuestos(RepuestoId) ON DELETE RESTRICT,
    INDEX idx_orden (OrdenServicioId),
    INDEX idx_repuesto (RepuestoId),
    CHECK (Cantidad > 0),
    CHECK (PrecioUnitario >= 0)
) ENGINE=InnoDB;

-- ============================================
-- TABLA: Facturas
-- Documentos de cobro generados
-- ============================================
CREATE TABLE Facturas (
    FacturaId INT AUTO_INCREMENT PRIMARY KEY,
    NumeroFactura VARCHAR(20) NOT NULL UNIQUE,
    OrdenServicioId INT NOT NULL,
    ClienteId INT NOT NULL,
    FechaEmision DATETIME DEFAULT CURRENT_TIMESTAMP,
    SubtotalRepuestos DECIMAL(10, 2) DEFAULT 0.00,
    SubtotalManoObra DECIMAL(10, 2) DEFAULT 0.00,
    IVA DECIMAL(10, 2) DEFAULT 0.00,
    Descuento DECIMAL(10, 2) DEFAULT 0.00,
    MontoTotal DECIMAL(10, 2) NOT NULL,
    FormaPago ENUM('Efectivo', 'Tarjeta', 'Transferencia', 'Cheque', 'Otro'),
    Estado ENUM('Pagada', 'Pendiente', 'Anulada') DEFAULT 'Pendiente',
    Observaciones TEXT,
    FechaCreacion DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (OrdenServicioId) REFERENCES OrdenesServicio(OrdenServicioId) ON DELETE RESTRICT,
    FOREIGN KEY (ClienteId) REFERENCES Clientes(ClienteId) ON DELETE RESTRICT,
    INDEX idx_numero_factura (NumeroFactura),
    INDEX idx_orden (OrdenServicioId),
    INDEX idx_cliente (ClienteId),
    INDEX idx_fecha_emision (FechaEmision),
    INDEX idx_estado (Estado),
    CHECK (MontoTotal >= 0),
    CHECK (SubtotalRepuestos >= 0),
    CHECK (SubtotalManoObra >= 0),
    CHECK (IVA >= 0),
    CHECK (Descuento >= 0)
) ENGINE=InnoDB;

-- ============================================
-- TABLA: Auditorias
-- Registro de todas las operaciones críticas
-- ============================================
CREATE TABLE Auditorias (
    AuditoriaId INT AUTO_INCREMENT PRIMARY KEY,
    UsuarioId INT,
    Entidad VARCHAR(50) NOT NULL,
    EntidadId INT NOT NULL,
    TipoAccion ENUM('Creación', 'Modificación', 'Eliminación', 'Consulta') NOT NULL,
    ValoresAnteriores JSON,
    ValoresNuevos JSON,
    DireccionIP VARCHAR(45),
    FechaHora DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(UsuarioId) ON DELETE SET NULL,
    INDEX idx_usuario (UsuarioId),
    INDEX idx_entidad (Entidad, EntidadId),
    INDEX idx_tipo_accion (TipoAccion),
    INDEX idx_fecha (FechaHora)
) ENGINE=InnoDB;

-- ============================================
-- TABLA: RateLimitConfig (Opcional)
-- Configuración de límites de solicitudes
-- ============================================
CREATE TABLE RateLimitConfig (
    ConfigId INT AUTO_INCREMENT PRIMARY KEY,
    Endpoint VARCHAR(100) NOT NULL,
    LimitePorMinuto INT NOT NULL DEFAULT 60,
    LimitePorHora INT NOT NULL DEFAULT 1000,
    LimitePorDia INT NOT NULL DEFAULT 10000,
    Activo BOOLEAN DEFAULT TRUE,
    FechaCreacion DATETIME DEFAULT CURRENT_TIMESTAMP,
    FechaActualizacion DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_endpoint (Endpoint)
) ENGINE=InnoDB;

-- ============================================
-- VISTAS ÚTILES
-- ============================================

-- Vista: Órdenes con información completa
CREATE VIEW vw_OrdenesCompletas AS
SELECT 
    o.OrdenServicioId,
    o.NumeroOrden,
    o.TipoServicio,
    o.Estado,
    o.FechaIngreso,
    o.FechaEstimadaEntrega,
    c.ClienteId,
    c.Nombre AS NombreCliente,
    c.Telefono AS TelefonoCliente,
    v.VehiculoId,
    v.Marca,
    v.Modelo,
    v.Anio,
    v.VIN,
    v.NumeroPlaca,
    m.Nombre AS NombreMecanico,
    r.Nombre AS NombreRecepcionista,
    o.CostoManoObra,
    COALESCE(SUM(d.Subtotal), 0) AS TotalRepuestos,
    (o.CostoManoObra + COALESCE(SUM(d.Subtotal), 0)) AS MontoTotal
FROM OrdenesServicio o
INNER JOIN Vehiculos v ON o.VehiculoId = v.VehiculoId
INNER JOIN Clientes c ON v.ClienteId = c.ClienteId
LEFT JOIN Usuarios m ON o.MecanicoId = m.UsuarioId
LEFT JOIN Usuarios r ON o.RecepcionistaId = r.UsuarioId
LEFT JOIN DetalleOrdenes d ON o.OrdenServicioId = d.OrdenServicioId
GROUP BY o.OrdenServicioId;

-- Vista: Repuestos con stock bajo
CREATE VIEW vw_RepuestosStockBajo AS
SELECT 
    RepuestoId,
    Codigo,
    Descripcion,
    Categoria,
    CantidadStock,
    StockMinimo,
    (StockMinimo - CantidadStock) AS Faltante,
    PrecioUnitario,
    Proveedor
FROM Repuestos
WHERE CantidadStock <= StockMinimo AND Activo = TRUE;

-- Vista: Facturas con detalles
CREATE VIEW vw_FacturasDetalladas AS
SELECT 
    f.FacturaId,
    f.NumeroFactura,
    f.FechaEmision,
    c.Nombre AS NombreCliente,
    c.Telefono AS TelefonoCliente,
    o.NumeroOrden,
    v.Marca,
    v.Modelo,
    v.VIN,
    f.SubtotalRepuestos,
    f.SubtotalManoObra,
    f.IVA,
    f.Descuento,
    f.MontoTotal,
    f.FormaPago,
    f.Estado
FROM Facturas f
INNER JOIN Clientes c ON f.ClienteId = c.ClienteId
INNER JOIN OrdenesServicio o ON f.OrdenServicioId = o.OrdenServicioId
INNER JOIN Vehiculos v ON o.VehiculoId = v.VehiculoId;

-- ============================================
-- PROCEDIMIENTOS ALMACENADOS
-- ============================================

DELIMITER //

-- Procedimiento: Registrar Cliente con Vehículo
CREATE PROCEDURE sp_RegistrarClienteConVehiculo(
    IN p_NombreCliente VARCHAR(100),
    IN p_Telefono VARCHAR(20),
    IN p_Correo VARCHAR(100),
    IN p_Direccion VARCHAR(200),
    IN p_Marca VARCHAR(50),
    IN p_Modelo VARCHAR(50),
    IN p_Anio INT,
    IN p_VIN VARCHAR(17),
    IN p_NumeroPlaca VARCHAR(20),
    IN p_Kilometraje INT,
    IN p_Color VARCHAR(30),
    OUT p_ClienteId INT,
    OUT p_VehiculoId INT
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;
    
    START TRANSACTION;
    
    INSERT INTO Clientes (Nombre, Telefono, Correo, Direccion)
    VALUES (p_NombreCliente, p_Telefono, p_Correo, p_Direccion);
    
    SET p_ClienteId = LAST_INSERT_ID();
    
    INSERT INTO Vehiculos (ClienteId, Marca, Modelo, Anio, VIN, NumeroPlaca, Kilometraje, Color)
    VALUES (p_ClienteId, p_Marca, p_Modelo, p_Anio, p_VIN, p_NumeroPlaca, p_Kilometraje, p_Color);
    
    SET p_VehiculoId = LAST_INSERT_ID();
    
    COMMIT;
END //

-- Procedimiento: Actualizar Stock de Repuesto
CREATE PROCEDURE sp_ActualizarStockRepuesto(
    IN p_RepuestoId INT,
    IN p_CantidadCambio INT,
    IN p_TipoOperacion ENUM('Entrada', 'Salida')
)
BEGIN
    DECLARE v_StockActual INT;
    
    SELECT CantidadStock INTO v_StockActual
    FROM Repuestos
    WHERE RepuestoId = p_RepuestoId;
    
    IF p_TipoOperacion = 'Entrada' THEN
        UPDATE Repuestos
        SET CantidadStock = CantidadStock + p_CantidadCambio
        WHERE RepuestoId = p_RepuestoId;
    ELSE
        IF v_StockActual >= p_CantidadCambio THEN
            UPDATE Repuestos
            SET CantidadStock = CantidadStock - p_CantidadCambio
            WHERE RepuestoId = p_RepuestoId;
        ELSE
            SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'Stock insuficiente para realizar la operación';
        END IF;
    END IF;
END //

-- Procedimiento: Generar Factura desde Orden
CREATE PROCEDURE sp_GenerarFactura(
    IN p_OrdenServicioId INT,
    IN p_FormaPago VARCHAR(20),
    IN p_IVAPorc DECIMAL(5,2),
    IN p_Descuento DECIMAL(10,2),
    OUT p_FacturaId INT,
    OUT p_NumeroFactura VARCHAR(20)
)
BEGIN
    DECLARE v_ClienteId INT;
    DECLARE v_SubtotalRepuestos DECIMAL(10,2);
    DECLARE v_SubtotalManoObra DECIMAL(10,2);
    DECLARE v_IVA DECIMAL(10,2);
    DECLARE v_MontoTotal DECIMAL(10,2);
    DECLARE v_NumFactura VARCHAR(20);
    
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        RESIGNAL;
    END;
    
    START TRANSACTION;
    
    -- Obtener datos de la orden
    SELECT v.ClienteId, o.CostoManoObra, COALESCE(SUM(d.Subtotal), 0)
    INTO v_ClienteId, v_SubtotalManoObra, v_SubtotalRepuestos
    FROM OrdenesServicio o
    INNER JOIN Vehiculos v ON o.VehiculoId = v.VehiculoId
    LEFT JOIN DetalleOrdenes d ON o.OrdenServicioId = d.OrdenServicioId
    WHERE o.OrdenServicioId = p_OrdenServicioId
    GROUP BY o.OrdenServicioId;
    
    -- Calcular montos
    SET v_IVA = (v_SubtotalRepuestos + v_SubtotalManoObra) * (p_IVAPorc / 100);
    SET v_MontoTotal = v_SubtotalRepuestos + v_SubtotalManoObra + v_IVA - p_Descuento;
    
    -- Generar número de factura
    SET v_NumFactura = CONCAT('FAC-', YEAR(NOW()), '-', LPAD(
        (SELECT COUNT(*) + 1 FROM Facturas WHERE YEAR(FechaEmision) = YEAR(NOW())), 
        6, '0'
    ));
    
    -- Insertar factura
    INSERT INTO Facturas (
        NumeroFactura, OrdenServicioId, ClienteId, SubtotalRepuestos,
        SubtotalManoObra, IVA, Descuento, MontoTotal, FormaPago
    ) VALUES (
        v_NumFactura, p_OrdenServicioId, v_ClienteId, v_SubtotalRepuestos,
        v_SubtotalManoObra, v_IVA, p_Descuento, v_MontoTotal, p_FormaPago
    );
    
    SET p_FacturaId = LAST_INSERT_ID();
    SET p_NumeroFactura = v_NumFactura;
    
    -- Actualizar estado de la orden
    UPDATE OrdenesServicio
    SET Estado = 'Facturada'
    WHERE OrdenServicioId = p_OrdenServicioId;
    
    COMMIT;
END //

DELIMITER ;




DELIMITER //
CREATE TRIGGER trg_DetalleOrden_AfterInsert
AFTER INSERT ON DetalleOrdenes
FOR EACH ROW
BEGIN
    UPDATE Repuestos
    SET CantidadStock = CantidadStock - NEW.Cantidad
    WHERE RepuestoId = NEW.RepuestoId;
END //

-- Trigger: Restaurar stock al eliminar detalle de orden
CREATE TRIGGER trg_DetalleOrden_AfterDelete
AFTER DELETE ON DetalleOrdenes
FOR EACH ROW
BEGIN
    UPDATE Repuestos
    SET CantidadStock = CantidadStock + OLD.Cantidad
    WHERE RepuestoId = OLD.RepuestoId;
END //

-- Trigger: Auditoría al crear orden
CREATE TRIGGER trg_OrdenServicio_AfterInsert
AFTER INSERT ON OrdenesServicio
FOR EACH ROW
BEGIN
    INSERT INTO Auditorias (UsuarioId, Entidad, EntidadId, TipoAccion, ValoresNuevos)
    VALUES (
        NEW.RecepcionistaId,
        'OrdenServicio',
        NEW.OrdenServicioId,
        'Creación',
        JSON_OBJECT(
            'NumeroOrden', NEW.NumeroOrden,
            'VehiculoId', NEW.VehiculoId,
            'TipoServicio', NEW.TipoServicio,
            'Estado', NEW.Estado
        )
    );
END //

DELIMITER ;

-- ============================================
-- DATOS INICIALES (SEED)
-- ============================================

-- Insertar usuarios iniciales
INSERT INTO Usuarios (Nombre, Correo, PasswordHash, Rol) VALUES
('Administrador', 'admin@autotaller.com', '$2a$11$hashedpassword1', 'Admin'),
('Juan Pérez', 'juan.mecanico@autotaller.com', '$2a$11$hashedpassword2', 'Mecánico'),
('María García', 'maria.recepcion@autotaller.com', '$2a$11$hashedpassword3', 'Recepcionista');

INSERT INTO RateLimitConfig (Endpoint, LimitePorMinuto, LimitePorHora, LimitePorDia) VALUES
('/Api/ordenesservicio', 60, 1000, 10000),
('/Api/repuestos', 30, 500, 5000),
('/Api/clientes', 100, 2000, 20000),
('/Api/vehiculos', 100, 2000, 20000),
('/Api/facturas', 50, 1000, 10000);

-- ============================================
-- ÍNDICES ADICIONALES PARA OPTIMIZACIÓN
-- ============================================

-- Índices compuestos para consultas comunes
CREATE INDEX idx_ordenes_vehiculo_estado ON OrdenesServicio(VehiculoId, Estado);
CREATE INDEX idx_ordenes_mecanico_fecha ON OrdenesServicio(MecanicoId, FechaIngreso);
CREATE INDEX idx_facturas_cliente_fecha ON Facturas(ClienteId, FechaEmision);
CREATE INDEX idx_vehiculos_cliente_activo ON Vehiculos(ClienteId, Activo);

