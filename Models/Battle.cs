using System;
using System.Collections.Generic;

namespace TheProjectTascamon.Models;

public partial class Battle
{
    public string BattleId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public int? Winner { get; set; }

    public virtual User? WinnerNavigation { get; set; }
}
