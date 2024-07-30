using TheProjectTascamon.ViewModel;

namespace TheProjectTascamon.IService
{
    public interface IPokemonService
    {
        Task CreatePokemon(PokemonViewModel pokemonDto);
    }
}
