SportsLeague API

API en .NET para gestionar una liga deportiva. El proyecto está organizado por capas: API, Domain y DataAccess.

El sistema incluye gestión de equipos, jugadores, árbitros, torneos, patrocinadores, partidos y alineaciones de partido.

En la parte de sponsors se implementó la entidad Sponsor, sus categorías, la relación con torneos mediante TournamentSponsor y las validaciones para evitar nombres duplicados, correos inválidos, contratos menores o iguales a cero y vínculos repetidos.

También se agregó la entidad Match para crear partidos entre dos equipos, asignar torneo, árbitro, fecha y manejar estados como Scheduled, InProgress, Finished y Cancelled.

Finalmente, se implementó MatchLineup para registrar jugadores convocados a un partido, indicando si son titulares o suplentes y la posición asignada. Esta funcionalidad valida que el partido exista, que el jugador exista, que pertenezca a uno de los equipos del partido, que no esté repetido en la alineación, que no haya más de 11 titulares por equipo y que el partido esté en estado Scheduled.

Endpoints principales:

GET /api/Sponsor
POST /api/Sponsor
GET /api/Sponsor/{id}
PUT /api/Sponsor/{id}
DELETE /api/Sponsor/{id}
GET /api/Sponsor/{id}/tournaments
POST /api/Sponsor/{id}/tournaments
DELETE /api/Sponsor/{id}/tournaments/{tid}
GET /api/Match
POST /api/Match
PATCH /api/Match/{id}/status/{status}
POST /api/match/{matchId}/lineup
GET /api/match/{matchId}/lineup
GET /api/match/{matchId}/lineup/team/{teamId}
DELETE /api/match/{matchId}/lineup/{id}

El proyecto compila correctamente y fue probado desde Swagger.

Para ejecutar:

dotnet restore
dotnet build
dotnet run --project SportsLeague.API

Swagger:

http://localhost:5105/swagger