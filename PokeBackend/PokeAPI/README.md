# Pokémon Explorer API - Backend

API REST desarrollada en ASP.NET Core que actúa como capa intermedia entre el frontend y la PokeAPI. Implementa persistencia en base de datos y arquitectura por capas.

## Tecnologías Utilizadas

- ASP.NET Core 8.0
- Entity Framework Core
- MySQL 8.0
- Swagger/OpenAPI

## Arquitectura del Proyecto

El proyecto implementa una arquitectura por capas (Layered Architecture) para separar responsabilidades:

```
PokeAPI/
│
├── Controllers/              # Endpoints REST
│   └── PokemonController.cs
│
├── Services/                 # Lógica de negocio
│   ├── IPokemonService.cs
│   └── PokemonService.cs
│
├── Repositories/             # Acceso a datos
│   ├── IPokemonRepository.cs
│   └── PokemonRepository.cs
│
├── Models/                   # Entidades y DTOs
│   ├── Pokemon.cs
│   ├── PokemonType.cs
│   └── DTOs/
│       ├── PokemonListResponse.cs
│       └── PokemonDetailDto.cs
│
├── Data/                     # DbContext
│   └── AppDbContext.cs
│
├── appsettings.json
└── Program.cs
```

### Flujo de Datos

```
Cliente HTTP → Controller → Service → Repository → Base de Datos
                              ↓
                         PokeAPI
```

## Base de Datos

### Script de Creación

```sql
CREATE DATABASE PokemonDB;
GO

USE PokemonDB;
GO

CREATE TABLE Pokemon (
    Id INT PRIMARY KEY,
    Name NVARCHAR(255) NOT NULL,
    Height INT,
    Weight INT,
    BaseExperience INT,
    SpriteUrl NVARCHAR(500),
    CreatedAt DATETIME2 DEFAULT GETDATE(),
    UpdatedAt DATETIME2 DEFAULT GETDATE()
);

CREATE TABLE PokemonTypes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PokemonId INT NOT NULL,
    TypeName NVARCHAR(50) NOT NULL,
    FOREIGN KEY (PokemonId) REFERENCES Pokemon(Id) ON DELETE CASCADE
);
```

### Estrategia de Persistencia

El backend implementa un patrón de cache lazy loading:

- Los datos se consultan primero desde la API externa
- Se persisten en la base de datos para consultas futuras
- Si la API externa falla, se sirven datos desde la base de datos local
- Los registros existentes se actualizan con información reciente

## Instalación y Configuración

### Requisitos

- .NET 8.0 SDK o superior
- MySQL Server 8.0 o superior

### Configuración

1. Crear la base de datos ejecutando el script SQL proporcionado

2. Configurar la cadena de conexión en `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=PokeDB;User=root;Password=TU_PASSWORD;"
  },
  "PokeApi": {
    "BaseUrl": "https://pokeapi.co/api/v2"
  }
}
```

3. Restaurar dependencias:

```bash
dotnet restore
```

4. Ejecutar el proyecto:

```bash
dotnet run
```

La API estará disponible en `http://localhost:5262` y la documentación Swagger en `http://localhost:5262/swagger`

## Endpoints

### GET /api/pokemon

Lista paginada de Pokémon con filtros opcionales.

**Parámetros:**
- `limit` (int): Cantidad por página (1-100), por defecto 20
- `offset` (int): Registros a saltar, por defecto 0
- `name` (string, opcional): Filtrar por nombre
- `type` (string, opcional): Filtrar por tipo (fire, water, grass, etc.)

**Ejemplo:**
```
GET /api/pokemon?limit=20&offset=0&type=fire&name=charmander
```

**Respuesta:**
```json
{
  "pokemon": [
    {
      "id": 4,
      "name": "charmander",
      "spriteUrl": "https://...",
      "types": ["fire"]
    }
  ],
  "totalCount": 3,
  "limit": 20,
  "offset": 0,
  "nextUrl": null,
  "previousUrl": null
}
```

### GET /api/pokemon/{id}

Detalle completo de un Pokémon por ID.

**Ejemplo:**
```
GET /api/pokemon/25
```

**Respuesta:**
```json
{
  "id": 25,
  "name": "pikachu",
  "height": 4,
  "weight": 60,
  "baseExperience": 112,
  "spriteUrl": "https://...",
  "types": ["electric"],
  "stats": [
    {
      "name": "hp",
      "baseStat": 35
    }
  ],
  "abilities": [
    {
      "name": "static"
    }
  ]
}
```

### GET /api/pokemon/health

Endpoint de verificación del estado de la API.

**Respuesta:**
```json
{
  "status": "OK",
  "message": "PokeAPI está funcionando correctamente",
  "timestamp": "2026-01-30T18:40:00"
}
```

## Decisiones de Diseño

**Arquitectura por Capas:** Facilita el mantenimiento y testing al separar responsabilidades. Cada capa tiene dependencias únicamente hacia las capas inferiores.

**Backend Intermedio:** Permite controlar qué datos se exponen, implementar caching, y proporcionar resiliencia ante fallos de la API externa.

**DTOs:** Se utilizan objetos de transferencia de datos separados de las entidades de base de datos para desacoplar la API pública de la estructura interna.

**Filtros Combinables:** El sistema permite combinar nombre y tipo para búsquedas precisas. La paginación se aplica después de filtrar los resultados.

## Autor

Carlos Daniel Pérez Serrano  
Prueba Técnica - Desarrollador Web Full Stack
