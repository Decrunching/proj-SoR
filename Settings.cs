using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SoR
{
    /*
     * Manage graphics settings.
     */
    public class Settings
    {
        private bool fullscreen;
        private bool borderless;
        private int screenWidth;
        private int screenHeight;
        private KeyboardState keyState;

        public Settings(GraphicsDeviceManager graphics, GameWindow Window)
        {
            fullscreen = false;
            borderless = false;
            screenWidth = 0;
            screenHeight = 0;
        }

        /*
         * 
         */
        public void ChooseScreenMode(GraphicsDeviceManager graphics, GameWindow Window)
        {
            keyState = Keyboard.GetState(); // Get the current keyboard state

            if (keyState.IsKeyDown(Keys.F4))
            {
                if (!graphics.IsFullScreen)
                {
                    ToggleFullscreen(graphics, Window);
                }
                if (graphics.IsFullScreen)
                {
                    ToggleBorderless(graphics, Window);
                }
            }
        }

        /*
         * 
         */
        public void ToggleFullscreen(GraphicsDeviceManager graphics, GameWindow Window)
        {
            bool fullscreenLast = fullscreen;

            if (borderless)
            {
                borderless = false;
            }
            else
            {
                fullscreen = !fullscreen;
            }

            ChangeFullscreen(graphics, Window, fullscreenLast);
        }

        /*
         * 
         */
        public void ToggleBorderless(GraphicsDeviceManager graphics, GameWindow Window)
        {
            bool fullscreenLast = fullscreen;

            borderless = !borderless;
            fullscreen = borderless;

            ChangeFullscreen(graphics, Window, fullscreenLast);
        }

        /*
         * 
         */
        public void ChangeFullscreen(GraphicsDeviceManager graphics, GameWindow Window, bool fullscreenLast)
        {
            if (fullscreen)
            {
                if (fullscreenLast)
                {
                    ApplyHardwareMode(graphics);
                }
                else
                {
                    SetFullscreen(graphics, Window);
                }
            }
            else
            {
                RemoveFullscreen(graphics);
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
        public void SetFullscreen(GraphicsDeviceManager graphics, GameWindow Window)
        {
            screenWidth = Window.ClientBounds.Width;
            screenHeight = Window.ClientBounds.Height;

            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.HardwareModeSwitch = !borderless;

            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
        }

        /*
         * 
         */
        public void RemoveFullscreen(GraphicsDeviceManager graphics)
        {
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
        }
    }
}
