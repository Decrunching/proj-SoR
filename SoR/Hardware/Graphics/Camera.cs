using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System;

namespace SoR.Hardware.Graphics
{
    /*
     * The game camera, which follows the player.
     */
    public class Camera : BoxingViewportAdapter
    {
        private OrthographicCamera camera;
        private BoxingViewportAdapter viewportAdapter;
        private int virtualWidth;
        private int virtualHeight;
        private int screenX;
        private int screenY;
        public int NewWidth { get; set; }
        public int NewHeight { get; set; }
        public Vector2 PlayerPosition { get; set; }

        public Camera(GameWindow Window, GraphicsDevice GraphicsDevice, int virtualWidth, int virtualHeight)
            : base(Window, GraphicsDevice, virtualWidth, virtualHeight)
        {
            this.virtualWidth = virtualWidth;
            this.virtualHeight = virtualHeight;
            screenX = virtualWidth / 2;
            screenY = virtualHeight / 2;

            PlayerPosition = new Vector2(screenX, screenY);

            // Instantiate the camera
            viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, virtualWidth, virtualHeight);

            camera = new OrthographicCamera(viewportAdapter);
            camera.Zoom = 1f;

            Window.ClientSizeChanged += OnClientSizeChanged;
        }

        /*
         * Follows the player.
         */
        public void FollowPlayer(Vector2 position)
        {
            PlayerPosition = position;

            camera.LookAt(PlayerPosition);
        }

        /*
         * Update the viewport adapter and camera after a resolution change.
         */
        public void UpdateViewportAdapter(GameWindow Window)
        {
            viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, virtualWidth, virtualHeight);
            camera = new OrthographicCamera(viewportAdapter);
        }

        /*
         * Get the new screen size after a resolution change;
         */
        public void GetResolutionUpdate(int width, int height)
        {
            NewWidth = width;
            NewHeight = height;
        }

        /*
         * Trigger if the window resolution changes to update the graphics device, camera and viewport.
         */
        public void OnClientSizeChanged(object sender, EventArgs e)
        {
            camera.LookAt(PlayerPosition);
        }

        /*
         * Get the Camera.
         */
        public OrthographicCamera GetCamera()
        {
            return camera;
        }
    }
}