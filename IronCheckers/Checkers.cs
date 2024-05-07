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
            TileMap tileMap = new(8, 8, new Tile());
			tileMap[1, 7].Object = new Piece(blackPlayer, -1);
			tileMap[0, 0].Object = new Piece(whitePlayer, 1);
			return tileMap;
        }

        protected override void OnExit()
        {
            Console.WriteLine("Game ended");
        }
    }
}

