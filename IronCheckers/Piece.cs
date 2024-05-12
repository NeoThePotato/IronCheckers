using IronEngine;
using DefaultRenderer.Defaults;

namespace IronCheckers
{
	public class Piece : RenderableTileObject, ICommandAble, ICommandAble.IHasKey
	{
		private static readonly string[] PIECE_CHARS = [@"[  ]"];

		public int movementDirectionY;
		public bool capturedThisTurn = false;

		public override Func<IMoveable, Tile, IEnumerable<Tile>> DefaultMovementStrategy => IMoveable.ShortestDirect;

		public Piece(Player player, int movementDirection) : base(player)
		{
			if (movementDirection == 0)
				movementDirectionY = 1;
			else
				movementDirectionY = Math.ClampRange(movementDirection, -1, 1);
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
				foreach (var tile in GetDiagonalTiles(Position, 1))
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
			if (ValidAndEmpty(tile))
			{
				action = new(() => Move(tile!), $"Move to {tile}", tile!.ToString());
				return true;
			}
			action = default;
			return false;
		}

		protected bool TryCapture(Tile? tile, Position startPosition, out ICommandAble.Command action, bool deepCheck = true)
		{
			if (HasFoePiece(tile, out Piece? piece))
			{
				var endTile = TileMap[startPosition.FlipXY(tile.Position)];
				if (ValidAndEmpty(endTile))
				{
					bool canZigZag = false;
					if (deepCheck)
					{
						foreach (var target in GetDiagonalTiles(endTile.Position, 1).Where(t => t != tile))
						{
							canZigZag = TryCapture(target, endTile.Position, out var _, false);
							if (canZigZag)
								break;
						}
					}
					action = new(() => { Move(endTile!); capturedThisTurn = true; }, $"Capture {piece}", endTile!.ToString(), !canZigZag);
					return true;
				}
			}
			action = default;
			return false;
		}

		protected static bool ValidAndEmpty(Tile? tile) => tile != null && !tile.HasObject;

		protected bool HasFoePiece(Tile? tile, out Piece? piece) => tile.TryGetObject(out piece) && piece!.Actor != Actor;

		protected Position GetDiagonalPosition(Position startingPosition, int xOffset, int steps = 1) => startingPosition + new Position(xOffset, movementDirectionY) * steps;

		protected Position GetDiagonalPosition(int xOffset, int steps = 1) => GetDiagonalPosition(Position, xOffset, steps);

		protected virtual IEnumerable<Tile?> GetDiagonalTiles(Position startingPosition, int steps = 1) => GetDiagonalPosition(startingPosition, 1, steps).MirrorX(Position).ToTiles(TileMap);

		public override string ToString() => $"{Actor}'s piece at {CurrentTile}";

		public string? Key => CurrentTile?.ToString();

		public string Description => ToString();
	}
}
