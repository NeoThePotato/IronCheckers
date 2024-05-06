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
				yield return GetDiagonalAction(1);
				yield return GetDiagonalAction(-1);
			}
		}

		private IActionable.Action GetDiagonalAction(int xOffset)
		{
			var tile = GetDiagonalTile(xOffset);
			if (ValidAndEmpty(tile))
				return new($"Move to {tile}" , () => { Move(tile); return true; });
			else if (HasFoePiece(tile, out Piece? piece))
			{
				tile = GetDiagonalTile(xOffset, 2);
				if (ValidAndEmpty(tile))
					return new($"Eat {piece} at {piece!.CurrentTile}", () => { Move(tile); return true; });
			}
			return default;
		}

		private static bool ValidAndEmpty(Tile? tile) => tile != null && !tile.HasObject;

		private bool HasFoePiece(Tile? tile, out Piece? piece)
		{
			piece = null;
			if (tile == null)
			{
				return false;
			}
			else if (tile.HasObject && tile.Object is Piece pieceTemp)
			{
				piece = pieceTemp;
				return piece.Actor != Actor;
			}
			return false;
		}

		private Tile? GetDiagonalTile(int xOffset, int steps = 1) => TileMap![Position + new Position(xOffset, movementDirectionY) * steps];
	}
}
