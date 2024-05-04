using System.Collections;
using IronEngine;

namespace IronCheckers
{
    public class GameInstanceManager : Runtime
    {
        public static void StartNewGame(string whitePlayerName, string blackPlayerName)
        {
            var Game = new GameInstanceManager(whitePlayerName, blackPlayerName);
            Game.StartGame();
        }

        public void StartGame()
        {
            currentPlayer = whitePlayer;
            //start game loop
        }
        
        private Player whitePlayer, blackPlayer;
        private Player currentPlayer;
        
        private GameInstanceManager(string whitePlayerName, string blackPlayerName)
        {
            whitePlayer = new Player(whitePlayerName, this);
            blackPlayer = new Player(blackPlayerName, this);
        }

        public void OnRecieveInput(string input)
        {
            
        }

        public static void EndGame(Player losingPlayer)
        {
            
        }

        protected override bool ExitCondition { get; }
        protected override IEnumerable<Actor> CreateActors()
        {
            throw new NotImplementedException();
        }

        protected override TileMap CreateTileMap()
        {
            throw new NotImplementedException();
        }

        protected override void OnExit()
        {
            throw new NotImplementedException();
        }
    }
}

