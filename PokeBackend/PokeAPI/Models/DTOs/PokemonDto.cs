namespace PokeAPI.Models.DTOs
{
    // DTO para listado
    public class PokemonListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SpriteUrl { get; set; } = string.Empty;
        public List<string> Types { get; set; } = new();
    }

    // DTO para detalle
    public class PokemonDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Height { get; set; }
        public int Weight { get; set; }
        public int BaseExperience { get; set; }
        public string SpriteUrl { get; set; } = string.Empty;
        public List<string> Types { get; set; } = new();
        public List<PokemonStatDto> Stats { get; set; } = new();
        public List<PokemonAbilityDto> Abilities { get; set; } = new();
    }

    public class PokemonStatDto
    {
        public string Name { get; set; } = string.Empty;
        public int BaseStat { get; set; }
    }

    public class PokemonAbilityDto
    {
        public string Name { get; set; } = string.Empty;
    }

    // DTO para respuesta paginada
    public class PokemonListResponse
    {
        public int TotalCount { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public string? NextUrl { get; set; }
        public string? PreviousUrl { get; set; }
        public List<PokemonListItemDto> Pokemon { get; set; } = new();
    }

    // DTOs para consumir PokeAPI
    public class PokeApiListResponse
    {
        public int Count { get; set; }
        public string? Next { get; set; }
        public string? Previous { get; set; }
        public List<PokeApiNamedResource> Results { get; set; } = new();
    }

    public class PokeApiNamedResource
    {
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }

    public class PokeApiPokemonDetail
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Height { get; set; }
        public int Weight { get; set; }
        public int Base_Experience { get; set; }
        public PokeApiSprites Sprites { get; set; } = new();
        public List<PokeApiTypeSlot> Types { get; set; } = new();
        public List<PokeApiStatSlot> Stats { get; set; } = new();
        public List<PokeApiAbilitySlot> Abilities { get; set; } = new();
    }

    public class PokeApiSprites
    {
        public string? Front_Default { get; set; }
    }

    public class PokeApiTypeSlot
    {
        public PokeApiType Type { get; set; } = new();
    }

    public class PokeApiType
    {
        public string Name { get; set; } = string.Empty;
    }

    public class PokeApiStatSlot
    {
        public int Base_Stat { get; set; }
        public PokeApiStat Stat { get; set; } = new();
    }

    public class PokeApiStat
    {
        public string Name { get; set; } = string.Empty;
    }

    public class PokeApiAbilitySlot
    {
        public PokeApiAbility Ability { get; set; } = new();
    }

    public class PokeApiAbility
    {
        public string Name { get; set; } = string.Empty;
    }

    public class PokeApiTypeResponse
    {
        public List<TypePokemonEntry> Pokemon { get; set; } = new();
    }

    public class TypePokemonEntry
    {
        public PokemonReference Pokemon { get; set; } = new();
    }

    public class PokemonReference
    {
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
