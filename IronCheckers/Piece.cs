using IronEngine;

namespace IronCheckers
{
	class Piece : TileObject
	{
		public int movementDirectionY;

		public Piece(Tile tile, Actor actor, int movementDirection) : base(tile, actor)
		{
			if (movementDirection == 0)
				movementDirectionY = 1;
			else
				movementDirectionY = Math.ClampRange(movementDirection, -1, 1);
		}

		private void OnObjectPass(TileObject other)
		{
			if (other.Actor != Actor)
				Destroy();
		}

		public override IEnumerable<Func<bool>>? GetAvailableActions()
		{
			if (TileMap != null)
			{
				yield return GetDiagonalAction(1);
				yield return GetDiagonalAction(-1);
			}
		}

		private Func<bool>? GetDiagonalAction(int xOffset)
		{
			var tile = GetDiagonalTile(xOffset);
			if (ValidAndEmpty(tile))
				return () => { Move(tile); return true; };
			else if (HasFoePiece(tile))
			{
				tile = GetDiagonalTile(xOffset, 2);
				if (ValidAndEmpty(tile))
					return () => { Move(tile); return true; };
			}
			return null;
		}

		private static bool ValidAndEmpty(Tile? tile) => tile != null && !tile.HasObject;

		private bool HasFoePiece(Tile? tile) => tile != null && tile.HasObject && tile.Object!.Actor != Actor;

		private Tile? GetDiagonalTile(int xOffset, int steps = 1) => TileMap![Position + new Position(xOffset, movementDirectionY) * steps];
	}
}
