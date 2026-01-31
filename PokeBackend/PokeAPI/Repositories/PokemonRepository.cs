using Microsoft.EntityFrameworkCore;
using PokeAPI.Data;
using PokeAPI.Models;

namespace PokeAPI.Repositories
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly AppDbContext _context;

        public PokemonRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Pokemon?> GetByIdAsync(int id)
        {
            return await _context.Pokemon
                .Include(p => p.Types)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Pokemon?> GetByNameAsync(string name)
        {
            return await _context.Pokemon
                .Include(p => p.Types)
                .FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());
        }

        public async Task<List<Pokemon>> GetAllAsync(int limit, int offset)
        {
            return await _context.Pokemon
                .Include(p => p.Types)
                .OrderBy(p => p.Id)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Pokemon.CountAsync();
        }

        public async Task SaveAsync(Pokemon pokemon)
        {
            await _context.Pokemon.AddAsync(pokemon);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Pokemon pokemon)
        {
            pokemon.LastUpdated = DateTime.Now;
            _context.Pokemon.Update(pokemon);
            await _context.SaveChangesAsync();
        }
    }
}
