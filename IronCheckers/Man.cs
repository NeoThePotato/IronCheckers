using IronEngine;
using DefaultRenderer.Defaults;

namespace IronCheckers
{
	using MovementFunc = Func<Position, int, IEnumerable<Tile?>>;

	public class Man : RenderableTileObject, ICommandAble, ICommandAble.IHasKey
	{

		private static readonly string[] PIECE_CHARS = [@"[  ]"];

		protected MovementFunc _movementFunc;
		public bool capturedThisTurn = false;

		public override Func<IMoveable, Tile, IEnumerable<Tile>> DefaultMovementStrategy => IMoveable.ShortestDirect;

		public Man(Player player, int movementDirection) : base(player)
		{
			if (System.Math.Abs(movementDirection) != 1)
				throw new ArgumentOutOfRangeException("Movement direction needs to be either 1 or -1");
			_movementFunc = (p, s) => GetDiagonalForwardTiles(p, movementDirection, s);
			FgColor = player.Color;
			Chars = PIECE_CHARS;
		}

		public override void OnObjectPass(TileObject other)
		{
			if (other.Actor != Actor)
				Destroy();
		}

		public IEnumerable<ICommandAble.Command> GetAvailableActions()
		{
			if (TileMap != null)
			{
				MovementFunc movementFunc = capturedThisTurn ? GetDiagonalTiles : _movementFunc;
				foreach (var tile in movementFunc(Position, 1))
				{
					if (TryGetAction(tile?.CurrentTile, out var action))
						yield return action;
				}
			}
		}

		protected bool TryGetAction(Tile? tile, out ICommandAble.Command action)
		{
			if (TryMove(tile, out action))
				return true;
			else if (TryCapture(tile, Position, out action))
				return true;
			action = default;
			return false;
		}

		protected bool TryMove(Tile? tile, out ICommandAble.Command action)
		{
			if (ValidAndEmpty(tile) && !capturedThisTurn)
			{
				action = new(() => Move(tile!), $"Move to {tile}", tile!.ToString());
				return true;
			}
			action = default;
			return false;
		}

		protected bool TryCapture(Tile? tile, Position startPosition, out ICommandAble.Command action, bool checkZigZag = true)
		{
			if (HasFoePiece(tile, out Man? piece))
			{
				var endTile = TileMap[startPosition.FlipXY(tile.Position)];
				if (ValidAndEmpty(endTile))
				{
					action = new(() => { Move(endTile!); capturedThisTurn = true; }, $"Capture {piece}", tile!.ToString(), !CanZigZag(endTile!, tile));
					return true;
				}
			}
			action = default;
			return false;
		}

		protected bool CanZigZag(Tile tileAfterFirstCapture, Tile exclude) => GetDiagonalTiles(tileAfterFirstCapture.Position).Where(t => t != exclude).Any(t => TryCapture(t, tileAfterFirstCapture.Position, out var _, false));

		protected bool HasFoePiece(Tile? tile, out Man? piece) => tile.TryGetObject(out piece) && piece!.Actor != Actor;

		protected IEnumerable<Tile?> GetDiagonalForwardTiles(Position startingPosition, int yMovementDirection, int steps = 1) => GetDiagonalPosition(startingPosition, 1, yMovementDirection, steps).MirrorX(startingPosition).ToTiles(TileMap);

		protected IEnumerable<Tile?> GetDiagonalTiles(Position startingPosition, int steps = 1) => GetDiagonalPosition(startingPosition, 1, 1, steps).MirrorXY(Position).ToTiles(TileMap);

		public override string ToString() => $"{Actor}'s man at {CurrentTile}";

		public string? Key => CurrentTile?.ToString();

		public string Description => ToString();

		protected static bool ValidAndEmpty(Tile? tile) => tile != null && !tile.HasObject;

		protected static Position GetDiagonalPosition(Position startingPosition, int xOffset, int yOffset, int steps = 1) => startingPosition + new Position(xOffset, yOffset) * steps;
	}
}
