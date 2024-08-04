using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using TheProjectTascamon.Models;
using System.Threading.Tasks;

namespace TheProjectTascamon.IRepos
{
    public interface IBattleRepository
    {
        Task<User> GetUserByUsernameAsync(string username);
        Task AddBattleAsync(Battle battle);
        Task AddUsersBattleAsync(UsersBattle usersBattle);
        Task SaveChangesAsync();
        Task<List<TrainerPokemonMove>> GetTrainerPokemonMovesAsync(int userId);
        Task<Pokemon> GetPokemonByIdAsync(int pokemonId);
        Task AddBattleStatsLogAsync(BattleStatsLog battleStatsLog);
        Task<Battle> GetBattleByIdAsync(string battleId);
        Task<List<UsersBattle>> GetUsersBattlesByBattleIdAsync(string battleId);
        Task AddBattleLogAsync(BattleLog battleLog);
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync(IDbContextTransaction transaction);
        Task RollbackTransactionAsync(IDbContextTransaction transaction);
        Task<bool> AnyAlivePokemon (string battleId, int playerId);
        Task<List<Pokemon>> GetAlivePokemonForTrainerAsync(string battleId, int userId);
    }

}

