using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace Logic.Game
{
    /*
     * The game camera, which follows the player.
     * 
     * TO DO:
     * Fix bug where camera doesn't follow the player in borderless or fullscreen unless game starts in borderless
     * or fullscreen, but still stops working in these modes after switching to windowed and back despite continuing
     * to work fine in windowed.
     */
    public class Camera
    {
        private OrthographicCamera camera;
        private BoxingViewportAdapter viewportAdapter;
        private Matrix viewMatrix;
        private KeyboardState keyState;
        private KeyboardState lastKeyState;
        private bool resolutionChange;
        private int screenWidth;
        private int screenHeight;

        public Camera(GraphicsDevice GraphicsDevice, GameWindow Window, int screenWidth, int screenHeight)
        {
            // Instantiate the camera
            viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, screenWidth, screenHeight);
            camera = new OrthographicCamera(viewportAdapter);
            viewMatrix = camera.GetViewMatrix();
            resolutionChange = false;
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
        }

        /*
         * Follows the player.
         * 
         * TO DO:
         * Fix issue where player moves off the bounds of the stage edge and springs back again.
         */
        public void FollowPlayer(
            GraphicsDevice GraphicsDevice, 
            GameWindow Window, 
            GraphicsDeviceManager graphics, 
            Vector2 position)
        {
            keyState = Keyboard.GetState(); // Get the current keyboard state

            // Check whether F4 was pressed and borderless toggled (TO DO: change to check this directly via ToggleBorderless later)
            if (keyState.IsKeyDown(Keys.F4) & !lastKeyState.IsKeyDown(Keys.F4))
            {
                // Get the new screen width and height
                screenWidth = graphics.PreferredBackBufferWidth;
                screenHeight = graphics.PreferredBackBufferHeight;

                // Reset the viewport adapter and camera
                viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, screenWidth, screenHeight);
                camera = new OrthographicCamera(viewportAdapter);
            }
            else if (!keyState.IsKeyDown(Keys.F4) & lastKeyState.IsKeyDown(Keys.F4))
            {
                // Get the new screen width and height
                screenWidth = graphics.PreferredBackBufferWidth;
                screenHeight = graphics.PreferredBackBufferHeight;
            }

            camera.Move(camera.WorldToScreen(position.X - (screenWidth / 2), position.Y - (screenHeight / 2)));

            lastKeyState = keyState;
        }

        /*
         * Get the camera.
         */
        public OrthographicCamera GetCamera()
        {
            return camera;
        }
    }
}