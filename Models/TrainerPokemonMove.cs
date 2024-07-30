using System;
using System.Collections.Generic;

namespace TheProjectTascamon.Models;

public partial class TrainerPokemonMove
{
    public int MovesId { get; set; }

    public int PokemonId { get; set; }

    public int UsernameId { get; set; }

    public int Slot { get; set; }

    public virtual Move Moves { get; set; } = null!;

    public virtual Pokemon Pokemon { get; set; } = null!;

    public virtual User Username { get; set; } = null!;
}
