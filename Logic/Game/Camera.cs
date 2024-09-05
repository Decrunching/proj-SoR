using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace Logic.Game
{/*
  * Positions the camera to follow the player.
  */
    public class Camera
    {
        private OrthographicCamera camera;

        public Camera(GraphicsDevice GraphicsDevice, GameWindow Window)
        {
            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 800, 480);
            camera = new OrthographicCamera(viewportAdapter);
        }

        public Matrix RenderCamera()
        {
            Matrix transformMatrix = camera.GetViewMatrix();
            return transformMatrix;
        }



        /*public Matrix Transform { get; private set; }
        private Viewport viewport;

        /*
         * Centre the camera over the player, clamp it to the stage bounds, and create the transformation
         * matrix for moving it as the player moves.
         */
        /*public void Follow(Player player, int screenWidth, int screenHeight)
        {
            var position = Matrix.CreateTranslation(
                -player.GetPosition().X - (player.GetHitbox().Width / 2),
                -player.GetPosition().Y - (player.GetHitbox().Height / 2),
                0);

            var offset = Matrix.CreateTranslation(
                    screenWidth / 2,
                    screenHeight / 2,
                    0);

            Transform = position * offset;

            /*Position = Vector2.Clamp(
                Position, Vector2.Zero, new Vector2(
                    stageWidth - viewport.Width, stageHeight - viewport.Height));
        }*/
    }
}