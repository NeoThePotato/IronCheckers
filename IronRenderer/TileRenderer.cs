using IronEngine;
using IronEngine.IO;
using static IronRenderer.ConsoleRenderer;

namespace IronRenderer
{
    internal class TileRenderer : IConsoleRenderer
    {
        public const int SIZE_X = 6;
		public const int SIZE_Y = 3;

        private FrameBuffer _cache;
        protected Tile _tile;

        public FrameBuffer Buffer { get; set; }

        public int SizeJ => SIZE_X;
        public int SizeI => SIZE_Y;
        public (int, int) Size => (SizeJ, SizeI);

        public TileRenderer(Tile tile)
        {
            _cache = new FrameBuffer(SizeJ, SizeI);
            _tile = tile;
			RenderToCache();
		}

        public virtual void UpdateFrame()
        {
            FrameBuffer.Copy(Buffer, _cache);
            if (_tile.HasObject)
            {
                if (_tile.Object is IRenderAble rednderable)
                {
                    var rednderer = rednderable.GetRenderer();
					if (rednderer is IConsoleRenderer consoleRenderer)
                        consoleRenderer.Buffer = Buffer;
					rednderer.UpdateFrame();
				}
            }
        }

        public void UpdateFrameFull()
        {
			RenderToCache();
            UpdateFrame();
        }

        public void RenderToCache()
        {
            for (int i = 0; i < SizeI; i++)
            {
                for (int j = 0; j < SizeJ; j++)
                {
					_cache[j, i] = EMPTY_CHAR;
                }
            }
        }
    }
}
