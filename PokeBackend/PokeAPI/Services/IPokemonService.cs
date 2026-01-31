using PokeAPI.Models.DTOs;

namespace PokeAPI.Services
{
    public interface IPokemonService
    {
        Task<PokemonListResponse> GetPokemonListAsync(int limit, int offset, string? name, string? type);
        Task<PokemonDetailDto?> GetPokemonByIdAsync(int id);
        Task<PokemonDetailDto?> GetPokemonByNameAsync(string name);
    }
}
