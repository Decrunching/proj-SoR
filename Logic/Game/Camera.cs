using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace Logic.Game
{
    /*
     * The game camera, which follows the player.
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
        public void FollowPlayer(Vector2 position, int screenWidth, int screenHeight)
        {
            camera.Move(camera.WorldToScreen(position.X - (screenWidth / 2), position.Y - (screenHeight / 2)));
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