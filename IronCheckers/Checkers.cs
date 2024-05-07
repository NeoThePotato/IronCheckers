using IronEngine;
using IronEngine.IO;

namespace IronCheckers
{
    public class Checkers : Runtime
    {
        public Player whitePlayer, blackPlayer;

        public IEnumerable<Player> Players => Actors.Cast<Player>();

		protected override bool ExitCondition => Players.Any(a => !a.HasPiecesLeft);

		protected override IInput Input => IInput.ConsoleInput;

		protected override IEnumerable<Actor> CreateActors()
		{
			yield return whitePlayer = new Player(Input.GetString("White Player, please enter your name: "));
			yield return blackPlayer = new Player(Input.GetString("Black Player, please enter your name: "));
			Console.WriteLine("Let the games begin!");
		}

        protected override TileMap CreateTileMap()
        {
            TileMap tileMap = new(8, 8, new CheckersTile());
			foreach (var tile in CheckerboardTiles(tileMap).Where(t => t.Position.y > 4))
				tile.Object = new Piece(whitePlayer, -1);
			foreach (var tile in CheckerboardTiles(tileMap).Where(t => t.Position.y < 3))
				tile.Object = new Piece(blackPlayer, 1);
			return tileMap;
        }

        protected override void OnExit()
        {
            Console.WriteLine("Game ended");
        }

		public IEnumerable<CheckersTile> CheckerboardTiles() => CheckerboardTiles(TileMap);

		public static IEnumerable<CheckersTile> CheckerboardTiles(TileMap tileMap) => tileMap.Checkerboard(1).Cast<CheckersTile>();

		public static string ToChessIndexing(TileMap tileMap, Position position) => $"{ToChessIndexingX(position.x)}{ToChessIndexingY(tileMap, position.y)}";

		public static int ToChessIndexingY(TileMap tileMap, int y) => tileMap.SizeY - y;

		public static char ToChessIndexingX(int x) => (char)(65 + x);
	}
}

