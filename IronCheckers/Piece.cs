using IronEngine;
using DefaultRenderer.Defaults;

namespace IronCheckers
{
	public class Piece : RenderableTileObject, ICommandAble, ICommandAble.IHasKey
	{
		private static readonly string[] PIECE_CHARS = [@"[  ]"];

		public int movementDirectionY;

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
				foreach (var tile in GetDiagonalTiles(1))
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
			else if (TryCapture(tile, out action))
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

		protected bool TryCapture(Tile? tile, out ICommandAble.Command action)
		{
			if (HasFoePiece(tile, out Piece? piece))
			{
				tile = TileMap[Position.FlipXY(tile.Position)];
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

		protected Position GetDiagonalPosition(int xOffset, int steps = 1) => Position + new Position(xOffset, movementDirectionY) * steps;

		protected virtual IEnumerable<Tile?> GetDiagonalTiles(int steps = 1) => GetDiagonalPosition(1, steps).MirrorX(Position).ToTiles(TileMap);

		public override string ToString() => $"{Actor}'s piece at {CurrentTile}";

		public string? Key => CurrentTile?.ToString();

		public string Description => ToString();
	}
}
