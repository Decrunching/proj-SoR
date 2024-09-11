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
    public class Camera
    {
        private OrthographicCamera camera;
        private BoxingViewportAdapter viewportAdapter;
        private KeyboardState keyState;
        private KeyboardState lastKeyState;
        private Matrix scaleMatrix;
        private int virtualWidth;
        private int virtualHeight;
        private int screenX;
        private int screenY;

        public Camera(GraphicsDevice GraphicsDevice, GameWindow Window)
        {
            virtualWidth = 800;
            virtualHeight = 600;
            screenX = virtualWidth / 2;
            screenY = virtualHeight / 2;

            // Instantiate the camera
            viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, virtualWidth, virtualHeight);
            
            camera = new OrthographicCamera(viewportAdapter);

            Window.ClientSizeChanged += new EventHandler<EventArgs>(OnClientSizeChanged);
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
         * Reset the viewport after a resolution change.
         */
        public void ResetViewport(GraphicsDevice GraphicsDevice, GameWindow Window)
        {
            viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, virtualWidth, virtualHeight);
            //camera = new OrthographicCamera(viewportAdapter);
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

        /*
         * 
         */
        public void CentreCamera()
        {
            camera.LookAt(position)
        }

        void OnClientSizeChanged(object sender, EventArgs e)
        {

        }
    }
}

/*
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace Logic.Game.Graphics
{
    /*
     * The game camera, which follows the player.
     *//*
    public class Camera
    {
        private OrthographicCamera camera;
        private BoxingViewportAdapter viewportAdapter;
        private KeyboardState keyState;
        private KeyboardState lastKeyState;
        private Matrix scaleMatrix;
        private int virtualWidth;
        private int virtualHeight;
        private int screenX;
        private int screenY;

        public Camera(GraphicsDevice GraphicsDevice, GameWindow Window)
        {
            virtualWidth = 800;
            virtualHeight = 600;
            screenX = virtualWidth / 2;
            screenY = virtualHeight / 2;

            // Instantiate the camera
            viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, virtualWidth, virtualHeight);

            camera = new OrthographicCamera(viewportAdapter);
        }

        /*
         * Follows the player.
         *//*
        public void FollowPlayer(Vector2 position)
        {
            //Resolution.currentResolution().X != Window.ClientBounds.Width || Resolution.currentResolution().Y != Window.ClientBounds.Height;

            camera.Move(camera.WorldToScreen(position.X - screenX, position.Y - screenY));
        }

        /*
         * Reset the viewport after a resolution change.
         *//*
        public void ResetViewport(GraphicsDevice GraphicsDevice, GameWindow Window)
        {
            viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, virtualWidth, virtualHeight);
            //camera = new OrthographicCamera(viewportAdapter);
        }

        /*
         * Get the camera.
         *//*
        public OrthographicCamera GetCamera()
        {
            return camera;
        }

        /*
         * Set the x,y coordinates for centring the camera.
         */
        /*public void SetXY(int x, int y)
        {
            screenX = x;
            screenY = y;
        }
    }
}*/