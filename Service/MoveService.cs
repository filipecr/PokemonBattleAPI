using AutoMapper;
using System;
using System.Linq;
using System.Threading.Tasks;
using TheProjectTascamon.IRepos;
using TheProjectTascamon.IService;
using TheProjectTascamon.Models;
using TheProjectTascamon.Repos;
using TheProjectTascamon.Service;
using TheProjectTascamon.ViewModel;

namespace Tascamon.Service
{
    public class MoveService : IMoveService
    {
        private readonly IMoveRepository _moveRepository;
        private readonly ILogger<MoveService> _logger;
        private readonly IMapper _mapper;

        public MoveService(IMoveRepository moveRepository, ILogger<MoveService> logger, IMapper mapper)
        {
            _moveRepository = moveRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task CreateMoveAsync(MoveViewModel moveDto)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(moveDto.MoveName))
            {
                throw new ArgumentException("Move name cannot be null or whitespace.", nameof(moveDto.MoveName));
            }

            if (moveDto.MovePower <= 0)
            {
                throw new ArgumentException("Move power must be greater than zero.", nameof(moveDto.MovePower));
            }

            // Map MoveViewModel to Move entity
            var move = _mapper.Map<Move>(moveDto);

            try
            {
                await _moveRepository.CreateMoveAsync(move);
                _logger.LogInformation($"Move {moveDto.MoveName} created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating move {moveDto.MoveName}");
                throw new ApplicationException($"An error occurred while creating the move: {ex.Message}");
            }
        }



        public async Task AttackMoveAsync(int attackerPokemonID, int targetPokemonID, int moveID, string battleid)
        {
            var attacker = await _moveRepository.GetPokemonByIdAsync(attackerPokemonID);
            var target = await _moveRepository.GetPokemonByIdAsync(targetPokemonID);
            var move = await _moveRepository.GetMoveByIdAsync(moveID);

            if (attacker == null || target == null || move == null)
            {
                throw new ArgumentException("Invalid attacker, target, or move.");
            }

            // Logic to calculate and apply damage
            double multiplier = GetTypeMultiplier(move.MoveType, target.PokemonType);
            int bonus = attacker.PokemonType == move.MoveType ? 1 : 0;
            double? damage = CalculateDamage(attacker.Ap, target.MagicRes, multiplier, move.MovePower, bonus);

            // Apply the damage to the log table
            await UpdateHPBattleStatsLogAsync(targetPokemonID, damage, battleid);
            await _moveRepository.SaveChangesAsync();
        }

        private async Task UpdateHPBattleStatsLogAsync(int targetPokemonID, double? damage, string battleid)
        {
            var battleStatsLog = await _moveRepository.GetBattleStatsLogAsync(targetPokemonID, battleid);

            if (battleStatsLog != null)
            {
                battleStatsLog.HpC -= (decimal?)damage;
                _moveRepository.UpdateBattleStatsLogAsync(battleStatsLog);
            }
            else
            {
                throw new ArgumentException("BattleStatsLog not found for target Pokemon.");
            }
        }

        private double? CalculateDamage(int? ap, int? magicres, double multipler, int? movepower, int bonus)
        {
            if (magicres == 0)
            {
                throw new ArgumentException("MagicRes cannot be zero.");
            }

            double? damage = (ap + movepower) / magicres * multipler;

            if (bonus > 0)
            {
                damage *= 1.10;
            }

            return damage;
        }

        private double GetTypeMultiplier(GameType moveType, GameType targetType)
        {
            double multiplier = 1.0;

            switch (moveType)
            {
                case GameType.Fire:
                    if (targetType == GameType.Grass)
                        multiplier *= 2.0;
                    else if (targetType == GameType.Water)
                        multiplier *= 0.5;
                    break;
                case GameType.Water:
                    if (targetType == GameType.Fire)
                        multiplier *= 2.0;
                    else if (targetType == GameType.Grass)
                        multiplier *= 0.5;
                    break;
                case GameType.Grass:
                    if (targetType == GameType.Water)
                        multiplier *= 2.0;
                    else if (targetType == GameType.Fire)
                        multiplier *= 0.5;
                    break;
            }

            return multiplier;
        }
    }
}
