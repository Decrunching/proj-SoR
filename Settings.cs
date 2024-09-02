using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SoR
{
    public class Settings
    {
        private bool fullscreen;
        private bool borderless;
        private int screenWidth;
        private int screenHeight;
        private MainGame game;

        public Settings(MainGame game, GameWindow window)
        {
            this.game = game;
        }

        /*
         * 
         */
        public void ToggleBorderless(GraphicsDeviceManager graphics, GameWindow window)
        {
            if (borderless)
            {
                borderless = false;
            }
            else
            {
                borderless = true;
            }

            ApplyBorderlessChange(game, graphics, window, borderless);
        }

        /*
         * 
         */
        public void ApplyBorderlessChange(MainGame game, GraphicsDeviceManager graphics, GameWindow window, bool borderlessLast)
        {
            if (borderless)
            {
                if (borderlessLast)
                {
                    ApplyHardwareMode(graphics);
                }
                else
                {
                    SetBorderless(graphics, window);
                }
            }
            else
            {
                SetWindowed(graphics);
            }
        }

        /*
         * 
         */
        public void ApplyHardwareMode(GraphicsDeviceManager graphics)
        {
            graphics.HardwareModeSwitch = !borderless;
            graphics.ApplyChanges();
        }

        /*
         * 
         */
        public void SetBorderless(GraphicsDeviceManager graphics, GameWindow window)
        {
            screenWidth = window.ClientBounds.Width;
            screenHeight = window.ClientBounds.Height;

            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.HardwareModeSwitch = !borderless;

            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
        }

        /*
         * 
         */
        public void SetWindowed(GraphicsDeviceManager graphics)
        {
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
        }
    }
}
