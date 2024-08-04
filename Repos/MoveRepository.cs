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

        var pokemon = await _context.Pokemons.SingleOrDefaultAsync(p => p.PokemonId == pokemonId);
       
        if (pokemon == null)
        {
            throw new KeyNotFoundException($"Pokemon with ID {pokemonId} not found.");
        }

        return pokemon;
    }

    public async Task<Move> GetMoveByIdAsync(int moveId)
    {
        var Move = await _context.Moves.SingleOrDefaultAsync(m => m.MovesId == moveId);
        if (Move == null)
        {
            throw new KeyNotFoundException($"Move with ID {moveId} not found.");
        }
        return Move;
    }

    public async Task<BattleStatsLog> GetBattleStatsLogAsync(int pokemonId, string battleId)
    {
        var battleStatsLog = await _context.BattleStatsLogs
       .SingleOrDefaultAsync(b => b.PokemonId == pokemonId && b.BattleId == battleId);

        if (battleStatsLog == null)
        {
            throw new KeyNotFoundException($"Battle stats log for Pokemon ID {pokemonId} in Battle ID {battleId} not found.");
        }

        return battleStatsLog;


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
