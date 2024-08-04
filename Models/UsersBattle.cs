using System;
using System.Collections.Generic;

namespace TheProjectTascamon.Models;

public partial class UsersBattle
{
    public string? BattleId { get; set; }

    public int UsersId { get; set; }

    public virtual Battle? Battle { get; set; }

    public virtual User? Player { get; set; }
}
