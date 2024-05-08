using System.Diagnostics;
using IronEngine;
using IronEngine.IO;
using static IronRenderer.ConsoleRenderer;

namespace IronRenderer
{
    internal class TileMapRenderer : IConsoleRenderer
    {
        private FrameBuffer _mapCache;
        private FrameBuffer _buffer;

        public FrameBuffer Buffer { get; set; }
        private TileMap TileMap { get; set; }
        public int SizeJ => TileMap.SizeX * TileRenderer.SIZE_X;
        public int SizeI => TileMap.SizeY * TileRenderer.SIZE_Y;
        public (int, int) Size => (SizeJ, SizeI);
        public (int, int) CacheSize => (_mapCache.SizeJ, _mapCache.SizeI);

        public TileMapRenderer(TileMap tileMap)
        {
            TileMap = tileMap;
            UpdateCacheSize();
            RenderToLocalCache();
        }

        public void UpdateFrame()
        {
            FrameBuffer.Copy(_buffer, _mapCache);
        }

        public void UpdateFrameFull()
        {
            RenderToLocalCache();
            UpdateFrame();
        }

        public static (int, int) TileMapPositionToFrameBufferPosition(Position position) => (position.x * TileRenderer.SIZE_X, position.y * TileRenderer.SIZE_Y);

        private void RenderToLocalCache()
        {
            ValidateCacheSize();
            RenderTileDataToCache();
        }

        private void RenderTileDataToCache()
        {
            foreach (Tile tile in TileMap)
            {
                if (tile != null && tile is IRenderAble rebderable)
                {
                    var renderer = rebderable.GetRenderer();
                    if (renderer is IConsoleRenderer consoleRenderer)
                    {
                        consoleRenderer.Buffer = new(other: _mapCache,
                            offsetJ: tile.Position.x * TileRenderer.SIZE_X,
                            offsetI: tile.Position.y * TileRenderer.SIZE_Y
                            );
					}
					renderer.UpdateFrame();
				}
            }
        }

        private bool ValidateCacheSize()
        {
            bool valid = CacheSize == Size;

            if (!valid)
                UpdateCacheSize();

            return valid;
        }

        private void UpdateCacheSize()
        {
            _mapCache = new FrameBuffer(SizeJ, SizeI);
        }
    }
}
