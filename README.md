(1H) Agendamiento de órdenes con validaciones de disponibilidad e inventario

## Descripción

Se requiere implementar el **caso de uso “Crear Orden de Servicio”** en el backend existente (.NET/ASP.NET Core + EF Core), garantizando las siguientes **reglas de negocio** y estándares de la solución:

1. **Un vehículo no puede tener dos órdenes activas simultáneas.**
2.  Antes de crear, verificar si el vehículo (por `VehiculoId` o `VIN`) tiene una orden con estado “Abierta/Activa/EnProceso/Agendada”. En caso afirmativo, **rechazar** la solicitud (`409`).
3. **Validar disponibilidad del mecánico** en la **fecha y franja de ingreso**.
4.  El sistema debe **rechazar** la creación si el mecánico presenta **solapamiento** con otra orden activa o si la franja no cumple las políticas operativas del taller. 
5. **Reservar repuestos** si hay stock suficiente.
6.  Si el payload incluye repuestos `{ RepuestoId, Cantidad }`, validar existencia/stock y **reservar** las cantidades (o descontar de columna “Reservado”, según el modelo). Si algún ítem no alcanza, **rechazar** con `409` y **desglose** por ítem.
7. **Calcular fecha estimada** según el **tipo de servicio**.
8.  Derivar una fecha estimada (p. ej.: “Mantenimiento menor = +1 día hábil; Mantenimiento mayor = +3 días hábiles; Diagnóstico = +0–1 día”) y **persistir** el valor.
9. **Exponer** el endpoint `POST /api/ordenesservicio`.
10.  Usar **DTOs**, **AutoMapper**, **Unit of Work** y **Repository** conforme a la arquitectura del proyecto. Documentar en Swagger y agregar ejemplos mínimos en `.http`.

### Casos típicos/condiciones que **deben** cubrirse (disponibilidad del mecánico)

**Traslape total o parcial (rechazar)**

 `Overlaps(aStart,aEnd,bStart,bEnd) ≡ aStart < bEnd && aEnd > bStart`

 Ej.: 09:00–11:00 vs 10:00–12:00 (parcial), 09:00–11:00 vs 09:00–11:00 (total), bordes 08:30–09:15 o 10:45–11:15.



**Back-to-back (permitir)**

 Fin anterior == inicio siguiente (p. ej., 09:00–10:00 y 10:00–11:00).



**Estados que bloquean**

 Solo cuentan `Abierta/Activa/EnProceso/Agendada`. `Cancelada/Completada` **no** bloquean.



**Jornada laboral (rechazar fuera de horario)**

 La franja propuesta debe caer íntegramente en el horario (ej. 08:00–18:00).



**Pausa/almuerzo bloqueante (si aplica, rechazar/interrumpir)**

 Si la política reserva 12:00–13:00, no se debe solapar con esa franja.



**Duración mínima por tipo de servicio (rechazar si no cabe)**

 Si “Mayor” exige ≥3h, la franja debe cubrir esa duración sin solapes.



**Sede/branch (rechazar cruce entre sedes)**

 El mismo mecánico no puede estar asignado en **dos sedes** en franjas que se solapan.



**Órdenes consecutivas válidas**

 Varias franjas encadenadas sin solape (back-to-back) son válidas.



**Validación por fecha completa (si el modelo no usa hora)**

 Si solo hay fecha (sin hora), segunda asignación **mismo día** → rechazar.



**Concurrencia básica (doble envío)**

 Debe evitarse el doble agendamiento con verificaciones dentro de la **misma transacción** antes de confirmar.



## Objetivo general



Crear de forma **consistente y transaccional** una Orden de Servicio que cumpla las reglas de negocio, proteja inventario y respete la disponibilidad operativa del taller.



## Objetivos específicos

- Implementar la **validación de unicidad** de orden activa por vehículo.
- Verificar la **disponibilidad del mecánico** para la fecha de ingreso.
- Gestionar la **reserva de repuestos** (éxito total o rechazo con detalle).
- **Calcular y persistir** la **fecha estimada** según el tipo de servicio.
- Exponer el **endpoint** con **DTOs + AutoMapper** y persistencia vía **UoW/Repository**.
- Devolver **códigos de estado** y **mensajes** claros (`201`, `400`, `404`, `409`, `422`).



## Alcance funcional (mínimo)

**Entrada (DTO de creación)** sugerida:

![img](https://khc-sistema-v2.s3.amazonaws.com/editor/17594434599802426ca8c7dc8d/attachment-1.png.png)



**Salida (201 Created)**: id de la orden, estado inicial, **fecha estimada**, resumen de repuestos reservados.

**Estados de error** estandarizados:

- `400 Bad Request`: DTO inválido / datos faltantes.
- `404 Not Found`: vehículo/mecánico/repuesto inexistente.
- `409 Conflict`: vehículo con orden activa / mecánico no disponible / stock insuficiente.
- `422 Unprocessable Entity`: reglas de negocio incumplidas (opcional si separas semánticamente de 409).



## Predicados bases a tener en Cuenta



bool Overlaps(DateTime aStart, DateTime aEnd, DateTime bStart, DateTime bEnd)

 => aStart < bEnd && aEnd > bStart; // solape estricto



bool BackToBack(DateTime aEnd, DateTime bStart)

 => aEnd == bStart;     // sin solape, pegadas



bool IsActiveStatus(string estado)

 => new[]{ "Abierta","Activa","EnProceso","Agendada" }.Contains(estado);



bool InWorkShift(DateTime start, DateTime end, TimeSpan dayStart, TimeSpan dayEnd)

 => start.TimeOfDay >= dayStart && end.TimeOfDay <= dayEnd;



bool IntersectsBreak(DateTime start, DateTime end, TimeSpan breakStart, TimeSpan breakEnd)

 => start.TimeOfDay < breakEnd && end.TimeOfDay > breakStart;



bool SameDay(DateTime a, DateTime b) => a.Date == b.Date;



Resultado esperado

## Resultados esperados

- `POST /api/ordenesservicio` crea una orden válida, con: vehículo, mecánico, fecha ingreso, **fecha estimada** y, opcionalmente, **detalle de repuestos reservados**.
- Si el vehículo ya tiene orden activa → **409 Conflict** con mensaje: “El vehículo X ya posee una orden activa”.
- Si el mecánico no está disponible → **409 Conflict** con mensaje: “Mecánico Y no disponible en la fecha Z”.
- Si un repuesto no alcanza → **409 Conflict** con desglose: `{ repuestoId, solicitado, disponible }`.
- Registro consistente en BD (transacción); sin órdenes “a medias”.
- **Swagger** actualizado y **tests .http** (al menos: caso exitoso + 1–2 casos de error).