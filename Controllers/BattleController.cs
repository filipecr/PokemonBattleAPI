using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheProjectTascamon.Service;

namespace TheProjectTascamon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BattleController : ControllerBase
    {
        private readonly IBattleService _battleService;

        public BattleController(IBattleService battleService)
        {
            _battleService = battleService;
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

        [HttpGet("CheckBattleOver")]
        public async Task<IActionResult> CheckBattleOverAsync(string battleId)
        {
            if (string.IsNullOrWhiteSpace(battleId))
            {
                return BadRequest("Battle ID cannot be empty.");
            }

            try
            {
                await _battleService.CheckBattleOverAsync(battleId);
                return Ok("Battle check completed.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
