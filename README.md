# SportsLeague API

API desarrollada en .NET para la gestión de una liga deportiva. El proyecto está organizado por capas, separando la API, el acceso a datos y las entidades del dominio.

## Entrega 3

En esta entrega se trabajó la gestión de torneos y su relación con equipos mediante una tabla intermedia. Se implementaron entidades, repositorios, servicios, DTOs y endpoints para manejar la inscripción de equipos en torneos.

## Entrega 4

En esta entrega se agregó la gestión de patrocinadores.

Cambios principales:

- Se creó la entidad `Sponsor`.
- Se creó el enum `SponsorCategory`.
- Se creó la relación muchos a muchos entre `Sponsor` y `Tournament` mediante `TournamentSponsor`.
- Se agregaron repositorios para `Sponsor` y `TournamentSponsor`.
- Se implementó el servicio con validaciones de negocio.
- Se agregaron DTOs de entrada y salida.
- Se configuraron los mapeos con AutoMapper.
- Se creó el controlador `SponsorController`.
- Se agregaron endpoints para crear, consultar, actualizar y eliminar sponsors.
- Se agregaron endpoints para vincular y desvincular sponsors de torneos.
- Se creó y aplicó la migración `AddSponsor_TournamentSponsor`.

## Validaciones implementadas

- No se permite crear sponsors con nombre duplicado.
- El correo de contacto debe tener formato válido.
- No se permite vincular un sponsor a un torneo inexistente.
- No se permite vincular un sponsor inexistente.
- No se permite repetir la misma relación entre sponsor y torneo.
- El monto del contrato debe ser mayor que cero.

## Endpoints principales de Sponsor

GET /api/Sponsor  
GET /api/Sponsor/{id}  
POST /api/Sponsor  
PUT /api/Sponsor/{id}  
DELETE /api/Sponsor/{id}  
GET /api/Sponsor/{id}/tournaments  
POST /api/Sponsor/{id}/tournaments  
DELETE /api/Sponsor/{id}/tournaments/{tid}

## Pruebas realizadas

Se probó la API desde Swagger. La creación de sponsors respondió correctamente con `201 Created`.

También se validaron errores como nombre duplicado, correo inválido y vínculo repetido, usando respuestas `409 Conflict`.

Además, se comprobó la relación entre Nike y el torneo Liga 2026-I, verificando que el sponsor pudiera vincularse correctamente y luego consultarse desde el endpoint correspondiente.