using Microsoft.Xna.Framework;

namespace SoR.Logic.Entities
{
    internal class Player : Entity
    {
        private Vector2 position;
        private float positionX;
        private float positionY;
        private float speed;

        public Player(GraphicsDeviceManager _graphics)
        {
            // Set the current position on the screen
            position = new Vector2(_graphics.PreferredBackBufferWidth / 2,
                _graphics.PreferredBackBufferHeight / 2);

            positionX = position.X; // Set the x-axis position
            positionY = position.Y; // Set the y-axis position

            speed = 200f; // Set the entity's travel speed
        }

        /*
         * Get the Atlas path.
         */
        public string GetAtlas()
        {
            return "F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Player\\Char sprites.atlas";
        }

        /*
         * Get the json path.
         */
        public string GetJson()
        {
            return "F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Player\\skeleton.json";
        }

        /*
         * Get the starting skin.
         */
        public string GetSkin()
        {
            return "solarknight-0";
        }

        /*
         * Get the starting animation.
         */
        public string GetStartingAnim()
        {
            return "idlebattle";
        }

        /*
         * Get the current travel speed.
         */
        public float GetSpeed()
        {
            return speed;
        }

        /*
         * Get the current x-axis position.
         */
        public float GetPositionX()
        {

            return positionX;
        }

        /*
         * Get the current y-axis position.
         */
        public float GetPositionY()
        {
            return positionY;
        }

        /*
         * Set the current x-axis position.
         */
        public void SetPositionX(float newPositionX)
        {
            positionX = newPositionX;
            position.X = positionX;
        }

        /*
         * Set the current y-axis position.
         */
        public void SetPositionY(float newPositionY)
        {
            positionY = newPositionY;
            position.Y = positionY;
        }
    }
}