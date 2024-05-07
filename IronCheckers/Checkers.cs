using IronEngine;
using IronEngine.IO;

namespace IronCheckers
{
    public class Checkers : Runtime
    {
        public Player whitePlayer, BlackPlayer;

        public IEnumerable<Player> Players => (IEnumerable<Player>)Actors;

		protected override bool ExitCondition => Players.Any(a => !a.HasPiecesLeft);

		protected override IInput Input => IInput.ConsoleInput;

		protected override IEnumerable<Actor> CreateActors()
		{
			yield return whitePlayer = new Player(Input.GetString("White Player, please enter your name: "));
			yield return BlackPlayer = new Player(Input.GetString("Black Player, please enter your name: "));
			Console.WriteLine("Let the games begin!");
		}

        protected override TileMap CreateTileMap()
        {
            TileMap tileMap = new(8, 8, new Tile());
			tileMap[0, 0].Object = new Piece(BlackPlayer, 1);
            return tileMap;
        }

        protected override void OnExit()
        {
            Console.WriteLine("Game ended");
        }
    }
}

