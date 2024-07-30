using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheProjectTascamon.Models;
using TheProjectTascamon.IRepos;

namespace TheProjectTascamon.Service
{
    public class BattleService : IBattleService
    {
        private readonly IBattleRepository _battleRepository;
        private readonly UserService _userService;
        private readonly ILogger<BattleService> _logger;

        public BattleService(IBattleRepository battleRepository, UserService userService, ILogger<BattleService> logger)
        {
            _battleRepository = battleRepository;
            _userService = userService;
            _logger = logger;
        }

        public string BattleIdGenerator(string player1, string player2)
        {
            _logger.LogInformation("Generating battle ID for players {Player1} and {Player2}", player1, player2);
            string trimmedPlayer1 = player1.Trim().ToLower();
            string trimmedPlayer2 = player2.Trim().ToLower();
            string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            string battleId = $"{trimmedPlayer1}_{trimmedPlayer2}_{timestamp}";
            return battleId;
        }

        public async Task CreateBattleAsync(string player1, string player2)
        {
            try
            {
                _logger.LogInformation("Creating battle between {Player1} and {Player2}", player1, player2);
                string battleUniqueId = BattleIdGenerator(player1, player2);

                var trainer1 = await _battleRepository.GetUserByUsernameAsync(player1);
                var trainer2 = await _battleRepository.GetUserByUsernameAsync(player2);

                if (trainer1 == null || trainer2 == null)
                {
                    _logger.LogError("One or both users not found: {Player1}, {Player2}", player1, player2);
                    throw new Exception("One or both users not found");
                }

                var battle = new Battle
                {
                    BattleId = battleUniqueId,
                    StartTime = DateTime.UtcNow
                };

                var usersBattle1 = new UsersBattle
                {
                    BattleId = battleUniqueId,
                    UsersId = trainer1.Id,
                };

                var usersBattle2 = new UsersBattle
                {
                    BattleId = battleUniqueId,
                    UsersId = trainer2.Id,
                };

                await _battleRepository.AddBattleAsync(battle);
                await _battleRepository.AddUsersBattleAsync(usersBattle1);
                await _battleRepository.AddUsersBattleAsync(usersBattle2);
                await _battleRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating battle between {Player1} and {Player2}", player1, player2);
                throw;
            }
        }


        public void PlayFirstPokemon(string player, string battleId)
        {
            try
            {
                _logger.LogInformation("Playing first Pokémon for player {Player} in battle {BattleId}", player, battleId);
                var user = _userService.GetUserByUsernameAsync(player);
                int userId = user.Id;
                var firstPokemon = _battleRepository.GetTrainerPokemonMovesAsync(userId).Result.SingleOrDefault(u => u.Slot == 1);

                if (firstPokemon == null)
                {
                    _logger.LogError("First Pokémon not found for player {Player}", player);
                    throw new Exception("First Pokémon not found.");
                }

                var battleLog = new BattleLog
                {
                    BattleId = battleId,
                    PlayerId = userId,
                    Turn = 0,
                    CurrentPokemonId = firstPokemon.PokemonId,
                    Action = "SetupTurn"
                };

                _battleRepository.AddBattleLogAsync(battleLog).Wait();
                _battleRepository.SaveChangesAsync().Wait();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error playing first Pokémon for player {Player} in battle {BattleId}", player, battleId);
                throw;
            }
        }

        public async Task RegisterAllPokemonOnLog(string player, string battleId)
        {
            try
            {
                _logger.LogInformation("Registering all Pokémon on log for player {Player} in battle {BattleId}", player, battleId);
                var user = await _userService.GetUserByUsernameAsync(player);
                int userId = user.Id;
                var trainerPokemonList = await _battleRepository.GetTrainerPokemonMovesAsync(userId);

                foreach (var item in trainerPokemonList)
                {
                    var pokemon = await _battleRepository.GetPokemonByIdAsync(item.PokemonId);
                    var battleStatsLog = new BattleStatsLog
                    {
                        BattleId = battleId,
                        PlayerId = userId,
                        PokemonId = item.PokemonId,
                        HpC = pokemon.Hp,
                        ApC = pokemon.Ap,
                        MagicResC = pokemon.MagicRes,
                        SpeedC = pokemon.Speed,
                        StatusEffect = "None",
                    };
                    await _battleRepository.AddBattleStatsLogAsync(battleStatsLog);
                }

                await _battleRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering all Pokémon on log for player {Player} in battle {BattleId}", player, battleId);
                throw;
            }
        }

        public async Task BattleStartAsync(string player1, string player2, string battleId)
        {
            using var transaction = await _battleRepository.BeginTransactionAsync();
            try
            {
                _logger.LogInformation("Starting battle between {Player1} and {Player2} in battle {BattleId}", player1, player2, battleId);
                PlayFirstPokemon(player1, battleId);
                PlayFirstPokemon(player2, battleId);
                await RegisterAllPokemonOnLog(player1, battleId);
                await RegisterAllPokemonOnLog(player2, battleId);
                await _battleRepository.CommitTransactionAsync(transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting battle {BattleId}", battleId);
                await _battleRepository.RollbackTransactionAsync(transaction);
                throw;
            }
        }

        public async Task CheckBattleOverAsync(string battleId)
        {
            try
            {
                _logger.LogInformation("Checking if battle {BattleId} is over", battleId);
                var battle = await _battleRepository.GetBattleByIdAsync(battleId);
                // Add logic to check if the battle is over
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if battle {BattleId} is over", battleId);
                throw;
            }
        }

        public async Task BattleEndWinnerUpdateAsync(string battleId, int winner)
        {
            try
            {
                _logger.LogInformation("Updating battle {BattleId} with winner {Winner}", battleId, winner);
                var battle = await _battleRepository.GetBattleByIdAsync(battleId);
                var players = await _battleRepository.GetUsersBattlesByBattleIdAsync(battleId);

                var player1 = players[0];
                var player2 = players[1];

                if (player1.UsersId == winner)
                {
                    battle.Winner = player1.UsersId;
                }
                else if (player2.UsersId == winner)
                {
                    battle.Winner = player2.UsersId;
                }
                else
                {
                    _logger.LogError("Winner {Winner} is not a participant in battle {BattleId}", winner, battleId);
                    throw new ArgumentException("The Winner is not a participant in this battle.");
                }

                await _battleRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating winner for battle {BattleId}", battleId);
                throw;
            }
        }
    }
}
