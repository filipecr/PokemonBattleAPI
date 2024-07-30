using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheProjectTascamon.Service;
using TheProjectTascamon.ViewModel;

namespace TheProjectTascamon.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly PokemonService _pokemonService;
        private readonly ILogger<PokemonController> _logger;

        public PokemonController(PokemonService pokemonService, ILogger<PokemonController> logger)
        {
            _pokemonService = pokemonService;
            _logger = logger;
        }

        [HttpPost("CreatePokemon")]
        public async Task<IActionResult> CreatePokemon([FromBody] PokemonViewModel pokemonDto)
        {
            if (pokemonDto == null)
            {
                return BadRequest("Pokemon data is null.");
            }

            try
            {
                await _pokemonService.CreatePokemon(pokemonDto);
                return Ok($"Pokemon {pokemonDto.PokemonName} created successfully.");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid input provided for creating Pokemon.");
                return BadRequest(ex.Message);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "An error occurred while creating Pokemon.");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
    }
}
