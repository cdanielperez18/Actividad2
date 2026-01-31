# Pokémon Explorer - Frontend

Aplicación web desarrollada en Angular 18 para explorar y consultar información de Pokémon. Consume una API REST intermedia que se comunica con PokeAPI.

## Tecnologías Utilizadas

- Angular 18
- TypeScript 5
- Bootstrap 5
- RxJS
- Angular Router

## Arquitectura del Proyecto

```
pokemon-app/
│
├── components/
│   ├── pokemon-list/          # Listado con filtros y paginación
│   │   ├── pokemon-list.component.ts
│   │   ├── pokemon-list.component.html
│   │   └── pokemon-list.component.css
│   │
│   └── pokemon-detail/         # Vista detallada
│       ├── pokemon-detail.component.ts
│       ├── pokemon-detail.component.html
│       └── pokemon-detail.component.css
│
├── services/
│   └── pokemon.service.ts      # Comunicación con API
│
├── models/
│   └── pokemon.model.ts        # Interfaces TypeScript
│
├── app.component.ts
└── app.routes.ts
```

### Flujo de Datos

```
Componente → Servicio → Backend API → PokeAPI
     ↓
  Template (HTML)
```

## Instalación y Configuración

### Requisitos

- Node.js 18 o superior
- Angular CLI 18

### Configuración

1. Instalar dependencias:

```bash
npm install
```

2. Configurar URL del backend en `pokemon.service.ts`:

```typescript
private apiUrl = 'http://localhost:5262/api/pokemon';
```

3. Ejecutar en modo desarrollo:

```bash
ng serve
```

La aplicación estará disponible en `http://localhost:4200`

## Funcionalidades

### Paginación

Sistema de paginación que permite:
- Selector de cantidad por página (10, 20, 50, 100)
- Navegación con botones Anterior/Siguiente
- Números de página clickeables
- Indicador de resultados actuales

**Implementación:**
```typescript
// Calcular página actual
this.currentPage = Math.floor(this.offset / this.limit) + 1;

// Navegar
nextPage(): void {
  this.offset += this.limit;
  this.loadPokemon();
}
```

### Búsqueda por Nombre

Campo de texto para buscar Pokémon por nombre exacto.
- Se activa con Enter o botón Buscar
- Búsqueda case-insensitive
- Muestra mensaje si no hay resultados

### Filtro por Tipo

18 botones visuales con colores representativos para filtrar por tipo:
- Fire, Water, Electric, Grass, Ice, Fighting, Poison, Ground, Flying
- Psychic, Bug, Rock, Ghost, Dragon, Dark, Steel, Fairy, Normal

**Características:**
- Filtrado instantáneo al hacer clic
- Toggle: clic nuevamente para deseleccionar
- Feedback visual en botón seleccionado

### Vista de Detalle

Página con información completa:
- Imagen del Pokémon
- Datos básicos (altura, peso, experiencia)
- Tipos
- Estadísticas con barras de progreso
- Habilidades

## Modelos TypeScript

### PokemonListItemDto
```typescript
export interface PokemonListItemDto {
  id: number;
  name: string;
  spriteUrl: string;
  types: string[];
}
```

### PokemonDetailDto
```typescript
export interface PokemonDetailDto {
  id: number;
  name: string;
  height: number;
  weight: number;
  baseExperience: number;
  spriteUrl: string;
  types: string[];
  stats: PokemonStatDto[];
  abilities: PokemonAbilityDto[];
}
```

### PokemonListResponse
```typescript
export interface PokemonListResponse {
  pokemon: PokemonListItemDto[];
  totalCount: number;
  limit: number;
  offset: number;
  nextUrl?: string;
  previousUrl?: string;
}
```

## Decisiones de Diseño

**Separación Input/Filtro:** El texto en el buscador no se aplica hasta presionar Buscar, evitando requests innecesarios mientras se escribe.

**Filtro de Tipo Instantáneo:** Los tipos son un conjunto fijo (18), permitiendo filtrado inmediato sin impacto en rendimiento.

**ChangeDetectorRef:** Se utiliza detección de cambios manual para asegurar actualización del DOM después de operaciones asíncronas.

**Bootstrap 5:** Proporciona diseño responsive y profesional sin CSS custom extensivo.

## Autor

Carlos Daniel Pérez Serrano  
Prueba Técnica - Desarrollador Web Full Stack
