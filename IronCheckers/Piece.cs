using IronEngine;

namespace IronCheckers
{
	public class Piece : TileObject, ICommandAble, ICommandAble.IHasKey
	{
		public int movementDirectionY;

		public override Func<IMoveable, Tile, IEnumerable<Tile>> DefaultMovementStrategy => IMoveable.ShortestDirect;

		public Piece(Player player, int movementDirection) : base(player)
		{
			if (movementDirection == 0)
				movementDirectionY = 1;
			else
				movementDirectionY = Math.ClampRange(movementDirection, -1, 1);
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
				if (TryGetDiagonalAction(1, out var action))
					yield return action;
				if (TryGetDiagonalAction(-1, out action))
					yield return action;
			}
		}

		protected bool TryGetDiagonalAction(int xOffset, out ICommandAble.Command action)
		{
			var tile = GetDiagonalTile(xOffset);
			if (TryMove(tile, out action))
				return true;
			else if (TryCapture(tile, xOffset, out action))
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

		protected bool TryCapture(Tile? tile, int xOffset, out ICommandAble.Command action)
		{
			if (HasFoePiece(tile, out Piece? piece))
			{
				tile = GetDiagonalTile(xOffset, 2);
				if (ValidAndEmpty(tile))
				{
					action = new(() => Move(tile!), $"Capture {piece}", tile!.ToString());
					return true;
				}
			}
			action = default;
			return false;
		}

		protected static bool ValidAndEmpty(Tile? tile) => tile != null && !tile.HasObject;

		protected bool HasFoePiece(Tile? tile, out Piece? piece) => tile.TryGetObject(out piece) && piece!.Actor != Actor;

		protected Tile? GetDiagonalTile(int xOffset, int steps = 1) => TileMap![Position + new Position(xOffset, movementDirectionY) * steps];

		public override string ToString() => $"{Actor}'s piece at {CurrentTile}";

		public string? Key => CurrentTile?.ToString();

		public string Description => ToString();
	}
}
