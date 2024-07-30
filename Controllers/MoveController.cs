using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheProjectTascamon.IService;
using TheProjectTascamon.Service;
using TheProjectTascamon.ViewModel;

namespace TheProjectTascamon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoveController : ControllerBase
    {

        private readonly IMoveService _moveService;
        private readonly ILogger<MoveController> _logger;
        public MoveController(IMoveService moveService, ILogger<MoveController> logger)
        {
            _moveService = moveService;
            _logger = logger;
        }

        [HttpPost("AttackMove")]
        public async Task<IActionResult> AttackMoveAsync(int attackerPokemonID, int targetPokemonID, int moveID, string battleId)
        {
            if (attackerPokemonID <= 0 || targetPokemonID <= 0 || moveID <= 0 || string.IsNullOrWhiteSpace(battleId))
            {
                return BadRequest("Invalid parameters.");
            }

            try
            {
                await _moveService.AttackMoveAsync(attackerPokemonID, targetPokemonID, moveID, battleId);
                return Ok("Attack move executed successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here as needed
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("CreateMove")]
        public async Task<IActionResult> CreateMove([FromBody] MoveViewModel moveDto)
        {
            if (moveDto == null)
            {
                return BadRequest("move data is null.");
            }

            try
            {
                await _moveService.CreateMoveAsync(moveDto);
                return Ok($"Move {moveDto.MoveName} created successfully.");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid input provided for creating a Move.");
                return BadRequest(ex.Message);
            }
            catch (ApplicationException ex)
            {
                _logger.LogError(ex, "An error occurred while creating Move.");
                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }
    }
}
