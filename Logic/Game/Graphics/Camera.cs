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
        private bool resolutionChange;
        private int _virtualWidth;
        private int _virtualHeight;

        public Camera(GraphicsDevice GraphicsDevice, GameWindow Window, int virtualWidth, int virtualHeight)
        {
            // Instantiate the camera
            viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, virtualWidth, virtualHeight);

            camera = new OrthographicCamera(viewportAdapter);
            resolutionChange = false;
            _virtualWidth = virtualWidth;
            _virtualHeight = virtualHeight;
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
            GraphicsSettings graphicsSettings,
            Vector2 position)
        {
            // Check whether F4 was pressed and borderless toggled (TO DO: change to check this directly via ToggleBorderless later)
            /*if (graphicsSettings.ResolutionHasChanged())
            {
                // Get the new screen width and height
                _virtualWidth = graphics.PreferredBackBufferWidth;
                _virtualHeight = graphics.PreferredBackBufferHeight;

                /*
                 * Need to upscale everything
                 * so take the new screen width & height, find out how many times the old ones divide
                 * into the new ones, and multiply them
                 */

            // Reset the viewport adapter and camera
            /*viewportAdapter = new BoxingViewportAdapter(GraphicsDevice, _virtualWidth, _virtualHeight);
            camera = new OrthographicCamera(viewportAdapter);

            graphicsSettings.ResolutionChangeFinished();

            // Debugging
            System.Diagnostics.Debug.WriteLine("Screen resolution changed: " + _virtualWidth + ", " + _virtualHeight);
            System.Diagnostics.Debug.WriteLine("Mid screen: " + _virtualWidth / 2 + ", " + _virtualHeight / 2);
            System.Diagnostics.Debug.WriteLine("Camera centre: " + camera.Center);
            System.Diagnostics.Debug.WriteLine("Graphics centre: " + graphics.PreferredBackBufferWidth / 2 + ", " + graphics.PreferredBackBufferHeight / 2);
            System.Diagnostics.Debug.WriteLine("Player position: " + position.X + ", " + position.Y);
        }*/

            // Update the viewport when the resolution changes
            //Window.ClientSizeChanged += (s, e) => viewportAdapter.OnclientSizeChanged

            //var matrix = Matrix.Invert(viewportAdapter.GetScaleMatrix());
            //var pointTransform = Vector2.Transform(position, matrix).ToPoint();

            camera.Move(camera.WorldToScreen(position.X - _virtualWidth / 2, position.Y - _virtualHeight / 2));
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
/*
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.ViewportAdapters;

namespace Logic.Game.Graphics
{
    public enum BoxingMode
    {
        Letterbox,
        Pillarbox
    }

    public class BoxingViewportAdapter : ScalingViewportAdapter
    {
        public BoxingViewportAdapter(GraphicsDevice GraphicsDevice, int virtualWidth, int virtualHeight)
             : base(GraphicsDevice, virtualWidth, virtualHeight) { }

        public BoxingMode BoxingMode { get; private set; }

        public void OnClientSizeChanged()
        {
            var viewport = GraphicsDevice.Viewport;
            var aspectRatio = (float)VirtualWidth / VirtualHeight;
            var width = viewport.Width;
            var height = (int)(width / aspectRatio + 0.5f);

            if (height > viewport.Height)
            {
                BoxingMode = BoxingMode.Pillarbox;
                height = viewport.Height;
                width = (int)(height * aspectRatio + 0.5f);
            }
            else
            {
                BoxingMode = BoxingMode.Letterbox;
            }

            var x = (viewport.Width / 2) - (width / 2);
            var y = (viewport.Height / 2) + (height / 2);
            GraphicsDevice.Viewport = new Viewport(x, y, width, height);
        }
    }
}
*/