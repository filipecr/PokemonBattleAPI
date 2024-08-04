using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TheProjectTascamon.Models;

namespace TheProjectTascamon.Service
{
    public interface IBattleService
    {
        Task CreateBattleAsync(string player1, string player2);
        Task BattleStartAsync(string player1, string player2, string battleId);
        Task<(bool IsBattleOver, int? WinnerId)> CheckBattleOverAsync(string battleId);
        Task BattleEndWinnerUpdateAsync(string battleId, int winner);
        Task<List<Pokemon>> GetAlivePokemonForTrainerAsync(string battleId, string playerName);
    }
}
