using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System.Drawing;

namespace Logic.Game.Graphics
{
    /*
     * The game camera, which follows the player.
     */
    public class Camera
    {
        private OrthographicCamera camera;
        private ScalingViewportAdapter viewportAdapter;
        private Matrix createMatrix;
        private Matrix projectionMatrix;
        private Matrix worldMatrix;
        private Vector2 playerPosition;
        private float scale;
        private int screenX;
        private int screenY;

        public Camera(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice)
        {
            scale = 1f;
            screenX = graphics.GraphicsDevice.Viewport.Width;
            screenY = graphics.GraphicsDevice.Viewport.Height;

            // Instantiate the camera
            viewportAdapter = new ScalingViewportAdapter(GraphicsDevice, 800, 600);


            camera = new OrthographicCamera(viewportAdapter);
        }

        /*
         * Create the translation and scale matrix.
         */
        public void CreateViewportMatrix(float scale, Vector2 position, GraphicsDeviceManager graphics)
        {
            Matrix transformMatrix = Matrix.CreateTranslation(-position.X, -position.Y, 0f);
            transformMatrix *= Matrix.CreateScale(scale, scale, 1f);
            transformMatrix *= Matrix.CreateTranslation(screenX, screenY, 0f);

            createMatrix = transformMatrix;

            Matrix viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, 1), new Vector3(0, 0, 0), Vector3.Up);
            Matrix projectionMatrix = CreateProjectionMatrix(graphics);
            worldMatrix = transformMatrix * viewMatrix * projectionMatrix;
        }

        /*
         * Get the viewport matrix.
         */
        public Matrix GetViewportMatrix()
        {
            return worldMatrix;
        }

        /*
         * Get the projection matrix.
         */
        public Matrix CreateProjectionMatrix(GraphicsDeviceManager graphics)
        {
            Matrix.CreateOrthographicOffCenter(0, screenX, screenY, 0, 1, 0, out Matrix projectionMatrix);

            if (graphics.GraphicsDevice.UseHalfPixelOffset)
            {
                projectionMatrix.M41 += -0.5f * projectionMatrix.M11;
                projectionMatrix.M42 += -0.5f * projectionMatrix.M22;
            }

            return projectionMatrix;
        }

        /*
         * Follows the player.
         */
        public void FollowPlayer(Vector2 position)
        {
            playerPosition = position;
            camera.Move(camera.WorldToScreen(playerPosition.X - screenX, playerPosition.Y - screenY));
        }

        /*
         * Reset the scale matrix after changing screen resolution.
         */
        public void UpdateViewportMatrix(float scale, Vector2 position, GraphicsDeviceManager graphics)
        {
            CreateViewportMatrix(scale, position, graphics);

            camera.Move(camera.WorldToScreen(position.X - screenX, position.Y - screenY));
        }
    }
}