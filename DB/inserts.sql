INSERT INTO Usuarios (Nombre, Correo, PasswordHash, Rol, Activo)
VALUES
('Administrador', 'admin@autotaller.com', '$2a$11$hash1', 'Admin', TRUE),
('Carlos Méndez', 'carlos.mendez@autotaller.com', '$2a$11$hash2', 'Mecánico', TRUE),
('Laura Torres', 'laura.torres@autotaller.com', '$2a$11$hash3', 'Recepcionista', TRUE),
('Andrés López', 'andres.lopez@autotaller.com', '$2a$11$hash4', 'Mecánico', TRUE);


INSERT INTO Clientes (Nombre, Telefono, Correo, Direccion)
VALUES
('Juan Pérez', '3104567890', 'juanperez@gmail.com', 'Cra 15 #23-45'),
('María Gómez', '3209876543', 'mariagomez@yahoo.com', 'Calle 10 #45-67'),
('Pedro Ramírez', '3001234567', 'pedro.ramirez@hotmail.com', 'Av. 30 #20-10'),
('Sandra Díaz', '3116543210', 'sandradiaz@gmail.com', 'Transv. 40 #12-50');


INSERT INTO Repuestos (Codigo, Descripcion, Categoria, CantidadStock, StockMinimo, PrecioUnitario, Proveedor)
VALUES
('R001', 'Filtro de aceite', 'Motor', 50, 10, 25.00, 'AutoPartes S.A.'),
('R002', 'Bujía NGK', 'Encendido', 100, 20, 15.50, 'MotoresCol'),
('R003', 'Pastillas de freno', 'Frenos', 40, 10, 80.00, 'BrakeMaster Ltda'),
('R004', 'Aceite 10W-40 1L', 'Lubricantes', 200, 30, 35.00, 'LubriCar SAS'),
('R005', 'Filtro de aire', 'Motor', 60, 15, 22.00, 'Repuestodo');


INSERT INTO Repuestos (Codigo, Descripcion, Categoria, CantidadStock, StockMinimo, PrecioUnitario, Proveedor)
VALUES
('R001', 'Filtro de aceite', 'Motor', 50, 10, 25.00, 'AutoPartes S.A.'),
('R002', 'Bujía NGK', 'Encendido', 100, 20, 15.50, 'MotoresCol'),
('R003', 'Pastillas de freno', 'Frenos', 40, 10, 80.00, 'BrakeMaster Ltda'),
('R004', 'Aceite 10W-40 1L', 'Lubricantes', 200, 30, 35.00, 'LubriCar SAS'),
('R005', 'Filtro de aire', 'Motor', 60, 15, 22.00, 'Repuestodo');


INSERT INTO OrdenesServicio (NumeroOrden, VehiculoId, MecanicoId, RecepcionistaId, TipoServicio, Descripcion, Estado, KilometrajeIngreso, CostoManoObra)
VALUES
('ORD-2025-0001', 1, 2, 3, 'Mantenimiento Preventivo', 'Cambio de aceite y filtros', 'Completada', 55000, 80.00),
('ORD-2025-0002', 2, 4, 3, 'Reparación', 'Cambio de pastillas de freno', 'Pendiente', 32000, 120.00),
('ORD-2025-0003', 3, 2, 3, 'Diagnóstico', 'Chequeo de encendido irregular', 'En Proceso', 67000, 60.00),
('ORD-2025-0004', 4, 4, 3, 'Revisión General', 'Inspección completa de suspensión y frenos', 'Pendiente', 41000, 150.00);


INSERT INTO DetalleOrdenes (OrdenServicioId, RepuestoId, Cantidad, PrecioUnitario)
VALUES
(1, 1, 1, 25.00),
(1, 4, 4, 35.00),
(2, 3, 1, 80.00),
(3, 2, 4, 15.50),
(4, 5, 1, 22.00);


INSERT INTO Facturas (NumeroFactura, OrdenServicioId, ClienteId, SubtotalRepuestos, SubtotalManoObra, IVA, Descuento, MontoTotal, FormaPago, Estado)
VALUES
('FAC-2025-000001', 1, 1, 165.00, 80.00, 46.50, 0.00, 291.50, 'Efectivo', 'Pagada'),
('FAC-2025-000002', 2, 2, 80.00, 120.00, 32.00, 10.00, 222.00, 'Tarjeta', 'Pendiente'),
('FAC-2025-000003', 3, 3, 62.00, 60.00, 24.40, 0.00, 146.40, 'Transferencia', 'Pagada');


INSERT INTO Auditorias (UsuarioId, Entidad, EntidadId, TipoAccion, ValoresNuevos, DireccionIP)
VALUES
(3, 'OrdenServicio', 1, 'Creación', '{"NumeroOrden":"ORD-2025-0001","Estado":"Completada"}', '192.168.1.10'),
(3, 'OrdenServicio', 2, 'Creación', '{"NumeroOrden":"ORD-2025-0002","Estado":"Pendiente"}', '192.168.1.11'),
(3, 'Factura', 1, 'Creación', '{"NumeroFactura":"FAC-2025-000001"}', '192.168.1.12');


INSERT INTO RateLimitConfig (Endpoint, LimitePorMinuto, LimitePorHora, LimitePorDia)
VALUES
('/api/ordenesservicio', 60, 1000, 10000),
('/api/repuestos', 50, 800, 8000),
('/api/clientes', 100, 2000, 20000),
('/api/vehiculos', 100, 2000, 20000),
('/api/facturas', 40, 1000, 10000);
