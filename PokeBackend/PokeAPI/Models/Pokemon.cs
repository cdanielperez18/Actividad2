namespace PokeAPI.Models
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Height { get; set; }
        public int Weight { get; set; }
        public int BaseExperience { get; set; }
        public string SpriteUrl { get; set; } = string.Empty;
        public DateTime LastUpdated { get; set; } = DateTime.Now;

        // Relación con tipos
        public List<PokemonType> Types { get; set; } = new();
    }

    public class PokemonType
    {
        public int Id { get; set; }
        public int PokemonId { get; set; }
        public string TypeName { get; set; } = string.Empty;

        // Relación inversa
        public Pokemon Pokemon { get; set; } = null!;
    }
}
