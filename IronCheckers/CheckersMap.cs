using DefaultRenderer.Defaults;
using IronEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronCheckers
{
	public class CheckersMap : RenderableTileMap
	{
		public CheckersMap(int sizeX, int sizeY, Tile? fillWith = null) : base(sizeX, sizeY, fillWith)
		{ }

		public IEnumerable<CheckersTile> CheckerboardTiles(uint offset = 0) => CheckerboardTiles(this, offset);

		public static IEnumerable<CheckersTile> CheckerboardTiles(TileMap tileMap, uint offset = 0) => tileMap.Checkerboard(offset).Cast<CheckersTile>();

		public static string ToChessIndexing(TileMap tileMap, Position position) => $"{ToChessIndexingX(position.x)}{ToChessIndexingY(tileMap, position.y)}";

		public static int ToChessIndexingY(TileMap tileMap, int y) => tileMap.SizeY - y;

		public static char ToChessIndexingX(int x) => (char)(65 + x);
	}
}
