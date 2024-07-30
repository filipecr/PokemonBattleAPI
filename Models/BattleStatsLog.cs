using System;
using System.Collections.Generic;

namespace TheProjectTascamon.Models;

public partial class BattleStatsLog
{
    public string BattleId { get; set; }

    public decimal? HpC { get; set; }

    public decimal? ApC { get; set; }

    public decimal? MagicResC { get; set; }

    public decimal? SpeedC { get; set; }

    public string? StatusEffect { get; set; }

    public int? PokemonId { get; set; }

    public int? PlayerId { get; set; }

    public virtual Battle? Battle { get; set; }

    public virtual Pokemon? Pokemon { get; set; }
}
