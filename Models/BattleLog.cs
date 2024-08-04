using System;
using System.Collections.Generic;

namespace TheProjectTascamon.Models;

public partial class BattleLog
{
    public string? BattleId { get; set; }

    public int PlayerId { get; set; }

    public int Turn { get; set; }

    public int CurrentPokemonId { get; set; }

    public string Action { get; set; } = null!;

    public virtual Battle Battle { get; set; } = null!;

    public virtual Pokemon CurrentPokemon { get; set; } = null!;

    public virtual User Player { get; set; } = null!;
}
