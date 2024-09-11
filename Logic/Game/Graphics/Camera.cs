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
    public class Camera : ScalingViewportAdapter
    {
        private OrthographicCamera camera;
        private ScalingViewportAdapter viewportAdapter;
        private KeyboardState keyState;
        private KeyboardState lastKeyState;
        private Matrix scaleMatrix;
        private Vector2 playerPosition;
        private bool resolutionChanging;
        private int virtualWidth;
        private int virtualHeight;
        private int newWidth;
        private int newHeight;
        private int screenX;
        private int screenY;

        public Camera(GameWindow Window, GraphicsDevice GraphicsDevice, int virtualWidth, int virtualHeight)
            : base(GraphicsDevice, virtualWidth, virtualHeight)
        {
            this.virtualWidth = virtualWidth;
            this.virtualHeight = virtualHeight;
            screenX = virtualWidth / 2;
            screenY = virtualHeight / 2;

            playerPosition = new Vector2(screenX, screenY);

            // Instantiate the camera
            viewportAdapter = new ScalingViewportAdapter(GraphicsDevice, virtualWidth, virtualHeight);
            
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

        public void UpdateGraphicsDevice(int width, int height)
        {
            newWidth = width;
            newHeight = height;
        }

        void OnClientSizeChanged(object sender, EventArgs e)
        {
            resolutionChanging = !resolutionChanging;

            if (resolutionChanging)
            {
                viewportAdapter = new ScalingViewportAdapter(GraphicsDevice, virtualWidth, virtualHeight);

                camera = new OrthographicCamera(viewportAdapter);

                camera.LookAt(playerPosition);

                System.Diagnostics.Debug.WriteLine("Camera centre: " + camera.Center);
                System.Diagnostics.Debug.WriteLine("Player centre: " + playerPosition);
                System.Diagnostics.Debug.WriteLine("Viewport centre: " + Viewport.X + ", " + Viewport.Y);


                resolutionChanging = false;
            }
        }

        /*
         * 
         */
        public OrthographicCamera GetCamera()
        {
            return camera;
        }
    }
}

/*
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

            Window.ClientSizeChanged += new EventHandler<EventArgs>(OnClientSizeChanged);
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
         *//*
        public void SetXY(int x, int y)
        {
            screenX = x;
            screenY = y;
        }

        /*
         * 
         *//*
        public void CentreCamera()
        {
            camera.LookAt(position)
        }

        void OnClientSizeChanged(object sender, EventArgs e)
        {

        }
    }
}*/