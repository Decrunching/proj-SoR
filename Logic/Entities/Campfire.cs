using Microsoft.Xna.Framework;

namespace SoR.Logic.Entities
{
    /*
     * Stores information unique to the campfire entity.
     */
    internal class Campfire : Entity
    {
        public Campfire(GraphicsDeviceManager _graphics)
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
            return "F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Player\\Char sprites.atlas";
            //return "D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Campfire\\templecampfire.atlas";
        }

        /*
         * Get the json path.
         */
        public override string GetJson()
        {
            return "F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Player\\skeleton.json";
            //return "D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Campfire\\skeleton.json";
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