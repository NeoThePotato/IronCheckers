using IronEngine;

namespace IronCheckers
{
	public class King : Piece
	{
		private static readonly string[] KING_CHARS = [
			@"[/\]",
			@"[  ]",
			@"[\/]"
		];

		public King(Player player) : base(player, 0)
		{
			Chars = KING_CHARS;
		}

		protected override IEnumerable<Tile?> GetDiagonalTiles(int steps = 1) => GetDiagonalPosition(1, steps).MirrorXY(Position).ToTiles(TileMap);

		public override string ToString() => $"{Actor}'s king at {CurrentTile}";
	}
}
