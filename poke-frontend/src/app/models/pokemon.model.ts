// Model para listado
export interface PokemonListItem {
  id: number;
  name: string;
  spriteUrl: string;
  types: string[];
}

// Model para respuesta paginada
export interface PokemonListResponse {
  totalCount: number;
  limit: number;
  offset: number;
  nextUrl: string | null;
  previousUrl: string | null;
  pokemon: PokemonListItem[];
}

// Model para detalle
export interface PokemonDetail {
  id: number;
  name: string;
  height: number;
  weight: number;
  baseExperience: number;
  spriteUrl: string;
  types: string[];
  stats: PokemonStat[];
  abilities: PokemonAbility[];
}

export interface PokemonStat {
  name: string;
  baseStat: number;
}

export interface PokemonAbility {
  name: string;
}
