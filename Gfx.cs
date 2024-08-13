using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SoR
{
    internal class Gfx : Game
    {
        private GraphicsDeviceManager _graphics;
        protected int screenWidth;
        protected int screenHeight;
        private Game1 game;
        private Gfx gfx;

        public Gfx(Game1 game)
        {
            this.game = game;
            _graphics = new GraphicsDeviceManager(game);
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            screenWidth = _graphics.PreferredBackBufferWidth;
            screenHeight = _graphics.PreferredBackBufferHeight;
        }

        public GraphicsDeviceManager GetGraphics()
        {
            return _graphics;
        }

        public GraphicsDevice GetGraphicsDevice()
        {
            return GraphicsDevice;
        }
    }
}
