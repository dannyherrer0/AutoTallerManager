# Examen Cierre Orden y generación de factura AutoTallerManager

Agregué CerrarOrdenServicio y GenerarFactura para calcular la mano de obra y repuestos 

# Alcance

CerrarOrdenServicio 
Cambia el estado de la orden a “completada”.
Consume definitivamente las reservas de repuestos asociadas (descuenta stock).
Registra tiempos finales (p. ej., fechaCierre) y el usuario que ejecuta la acción.
GenerarFactura
Endpoint: POST /api/facturas
Crea una Factura enlazada a la orden, con desglose de ítems (repuestos) y mano de obra.
Calcula subtotal, impuestos (si aplica) y total.
La orden debe estar completada para facturar (evitar facturar órdenes abiertas).
Persistir el enlace factura ↔ orden (p. ej., ordenId en factura).
Seguridad y roles (JWT)
Mecánico: puede cerrar orden.
Admin: puede consultar todo (y facturar si así se define).
Recepcionista: no puede facturar; puede consultar lo que su rol permita.
Responder con 401 (no autenticado) y 403 (autenticado sin permiso) cuando corresponda.


## Reglas de negocio clave
Consumo de reservas: al cerrar la orden, toda reserva pendiente debe convertirse en consumo (descuento final de stock); no deben quedar reservas activas para esa orden.
Idempotencia: evitar cierres duplicados o facturas duplicadas para la misma orden (si se reintenta, devolver estado actual o 409 Conflict con mensaje claro).
Precondiciones para facturar:
Orden en estado “completada”.
Totales calculados a partir de ítems consumidos + mano de obra.
Validaciones mínimas: existencia de la orden, estado válido para cerrar, que exista al menos un ítem o mano de obra (si la política lo exige), y que el usuario tenga el rol adecuado.