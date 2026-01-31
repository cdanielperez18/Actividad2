using PokeAPI.Models;

namespace PokeAPI.Repositories
{
    public interface IPokemonRepository
    {
        Task<Pokemon?> GetByIdAsync(int id);
        Task<Pokemon?> GetByNameAsync(string name);
        Task<List<Pokemon>> GetAllAsync(int limit, int offset);
        Task<int> GetTotalCountAsync();
        Task SaveAsync(Pokemon pokemon);
        Task UpdateAsync(Pokemon pokemon);
    }
}
