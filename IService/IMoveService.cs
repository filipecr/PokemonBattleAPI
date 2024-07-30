using TheProjectTascamon.ViewModel;

namespace TheProjectTascamon.IService
{
    public interface IMoveService
    {
        Task AttackMoveAsync(int attackerPokemonID, int targetPokemonID, int moveID, string battleid);
        Task CreateMoveAsync(MoveViewModel move);
    }
}
