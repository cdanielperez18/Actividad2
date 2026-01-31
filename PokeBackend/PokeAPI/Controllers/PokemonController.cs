using Microsoft.AspNetCore.Mvc;
using PokeAPI.Models.DTOs;
using PokeAPI.Services;

namespace PokeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;

        public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        /// <summary>
        /// Obtiene una lista paginada de Pokémon
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<PokemonListResponse>> GetPokemon(
            [FromQuery] int limit = 20,
            [FromQuery] int offset = 0,
            [FromQuery] string? name = null,
            [FromQuery] string? type = null)
        {
            if (limit < 1 || limit > 100)
            {
                return BadRequest("El límite debe estar entre 1 y 100");
            }

            if (offset < 0)
            {
                return BadRequest("El offset no puede ser negativo");
            }

            var result = await _pokemonService.GetPokemonListAsync(limit, offset, name, type);
            return Ok(result);
        }

        /// <summary>
        /// Obtiene el detalle de un Pokémon por ID
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<PokemonDetailDto>> GetPokemonById(int id)
        {
            if (id < 1)
            {
                return BadRequest("El ID debe ser mayor a 0");
            }

            var pokemon = await _pokemonService.GetPokemonByIdAsync(id);

            if (pokemon == null)
            {
                return NotFound($"No se encontró el Pokémon con ID {id}");
            }

            return Ok(pokemon);
        }

        /// <summary>
        /// Obtiene el detalle de un Pokémon por nombre
        /// </summary>
        [HttpGet("{name}")]
        public async Task<ActionResult<PokemonDetailDto>> GetPokemonByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("El nombre no puede estar vacío");
            }

            var pokemon = await _pokemonService.GetPokemonByNameAsync(name);

            if (pokemon == null)
            {
                return NotFound($"No se encontró el Pokémon '{name}'");
            }

            return Ok(pokemon);
        }

        /// <summary>
        /// Endpoint de salud
        /// </summary>
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new
            {
                status = "OK",
                message = "PokeAPI está funcionando correctamente",
                timestamp = DateTime.Now
            });
        }
    }
}
