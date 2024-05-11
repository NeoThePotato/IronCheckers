using DefaultRenderer.Defaults;

namespace IronCheckers
{
	public class CheckersTile : RenderableTile
	{
		public override string ToString() => CheckersMap.ToChessIndexing(TileMap, Position);
	}
}
