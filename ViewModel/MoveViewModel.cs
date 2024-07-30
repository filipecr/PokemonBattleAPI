using TheProjectTascamon.Models;

namespace TheProjectTascamon.ViewModel
{
    public class MoveViewModel
    {
        public int MovesId { get; set; }

        public string? MoveName { get; set; }

        public int? MovePower { get; set; }

        public string? MoveStatus { get; set; }

        public int? MoveApChange { get; set; }

        public int? MoveHpChange { get; set; }

        public GameType MoveType { get; set; }
    }
}
