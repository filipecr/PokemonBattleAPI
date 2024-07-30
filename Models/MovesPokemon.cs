using System;
using System.Collections.Generic;

namespace TheProjectTascamon.Models;

public partial class MovesPokemon
{
    public int MovesId { get; set; }

    public int PokemonId { get; set; }

    public virtual Move Moves { get; set; } = null!;

    public virtual Pokemon Pokemon { get; set; } = null!;
}
