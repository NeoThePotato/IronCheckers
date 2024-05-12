using IronEngine;

namespace IronCheckers
{
	public class KingMaker : CheckersTile
	{
		public KingMaker(Actor actor) : base()
		{
			Actor = actor;
		}

		public override void OnObjectEnter(TileObject other)
		{
			if (other.Actor == Actor && Actor is Player player)
				MakeKing(other, player);
		}

		private void MakeKing(TileObject piece, Player owner)
		{
			piece.Destroy();
			Object = new King(owner);
		}
	}
}
