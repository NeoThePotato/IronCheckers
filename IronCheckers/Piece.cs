using IronEngine;

namespace IronCheckers
{
	public class Piece : TileObject, IActionable
	{
		public int movementDirectionY;

		public Piece(Tile tile, Actor actor, int movementDirection) : base(tile, actor)
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

		public IEnumerable<IActionable.Action> GetAvailableActions()
		{
			if (TileMap != null)
			{
				if (TryGetDiagonalAction(1, out var action))
					yield return action;
				if (TryGetDiagonalAction(-1, out action))
					yield return action;
			}
		}

		private bool TryGetDiagonalAction(int xOffset, out IActionable.Action action)
		{
			var tile = GetDiagonalTile(xOffset);
			if (ValidAndEmpty(tile))
			{
				action = new($"Move to {tile}", () => { Move(tile); return true; });
				return true;
			}
			else if (HasFoePiece(tile, out Piece? piece))
			{
				tile = GetDiagonalTile(xOffset, 2);
				if (ValidAndEmpty(tile))
				{
					action = new($"Eat {piece} at {piece!.CurrentTile}", () => { Move(tile); return true; });
					return true;
				}
			}
			action = default;
			return false;
		}

		private static bool ValidAndEmpty(Tile? tile) => tile != null && !tile.HasObject;

		private bool HasFoePiece(Tile? tile, out Piece? piece) => tile.TryGetObject(out piece) && piece!.Actor != Actor;

		private Tile? GetDiagonalTile(int xOffset, int steps = 1) => TileMap![Position + new Position(xOffset, movementDirectionY) * steps];
	}
}
