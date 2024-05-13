namespace IronCheckers
{
	public class King : Man
	{
		private static readonly string[] KING_CHARS = [
			@"[/\]",
			@"[  ]",
			@"[\/]"
		];

		public King(Player player) : base(player, 1)
		{
			Chars = KING_CHARS;
			_movementFunc = GetDiagonalTiles;
		}

		public override string ToString() => $"{Actor}'s king at {CurrentTile}";
	}
}
