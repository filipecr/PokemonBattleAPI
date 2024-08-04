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
        var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == username);
        if (user == null)
        {
            throw new Exception($"User with username {username} not found.");
        }
        return user;
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
        var pokemon = await _context.Pokemons.SingleOrDefaultAsync(p => p.PokemonId == pokemonId);
        if (pokemon == null)
        {
            throw new Exception($"Pokemon with ID {pokemonId} not found.");
        }
        return pokemon;
    }

    public async Task AddBattleStatsLogAsync(BattleStatsLog battleStatsLog)
    {
        await _context.BattleStatsLogs.AddAsync(battleStatsLog);
    }

    public async Task<Battle> GetBattleByIdAsync(string battleId)
    {
        var battle = await _context.Battles.SingleOrDefaultAsync(b => b.BattleId == battleId);
        if (battle == null)
        {
            throw new Exception($"Battle with ID {battleId} not found.");
        }
        return battle;
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

    public async Task<bool> AnyAlivePokemon(string battleId, int playerId)
    {       
        return await _context.BattleStatsLogs
            .Where(b => b.BattleId == battleId && b.PlayerId == playerId && b.HpC > 0 )
            .AnyAsync();        
    }

    public async Task<List<Pokemon>> GetAlivePokemonForTrainerAsync(string battleId, int userId)
    {
        return await _context.BattleStatsLogs
            .Where(b => b.BattleId == battleId && b.PlayerId == userId && b.HpC > 0)
            .Join(_context.Pokemons,
                  b => b.PokemonId,
                  p => p.PokemonId,
                  (b, p) => p)
            .ToListAsync();
    }
}

