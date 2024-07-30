using TheProjectTascamon.DBContext;
using TheProjectTascamon.IRepos;
using TheProjectTascamon.Models;

namespace TheProjectTascamon.Repos
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly TascamonContext _context;

        public PokemonRepository(TascamonContext context)
        {
            _context = context;
        }

        public async Task CreatePokemon(Pokemon pokemon)
        {
            _context.Pokemons.Add(pokemon);
            await _context.SaveChangesAsync();
        }


    }
}
