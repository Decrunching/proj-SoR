using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System;

namespace Logic.Game.Graphics
{
    /*
     * The game camera, which follows the player.
     */
    public class Camera : BoxingViewportAdapter
    {
        private OrthographicCamera camera;
        private BoxingViewportAdapter viewportAdapter;
        private Vector2 playerPosition;
        private bool resolutionChanging;
        private int virtualWidth;
        private int virtualHeight;
        private int newWidth;
        private int newHeight;
        private int screenX;
        private int screenY;

        public Camera(GameWindow Window, GraphicsDevice GraphicsDevice, int virtualWidth, int virtualHeight)
            : base(Window, GraphicsDevice, virtualWidth, virtualHeight)
        {
            this.virtualWidth = virtualWidth;
            this.virtualHeight = virtualHeight;
            screenX = virtualWidth / 2;
            screenY = virtualHeight / 2;

            playerPosition = new Vector2(screenX, screenY);

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
            playerPosition = position;
            
            camera.LookAt(playerPosition);
            //camera.Move(camera.WorldToScreen(position.X - screenX, position.Y - screenY));
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
            newWidth = width;
            newHeight = height;
        }

        /*
         * Trigger if the window resolution changes to update the graphics device, camera and viewport.
         */
        void OnClientSizeChanged(object sender, EventArgs e)
        {
            if (resolutionChanging)
            {
                camera.LookAt(playerPosition);

                resolutionChanging = false;
            }
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