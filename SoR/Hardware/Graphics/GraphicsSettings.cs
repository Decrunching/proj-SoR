﻿using SoR.Hardware.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SoR.Hardware.Graphics
{
    /*
     * Switch between windowed and borderless.
     */
    public class GraphicsSettings
    {
        private float x;
        private float y;
        private bool isFullscreen;
        private bool isBorderless;
        private int screenWidth;
        private int screenHeight;
        private GamePadInput gamePadInput;
        private KeyboardInput keyboardInput;
        private Vector2 resolution;

        public GraphicsSettings(MainGame game, GraphicsDeviceManager graphics, GameWindow Window)
        {
            screenWidth = 800;
            screenHeight = 600;
            resolution = new Vector2(screenWidth, screenHeight);
            x = 400;
            y = 300;

            graphics.IsFullScreen = isFullscreen = false;
            isBorderless = false;
            Window.AllowAltF4 = true;
            Window.AllowUserResizing = false;
            game.IsMouseVisible = false;

            gamePadInput = new GamePadInput();
            keyboardInput = new KeyboardInput();

            RestoreWindow(graphics, Window);
        }

        /*
         * Toggle between borderless and windowed when F4 is pressed.
         */
        public Vector2 CheckIfBorderlessToggled(GraphicsDeviceManager graphics, GameWindow Window)
        {
            if (gamePadInput.CheckButtonInput() == "Back" || keyboardInput.CheckKeyInput() == "F4")
            {
                ToggleBorderlessMode(graphics, Window);
                resolution = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            }

            return resolution;
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
            RestoreWindow(graphics, Window);
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
        public void RestoreWindow(GraphicsDeviceManager graphics, GameWindow Window)
        {
            Window.Position = new Point((int)x, (int)y);
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.ApplyChanges();
        }
    }
}