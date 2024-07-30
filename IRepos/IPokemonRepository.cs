using TheProjectTascamon.Models;

namespace TheProjectTascamon.IRepos
{
    public interface IPokemonRepository
    {
         Task CreatePokemon(Pokemon pokemon);
         
    }
}
