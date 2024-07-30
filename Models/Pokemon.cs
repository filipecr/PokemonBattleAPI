using System;
using System.Collections.Generic;

namespace TheProjectTascamon.Models;

public partial class Pokemon
{
    public int PokemonId { get; set; }

    public string? PokemonName { get; set; }

    public GameType PokemonType { get; set; }

    public byte[]? Image { get; set; }

    public int? Hp { get; set; }

    public int? Ap { get; set; }

    public int? MagicRes { get; set; }

    public int? Speed { get; set; }
}
