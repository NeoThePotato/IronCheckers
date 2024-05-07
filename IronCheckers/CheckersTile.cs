using IronEngine;

namespace IronCheckers
{
	public class CheckersTile : Tile
	{
		public override string ToString() => Checkers.ToChessIndexing(TileMap, Position);
	}
}
