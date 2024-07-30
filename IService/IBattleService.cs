using System.Threading.Tasks;

namespace TheProjectTascamon.Service
{
    public interface IBattleService
    {
        Task CreateBattleAsync(string player1, string player2);
        Task BattleStartAsync(string player1, string player2, string battleId);
        Task CheckBattleOverAsync(string battleId);
        Task BattleEndWinnerUpdateAsync(string battleId, int winner);
    }
}
