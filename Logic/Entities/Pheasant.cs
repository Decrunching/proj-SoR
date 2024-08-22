using Microsoft.Xna.Framework;

namespace SoR.Logic.Entities
{
    internal class Pheasant : Entity
    {
        public Pheasant(GraphicsDeviceManager _graphics)
        {
            // Set the current position on the screen
            Position = new Vector2(_graphics.PreferredBackBufferWidth / 2,
                _graphics.PreferredBackBufferHeight / 2);

            PositionX = Position.X; // Set the x-axis position
            PositionY = Position.Y; // Set the y-axis position

            Speed = 200f; // Set the entity's travel speed
        }

        /*
         * Get the Atlas path.
         */
        public override string GetAtlas()
        {
            return "F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Pheasant\\savedthepheasant.atlas";
        }

        /*
         * Get the json path.
         */
        public override string GetJson()
        {
            return "F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Pheasant\\skeleton.json";
        }

        /*
         * Get the starting skin.
         */
        public override string GetSkin()
        {
            return "default";
        }

        /*
         * Get the starting animation.
         */
        public override string GetStartingAnim()
        {
            return "idle";
        }
    }
}