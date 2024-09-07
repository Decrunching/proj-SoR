using Logic.Game.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private Matrix viewMatrix;

        public Camera(GraphicsDevice GraphicsDevice, GameWindow Window, int screenWidth, int screenHeight)
        {
            // Instantiate the camera
            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, screenWidth, screenHeight);
            camera = new OrthographicCamera(viewportAdapter);
            viewMatrix = camera.GetViewMatrix();
        }

        /*
         * Follows the player.
         * 
         * TO DO:
         * Fix issue where player moves off the bounds of the stage edge and springs back again.
         */
        public void FollowPlayer(GraphicsDeviceManager graphics, GraphicsSettings graphicsSettings, Vector2 position, int screenWidth, int screenHeight)
        {
            if (graphicsSettings.IsBorderless)
            {
                camera.Move(camera.WorldToScreen(position.X - (
                    GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2),
                    position.Y - (
                    GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2)));
            }
            else
            {
                camera.Move(camera.WorldToScreen(position.X - (screenWidth / 2), position.Y - (screenHeight / 2)));
            }
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