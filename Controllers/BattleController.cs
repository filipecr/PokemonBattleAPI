using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TheProjectTascamon.Models;
using TheProjectTascamon.Service;

namespace TheProjectTascamon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BattleController : ControllerBase
    {
        private readonly IBattleService _battleService;
        private readonly ILogger<BattleController> _logger;

        public BattleController(IBattleService battleService, ILogger<BattleController> logger)
        {
            _battleService = battleService;
            _logger = logger;
        }

        [HttpPost("CreateBattle")]
        public async Task<IActionResult> CreateBattleAsync(string player1, string player2)
        {
            if (string.IsNullOrWhiteSpace(player1) || string.IsNullOrWhiteSpace(player2))
            {
                return BadRequest("Player names cannot be empty.");
            }

            try
            {
                await _battleService.CreateBattleAsync(player1, player2);
                return Ok("Battle created successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("StartBattle")]
        public async Task<IActionResult> StartBattleAsync(string player1, string player2, string battleId)
        {
            if (string.IsNullOrWhiteSpace(player1) || string.IsNullOrWhiteSpace(player2) || string.IsNullOrWhiteSpace(battleId))
            {
                return BadRequest("Invalid parameters.");
            }

            try
            {
                await _battleService.BattleStartAsync(player1, player2, battleId);
                return Ok("Battle started successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("EndBattle")]
        public async Task<IActionResult> EndBattleAsync(string battleId, int winner)
        {
            if (string.IsNullOrWhiteSpace(battleId) || winner <= 0)
            {
                return BadRequest("Invalid parameters.");
            }

            try
            {
                await _battleService.BattleEndWinnerUpdateAsync(battleId, winner);
                return Ok("Battle ended and winner updated successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        

        [HttpGet("/IsOverAndWinnner")]
        public async Task<IActionResult> IsBattleOver(string battleId)
        {
            try
            {
                (bool isBattleOver, int? winnerId) = await _battleService.CheckBattleOverAsync(battleId);
                return Ok(new { IsBattleOver = isBattleOver, WinnerId = winnerId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if battle is over");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{battleId}/alive-pokemon/{playerName}")]
        public async Task<IActionResult> GetAlivePokemonForTrainer(string battleId, string playerName)
        {
            try
            {
                var alivePokemon = await _battleService.GetAlivePokemonForTrainerAsync(battleId, playerName);
                return Ok(alivePokemon);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving alive Pokémon for player {PlayerName} in battle {BattleId}", playerName, battleId);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
