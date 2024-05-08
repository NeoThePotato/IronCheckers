using IronEngine;
using static IronRenderer.ConsoleRenderer;

namespace IronRenderer
{
	internal class TileObjectRenderer : IConsoleRenderer
	{
		protected TileObject _tileObject;

		public FrameBuffer Buffer { get; set; }
		
		public int SizeJ => TileRenderer.SIZE_X;
		public int SizeI => TileRenderer.SIZE_Y;
		public (int, int) Size => (SizeJ, SizeI);

		public TileObjectRenderer(TileObject tileObject)
		{
			_tileObject = tileObject;
			Render();
		}

		public virtual void UpdateFrame()
		{
			Render();
		}

		public void UpdateFrameFull()
		{
			Render();
		}

		public void Render()
		{
			for (int i = 0; i < SizeI; i++)
			{
				for (int j = 0; j < SizeJ; j++)
				{
					FrameBuffer buffer = Buffer;
					buffer[j, i] = EMPTY_CHAR;
				}
			}
		}
	}
}
