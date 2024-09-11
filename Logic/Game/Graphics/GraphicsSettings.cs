using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoR;

namespace Logic.Game.Graphics
{
    /*
     * Switch between windowed and borderless.
     */
    internal class GraphicsSettingsTemp
    {
        private float x;
        private float y;
        private bool isFullscreen;
        private bool isBorderless;
        private bool resolutionChange;
        private int screenWidth;
        private int screenHeight;
        private GameWindow Window;
        private KeyboardState keyState;
        private KeyboardState lastKeyState;

        public GraphicsSettingsTemp(MainGame game, GraphicsDeviceManager graphics)
        {
            screenWidth = graphics.PreferredBackBufferWidth = 800;
            screenHeight = graphics.PreferredBackBufferHeight = 600;
            x = graphics.PreferredBackBufferWidth / 2;
            y = graphics.PreferredBackBufferHeight / 2;

            graphics.IsFullScreen = isFullscreen = false;
            isBorderless = false;
            Window.AllowAltF4 = true;
            Window.AllowUserResizing = false;
            game.IsMouseVisible = false;

            RestoreWindow(graphics);
        }

        /*
         * Toggle between borderless and windowed when F4 is pressed.
         */
        public void ToggleBorderlessWindowed(GraphicsDeviceManager graphics)
        {
            keyState = Keyboard.GetState(); // Get the current keyboard state

            if (keyState.IsKeyDown(Keys.F4) & !lastKeyState.IsKeyDown(Keys.F4))
            {
                ToggleBorderlessMode(graphics, Window);
            }

            lastKeyState = keyState; // Get the previous keyboard state
        }

        /*
         * (NOT ENABLED) Toggle fullscreen.
         */
        public void ToggleFullscreenMode(GraphicsDeviceManager graphics, GameWindow Window)
        {
            bool fullscreenLast = isFullscreen;

            if (isBorderless)
            {
                isBorderless = false;
            }
            else
            {
                isFullscreen = !isFullscreen;
            }

            ChangeFullscreen(graphics, Window, fullscreenLast);
        }

        /*
         * Toggle borderless state and set fullscreen state to the same.
         */
        public void ToggleBorderlessMode(GraphicsDeviceManager graphics, GameWindow Window)
        {
            bool fullscreenLast = isFullscreen;

            isBorderless = !isBorderless;
            isFullscreen = isBorderless;

            ChangeFullscreen(graphics, Window, fullscreenLast);
        }

        /*
         * Set or remove fullscreen mode.
         */
        public void ChangeFullscreen(GraphicsDeviceManager graphics, GameWindow Window, bool fullscreenLast)
        {
            if (isFullscreen)
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
                RemoveFullscreen(graphics, Window);
            }
        }

        /*
         * Called when already in fullscreen. When hardware mode switch set to false, will be borderless.
         */
        public void ApplyHardwareMode(GraphicsDeviceManager graphics)
        {
            graphics.HardwareModeSwitch = !isBorderless;
            graphics.ApplyChanges();
        }

        /*
         * Set screen width and height to the size of the monitor, then switch to fullscreen mode.
         */
        public void SetFullscreen(GraphicsDeviceManager graphics, GameWindow Window)
        {
            SaveWindow(Window);

            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            graphics.HardwareModeSwitch = !isBorderless;

            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
        }

        /*
         * Restore window dimensions to their previous state, and remove fullscreen mode.
         */
        public void RemoveFullscreen(GraphicsDeviceManager graphics, GameWindow Window)
        {
            graphics.IsFullScreen = false;
            RestoreWindow(graphics);
        }

        /*
         * Save window settings.
         */
        public void SaveWindow(GameWindow Window)
        {
            x = Window.ClientBounds.X;
            y = Window.ClientBounds.Y;
            screenWidth = Window.ClientBounds.Width;
            screenHeight = Window.ClientBounds.Height;
        }

        /*
         * Restore the window to defaults.
         */
        public void RestoreWindow(GraphicsDeviceManager graphics)
        {
            Window.Position = new Point((int)x, (int)y);
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.ApplyChanges();
        }
    }
}