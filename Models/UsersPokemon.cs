using System;
using System.Collections.Generic;

namespace TheProjectTascamon.Models;

public partial class UsersPokemon
{
    public int UsersId { get; set; }

    public int PokemonId { get; set; }

    public int Slot { get; set; }

    public virtual Pokemon Pokemon { get; set; } = null!;

    public virtual User Users { get; set; } = null!;
}
