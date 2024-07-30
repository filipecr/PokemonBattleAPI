using TheProjectTascamon.Models;
using TheProjectTascamon.ViewModel;

namespace TheProjectTascamon.IRepos
{
    public interface IMoveRepository
    {
        Task CreateMoveAsync(Move moveDto);
        Task<Pokemon> GetPokemonByIdAsync(int pokemonId);
        Task<Move> GetMoveByIdAsync(int moveId);
        Task<BattleStatsLog> GetBattleStatsLogAsync(int pokemonId, string battleId);
        void UpdateBattleStatsLogAsync(BattleStatsLog log);
        Task SaveChangesAsync();
    }
}
