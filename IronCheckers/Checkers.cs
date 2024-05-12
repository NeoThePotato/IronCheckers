using IronEngine;
using IronEngine.IO;
using IronEngine.DefaultRenderer;

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
			yield return whitePlayer = new Player(Input.GetString("White Player, please enter your name: "), ConsoleRenderer.COLOR_WHITE);
			yield return blackPlayer = new Player(Input.GetString("Black Player, please enter your name: "), 8);
			Console.WriteLine("Let the games begin!");
		}

        protected override TileMap CreateTileMap()
        {
            CheckersMap tileMap = new(8, 8, new CheckersTile());
			foreach (var tile in tileMap.CheckerboardTiles())
				tile.BgColor = ConsoleRenderer.COLOR_WHITE;
			foreach (var tile in tileMap.CheckerboardTiles(1).Where(t => t.Position.y > 4))
				tile.Object = new Piece(whitePlayer, -1);
			foreach (var tile in tileMap.CheckerboardTiles(1).Where(t => t.Position.y < 3))
				tile.Object = new Piece(blackPlayer, 1);
			return tileMap;
        }

		protected override IRenderer CreateRenderer()
		{
			return new ConsoleRenderer(TileMap);
		}

		protected override void OnGameStart()
		{
			(Input as ConsoleInput).SelectCommandAblePrompt = "Select Piece:";
		}

		protected override void OnExit()
        {
            Console.WriteLine("Game ended");
        }
	}
}

