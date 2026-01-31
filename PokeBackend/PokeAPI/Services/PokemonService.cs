using PokeAPI.Models;
using PokeAPI.Models.DTOs;
using PokeAPI.Repositories;
using System.Text.Json;

namespace PokeAPI.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokemonRepository _repository;
        private readonly HttpClient _httpClient;
        private readonly string _pokeApiBaseUrl;
        private readonly JsonSerializerOptions _jsonOptions; // ← AGREGAR

        public PokemonService(IPokemonRepository repository, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _repository = repository;
            _httpClient = httpClientFactory.CreateClient();
            _pokeApiBaseUrl = configuration["PokeApi:BaseUrl"] ?? "https://pokeapi.co/api/v2";

            // ← AGREGAR ESTO
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            };
        }

        public async Task<PokemonListResponse> GetPokemonListAsync(int limit, int offset, string? name, string? type)
        {
            // CASO 1: Filtro por tipo (con o sin nombre)
            if (!string.IsNullOrWhiteSpace(type))
            {
                try
                {
                    var response = await _httpClient.GetAsync($"{_pokeApiBaseUrl}/type/{type.ToLower()}");
                    response.EnsureSuccessStatusCode();

                    var content = await response.Content.ReadAsStringAsync();
                    var typeResponse = JsonSerializer.Deserialize<PokeApiTypeResponse>(content, _jsonOptions);

                    if (typeResponse == null || typeResponse.Pokemon.Count == 0)
                    {
                        return new PokemonListResponse { TotalCount = 0, Limit = limit, Offset = offset };
                    }

                    var allTypePokemon = typeResponse.Pokemon;

                    // Si también hay filtro por nombre, aplicarlo
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        allTypePokemon = allTypePokemon
                            .Where(p => p.Pokemon.Name.Contains(name.ToLower(), StringComparison.OrdinalIgnoreCase))
                            .ToList();
                    }

                    var totalCount = allTypePokemon.Count;
                    var paginatedPokemon = allTypePokemon.Skip(offset).Take(limit).ToList();
                    var pokemonList = new List<PokemonListItemDto>();

                    foreach (var item in paginatedPokemon)
                    {
                        var detail = await FetchAndSavePokemonAsync(item.Pokemon.Name);
                        if (detail != null)
                        {
                            pokemonList.Add(new PokemonListItemDto
                            {
                                Id = detail.Id,
                                Name = detail.Name,
                                SpriteUrl = detail.SpriteUrl,
                                Types = detail.Types
                            });
                        }
                    }

                    return new PokemonListResponse
                    {
                        TotalCount = totalCount,
                        Limit = limit,
                        Offset = offset,
                        NextUrl = offset + limit < totalCount ? $"/api/pokemon?limit={limit}&offset={offset + limit}&type={type}" : null,
                        PreviousUrl = offset > 0 ? $"/api/pokemon?limit={limit}&offset={Math.Max(0, offset - limit)}&type={type}" : null,
                        Pokemon = pokemonList
                    };
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al obtener Pokémon por tipo {type}: {ex.Message}");
                    return new PokemonListResponse { TotalCount = 0, Limit = limit, Offset = offset };
                }
            }

            // CASO 2: Solo búsqueda por nombre (sin tipo)
            if (!string.IsNullOrWhiteSpace(name))
            {
                var pokemon = await GetPokemonByNameAsync(name);
                if (pokemon != null)
                {
                    return new PokemonListResponse
                    {
                        TotalCount = 1,
                        Limit = limit,
                        Offset = 0,
                        Pokemon = new List<PokemonListItemDto>
                {
                    new PokemonListItemDto
                    {
                        Id = pokemon.Id,
                        Name = pokemon.Name,
                        SpriteUrl = pokemon.SpriteUrl,
                        Types = pokemon.Types
                    }
                }
                    };
                }
                else
                {
                    return new PokemonListResponse { TotalCount = 0, Limit = limit, Offset = offset };
                }
            }

            // CASO 3: Sin filtros - listado normal
            var localCount = await _repository.GetTotalCountAsync();
            var localPokemon = await _repository.GetAllAsync(limit, offset);

            if (localPokemon.Count == limit)
            {
                return new PokemonListResponse
                {
                    TotalCount = 1025,
                    Limit = limit,
                    Offset = offset,
                    NextUrl = offset + limit < 1025 ? $"/api/pokemon?limit={limit}&offset={offset + limit}" : null,
                    PreviousUrl = offset > 0 ? $"/api/pokemon?limit={limit}&offset={Math.Max(0, offset - limit)}" : null,
                    Pokemon = localPokemon.Select(p => new PokemonListItemDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        SpriteUrl = p.SpriteUrl,
                        Types = p.Types.Select(t => t.TypeName).ToList()
                    }).ToList()
                };
            }

            try
            {
                var response = await _httpClient.GetAsync($"{_pokeApiBaseUrl}/pokemon?limit={limit}&offset={offset}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<PokeApiListResponse>(content, _jsonOptions);

                if (apiResponse == null || apiResponse.Results.Count == 0)
                {
                    return new PokemonListResponse { TotalCount = 0, Limit = limit, Offset = offset };
                }

                var pokemonList = new List<PokemonListItemDto>();

                foreach (var item in apiResponse.Results)
                {
                    var detail = await FetchAndSavePokemonAsync(item.Name);
                    if (detail != null)
                    {
                        pokemonList.Add(new PokemonListItemDto
                        {
                            Id = detail.Id,
                            Name = detail.Name,
                            SpriteUrl = detail.SpriteUrl,
                            Types = detail.Types
                        });
                    }
                }

                return new PokemonListResponse
                {
                    TotalCount = apiResponse.Count,
                    Limit = limit,
                    Offset = offset,
                    NextUrl = apiResponse.Next != null ? $"/api/pokemon?limit={limit}&offset={offset + limit}" : null,
                    PreviousUrl = apiResponse.Previous != null ? $"/api/pokemon?limit={limit}&offset={Math.Max(0, offset - limit)}" : null,
                    Pokemon = pokemonList
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener lista de Pokémon: {ex.Message}");

                return new PokemonListResponse
                {
                    TotalCount = localCount,
                    Limit = limit,
                    Offset = offset,
                    Pokemon = localPokemon.Select(p => new PokemonListItemDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        SpriteUrl = p.SpriteUrl,
                        Types = p.Types.Select(t => t.TypeName).ToList()
                    }).ToList()
                };
            }
        }

        public async Task<PokemonDetailDto?> GetPokemonByIdAsync(int id)
        {
            // Siempre consultar a PokeAPI para tener datos completos
            return await FetchAndSavePokemonAsync(id.ToString());
        }


        public async Task<PokemonDetailDto?> GetPokemonByNameAsync(string name)
        {
            // Siempre consultar a PokeAPI para tener datos completos
            return await FetchAndSavePokemonAsync(name.ToLower());
        }


        private async Task<PokemonDetailDto?> FetchAndSavePokemonAsync(string idOrName)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_pokeApiBaseUrl}/pokemon/{idOrName}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var apiPokemon = JsonSerializer.Deserialize<PokeApiPokemonDetail>(content, _jsonOptions);

                if (apiPokemon == null) return null;

                // Verificar si ya existe
                var existingPokemon = await _repository.GetByIdAsync(apiPokemon.Id);

                if (existingPokemon != null)
                {
                    // Actualizar
                    existingPokemon.Name = apiPokemon.Name;
                    existingPokemon.Height = apiPokemon.Height;
                    existingPokemon.Weight = apiPokemon.Weight;
                    existingPokemon.BaseExperience = apiPokemon.Base_Experience;
                    existingPokemon.SpriteUrl = apiPokemon.Sprites.Front_Default ?? "";

                    // Actualizar tipos
                    existingPokemon.Types.Clear();
                    existingPokemon.Types = apiPokemon.Types.Select(t => new PokemonType
                    {
                        PokemonId = apiPokemon.Id,
                        TypeName = t.Type.Name
                    }).ToList();

                    await _repository.UpdateAsync(existingPokemon);
                    return await MapToDetailDto(existingPokemon, apiPokemon);
                }
                else
                {
                    // Guardar nuevo
                    var newPokemon = new Pokemon
                    {
                        Id = apiPokemon.Id,
                        Name = apiPokemon.Name,
                        Height = apiPokemon.Height,
                        Weight = apiPokemon.Weight,
                        BaseExperience = apiPokemon.Base_Experience,
                        SpriteUrl = apiPokemon.Sprites.Front_Default ?? "",
                        Types = apiPokemon.Types.Select(t => new PokemonType
                        {
                            PokemonId = apiPokemon.Id,
                            TypeName = t.Type.Name
                        }).ToList()
                    };

                    await _repository.SaveAsync(newPokemon);
                    return await MapToDetailDto(newPokemon, apiPokemon);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener Pokémon {idOrName}: {ex.Message}");
                return null;
            }
        }

        private Task<PokemonDetailDto> MapToDetailDto(Pokemon pokemon, PokeApiPokemonDetail? apiDetail = null)
        {
            var dto = new PokemonDetailDto
            {
                Id = pokemon.Id,
                Name = pokemon.Name,
                Height = pokemon.Height,
                Weight = pokemon.Weight,
                BaseExperience = pokemon.BaseExperience,
                SpriteUrl = pokemon.SpriteUrl,
                Types = pokemon.Types.Select(t => t.TypeName).ToList()
            };

            // Si tenemos datos de la API, agregar stats y abilities
            if (apiDetail != null)
            {
                dto.Stats = apiDetail.Stats.Select(s => new PokemonStatDto
                {
                    Name = s.Stat.Name,
                    BaseStat = s.Base_Stat
                }).ToList();

                dto.Abilities = apiDetail.Abilities.Select(a => new PokemonAbilityDto
                {
                    Name = a.Ability.Name
                }).ToList();
            }

            return Task.FromResult(dto);
        }

    }
}
