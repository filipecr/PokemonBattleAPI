using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TheProjectTascamon.IRepos;
using TheProjectTascamon.IService;
using TheProjectTascamon.Models;
using TheProjectTascamon.Repos;
using TheProjectTascamon.ViewModel;

namespace TheProjectTascamon.Service
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokemonRepository _PokemonRepository;
        private readonly ILogger<PokemonService> _logger;
        private readonly IMapper _mapper;

        public PokemonService(IPokemonRepository pokemonRepository, ILogger<PokemonService> logger, IMapper mapper)
        {
            _PokemonRepository = pokemonRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task CreatePokemon(PokemonViewModel pokemonDto)
        {
            if (string.IsNullOrWhiteSpace(pokemonDto.PokemonName))
            {
                throw new ArgumentException("Pokemon name cannot be null or whitespace.", nameof(pokemonDto.PokemonName));
            }

            if (pokemonDto.Hp <= 0 || pokemonDto.Ap <= 0 || pokemonDto.MagicRes <= 0 || pokemonDto.Speed <= 0)
            {
                throw new ArgumentException("HP, AP, MagicRes, and Speed must be greater than zero.");
            }
            
            // Map MoveViewModel to entity
            var Pokemon = _mapper.Map<Pokemon>(pokemonDto);
            try
            {
                await _PokemonRepository.CreatePokemon(Pokemon);
                _logger.LogInformation($"Pokemon {pokemonDto.PokemonName} created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating Pokemon {pokemonDto.PokemonName}");
                throw new ApplicationException($"An error occurred while creating the Pokemon: {ex.Message}");
            }
        }

    }
}
