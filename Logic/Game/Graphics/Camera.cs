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

        public Camera(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice, GameWindow Window, int virtualWidth, int virtualHeight)
        {
            _virtualWidth = virtualWidth;
            _virtualHeight = virtualHeight;
            
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

            camera.Move(camera.WorldToScreen(position.X - _virtualWidth / 2, position.Y - _virtualHeight / 2));
        }

        /*
         * Get the camera.
         */
        public OrthographicCamera GetCamera()
        {
            return camera;
        }

        /*
         * 
         */
        public void ScaleViewport(GraphicsDeviceManager graphics)
        {
            _virtualWidth = graphics.PreferredBackBufferWidth / 800;
            _virtualHeight = graphics.PreferredBackBufferHeight / 600;


        }
    }
}