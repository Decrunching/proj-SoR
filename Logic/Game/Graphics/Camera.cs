using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace Logic.Game.Graphics
{
    /*
     * The game camera, which follows the player.
     */
    public class Camera
    {
        private OrthographicCamera camera;
        private BoxingViewportAdapter viewportAdapter;
        private KeyboardState keyState;
        private KeyboardState lastKeyState;
        private Matrix scaleMatrix;
        private int _virtualWidth;
        private int _virtualHeight;
        private int screenX;
        private int screenY;

        public Camera(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice, GameWindow Window, int virtualWidth, int virtualHeight)
        {
            _virtualWidth = virtualWidth;
            _virtualHeight = virtualHeight;
            screenX = virtualWidth / 2;
            screenY = virtualHeight / 2;

            // Instantiate the camera
            viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, virtualWidth, virtualHeight);
            
            camera = new OrthographicCamera(viewportAdapter);
        }

        /*
         * Follows the player.
         */
        public void FollowPlayer(Vector2 position)
        {
            //Resolution.currentResolution().X != Window.ClientBounds.Width || Resolution.currentResolution().Y != Window.ClientBounds.Height;

            camera.Move(camera.WorldToScreen(position.X - screenX, position.Y - screenY));
        }

        /*
         * Get the camera.
         */
        public OrthographicCamera GetCamera()
        {
            return camera;
        }

        /*
         * Set the x,y coordinates for centring the camera.
         */
        public void SetXY(int x, int y)
        {
            screenX = x;
            screenY = y;
        }
    }
}