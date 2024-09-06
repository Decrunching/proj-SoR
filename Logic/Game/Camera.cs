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
        private BoxingViewportAdapter viewportAdapter;
        private Matrix viewMatrix;
        private int screenWidth;
        private int screenHeight;

        public Camera(GameWindow Window, GraphicsDevice GraphicsDevice, GraphicsSettings graphicsSettings)
        {
            viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, graphicsSettings.Width, graphicsSettings.Height);

            camera = new OrthographicCamera(viewportAdapter);

            viewMatrix = camera.GetViewMatrix();

            screenWidth = graphicsSettings.Width;
            screenHeight = graphicsSettings.Height;
        }

        /*
         * Follows the player.
         */
        public void FollowPlayer(GameWindow Window,
            GraphicsDevice GraphicsDevice,
            GraphicsSettings graphicsSettings,
            Vector2 position)
        {
            if (graphicsSettings.IsWindowResized())
            {
                if (graphicsSettings.IsBorderless)
                {
                    viewportAdapter = new BoxingViewportAdapter(
                        Window,
                        GraphicsDevice,
                        GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width,
                        GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);

                    camera = new OrthographicCamera(viewportAdapter);

                    viewMatrix = camera.GetViewMatrix();

                    screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                    screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

                    graphicsSettings.WindowResized();
                }
                else
                {
                    viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, graphicsSettings.Width, graphicsSettings.Height);

                    camera = new OrthographicCamera(viewportAdapter);

                    viewMatrix = camera.GetViewMatrix();

                    screenWidth = graphicsSettings.Width;
                    screenHeight = graphicsSettings.Height;

                    graphicsSettings.WindowResized();
                }
            }

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