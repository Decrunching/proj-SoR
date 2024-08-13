using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SoR
{
    public class Gfx : Game
    {
        private GraphicsDeviceManager _graphics;
        private GraphicsDevice graphicsDevice;
        protected int screenWidth;
        protected int screenHeight;

        public Gfx(Game1 game)
        {
            _graphics = new GraphicsDeviceManager(game);
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            screenWidth = _graphics.PreferredBackBufferWidth;
            screenHeight = _graphics.PreferredBackBufferHeight;
            graphicsDevice = game.GraphicsDevice;
        }

        public GraphicsDeviceManager GetGraphicsDeviceManagerGfx()
        {
            return _graphics;
        }

        public GraphicsDevice GetGraphicsDeviceGfx()
        {
            return graphicsDevice;
        }

        public int GetScreenWidthGfx()
        {
            return screenWidth;
        }

        public int GetScreenHeightGfx()
        {
            return screenHeight;
        }
    }
}
