using IronEngine;

namespace IronCheckers
{
	public class King : Man
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

		protected override IEnumerable<Tile?> GetDiagonalTiles(Position startingPosition, int steps = 1) => GetDiagonalPosition(startingPosition, 1, steps).MirrorXY(Position).ToTiles(TileMap);

		public override string ToString() => $"{Actor}'s king at {CurrentTile}";
	}
}
