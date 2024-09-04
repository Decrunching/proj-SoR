using Logic.Entities.Character.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Logic.Game
{
    public class Camera
    {
        public Matrix Transform { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Vector2 { get; set; }
        private Viewport viewport;

        public Camera(Viewport viewport)
        {
            this.viewport = viewport;
            Position = Vector2.Zero;
        }

        /*
         * Centre the camera over the player and create the transformation matrix for moving the
         * camera as the player moves.
         */
        public void UpdateCamera(Player player)
        {
            Position = player.GetPosition() - new Vector2(viewport.Width / 2, viewport.Height / 2);

            Transform = Matrix.CreateTranslation(new Vector3(-Position, 0));
        }

        public float GetCameraPositionX()
        {
            return Position.X;
        }

        public float GetCameraPositionY()
        {
            return Position.Y;
        }
    }
}
