using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TheProjectTascamon.DBContext;
using TheProjectTascamon.IRepos;
using TheProjectTascamon.Models;
using TheProjectTascamon.Repos;

public class MoveRepository : IMoveRepository
{
    private readonly TascamonContext _context;

    public MoveRepository(TascamonContext context)
    {
        _context = context;
    }

    public async Task<Pokemon> GetPokemonByIdAsync(int pokemonId)
    {
        return await _context.Pokemons.SingleOrDefaultAsync(p => p.PokemonId == pokemonId);
    }

    public async Task<Move> GetMoveByIdAsync(int moveId)
    {
        return await _context.Moves.SingleOrDefaultAsync(m => m.MovesId == moveId);
    }

    public async Task<BattleStatsLog> GetBattleStatsLogAsync(int pokemonId, string battleId)
    {
        return await _context.BattleStatsLogs.SingleOrDefaultAsync(b => b.PokemonId == pokemonId && b.BattleId == battleId);
    }

    public void UpdateBattleStatsLogAsync(BattleStatsLog log)
    {
        _context.BattleStatsLogs.Update(log);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task CreateMoveAsync(Move move)
    {
        _context.Moves.Add(move);
        await _context.SaveChangesAsync();

    }

}
