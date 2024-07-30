using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TheProjectTascamon.DBContext;
using TheProjectTascamon.Models;
using TheProjectTascamon.IRepos;
using Microsoft.EntityFrameworkCore.Storage;

public class BattleRepository : IBattleRepository
{
    private readonly TascamonContext _context;

    public BattleRepository(TascamonContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.SingleOrDefaultAsync(u => u.UserName == username);
    }

    public async Task AddBattleAsync(Battle battle)
    {
        await _context.Battles.AddAsync(battle);
    }

    public async Task AddUsersBattleAsync(UsersBattle usersBattle)
    {
        await _context.UsersBattles.AddAsync(usersBattle);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<List<TrainerPokemonMove>> GetTrainerPokemonMovesAsync(int userId)
    {
        return await _context.TrainerPokemonMoves.Where(u => u.UsernameId == userId).ToListAsync();
    }

    public async Task<Pokemon> GetPokemonByIdAsync(int pokemonId)
    {
        return await _context.Pokemons.SingleOrDefaultAsync(p => p.PokemonId == pokemonId);
    }

    public async Task AddBattleStatsLogAsync(BattleStatsLog battleStatsLog)
    {
        await _context.BattleStatsLogs.AddAsync(battleStatsLog);
    }

    public async Task<Battle> GetBattleByIdAsync(string battleId)
    {
        return await _context.Battles.SingleOrDefaultAsync(b => b.BattleId == battleId);
    }

    public async Task<List<UsersBattle>> GetUsersBattlesByBattleIdAsync(string battleId)
    {
        return await _context.UsersBattles.Where(b => b.BattleId == battleId).ToListAsync();
    }

    public async Task AddBattleLogAsync(BattleLog battleLog)
    {
        await _context.BattleLogs.AddAsync(battleLog);
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        await transaction.CommitAsync();
    }

    public async Task RollbackTransactionAsync(IDbContextTransaction transaction)
    {
        await transaction.RollbackAsync();
    }
}

