# Pokémon Explorer

Aplicación Full Stack que implementa un explorador de Pokémon consumiendo PokeAPI mediante un backend intermedio desarrollado en ASP.NET Core con persistencia en SQL Server y un frontend en Angular.

## Descripción

Este proyecto demuestra la implementación de una arquitectura completa que incluye:

- Consumo de API REST externa con documentación oficial
- Backend intermedio con arquitectura por capas
- Persistencia de datos en base de datos relacional
- Frontend moderno con framework JavaScript
- Filtrado por tipo, búsqueda y paginación
- Manejo de estados de interfaz y errores

## Arquitectura General

```
┌─────────────────┐
│  Angular 18     │
│   (Frontend)    │
└────────┬────────┘
         │
         │ HTTP Request/Response
         ↓
┌─────────────────┐        ┌──────────────────┐
│  ASP.NET Core   │◄──────►│    PokeAPI       │
│  (Backend API)  │  HTTP  │    (Externa)     │
└────────┬────────┘        └──────────────────┘
         │
         │ Entity Framework Core
         ↓
┌─────────────────┐
│  SQL Server     │
│   (Database)    │
└─────────────────┘
```

## Stack Tecnológico

**Backend:**
- ASP.NET Core 8.0
- Entity Framework Core
- SQL Server 2019
- Swagger/OpenAPI

**Frontend:**
- Angular 18
- TypeScript 5
- Bootstrap 5
- RxJS

## Estructura del Proyecto

```
pokemon-explorer/
│
├── PokeAPI/
│   ├── Controllers/
│   ├── Services/
│   ├── Repositories/
│   ├── Models/
│   ├── Data/
│   └── README.md
│
└── pokemon-app/
    ├── src/app/
    │   ├── components/
    │   ├── services/
    │   └── models/
    └── README.md
```

## Instalación y Ejecución

### Paso 1: Configurar Base de Datos

1. Abrir SQL Server Management Studio (SSMS)
2. Ejecutar el script SQL proporcionado en el README del backend
3. Verificar que la base de datos `PokemonDB` se haya creado correctamente

### Paso 2: Ejecutar Backend

```bash
cd PokeAPI
dotnet restore
dotnet run
```

Verificar que el servidor esté disponible en `http://localhost:5262`

### Paso 3: Ejecutar Frontend

En una terminal separada:

```bash
cd pokemon-app
npm install
ng serve
```

Acceder a la aplicación en `http://localhost:4200`

## Funcionalidades Implementadas

### Requisitos Funcionales

- Listado de Pokémon con imagen, nombre, ID y tipos
- Búsqueda por nombre exacto
- Filtro por tipo (18 tipos disponibles)
- Selector de cantidad por página (10, 20, 50, 100)
- Paginación con navegación entre páginas
- Vista de detalle con información completa (stats, habilidades)
- Manejo de estados: cargando, error, sin resultados

### Requisitos Técnicos

- Frontend consume exclusivamente el backend propio
- Backend desarrollado en ASP.NET Core
- Backend consume PokeAPI externa
- Persistencia de datos en SQL Server
- Uso de HttpClient y modelos tipados
- Manejo centralizado de errores
- Separación por capas en backend (Controller → Service → Repository)
- Arquitectura de componentes standalone en frontend

## Documentación Adicional

- [Backend README](./PokeBackend/PokeAPI/README.md) - Detalles de arquitectura, endpoints y configuración
- [Frontend README](./poke-frontend/README.md) - Componentes, servicios y estructura

## Autor

Carlos Daniel Pérez Serrano  
Desarrollador Web Full Stack  
Enero 2026
