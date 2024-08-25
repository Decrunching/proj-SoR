using Microsoft.Xna.Framework;

namespace SoR.Logic.Entities
{
    /*
     * Stores information unique to the chara entity.
     */
    internal class Chara : Entity
    {
        public Chara(GraphicsDeviceManager _graphics)
        {
            // Set the current position on the screen
            position = new Vector2(_graphics.PreferredBackBufferWidth / 2,
                _graphics.PreferredBackBufferHeight / 2);

            positionX = position.X; // Set the x-axis position
            positionY = position.Y; // Set the y-axis position

            Speed = 200f; // Set the entity's travel speed
        }

        /*
         * Get the Atlas path.
         */
        public override string GetAtlas()
        {
            return "F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Chara\\savedit.atlas";
        }

        /*
         * Get the json path.
         */
        public override string GetJson()
        {
            return "F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Chara\\skeleton.json";
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
