using Microsoft.Xna.Framework;

namespace SoR.Logic.Entities
{
    /*
     * All other entities are currently based on this class to reduce code repetition.
     */
    internal abstract class Entity
    {
        public Vector2 Position {  get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float Speed { get; set; }

        /*
         * Get the Atlas path.
         */
        public abstract string GetAtlas();

        /*
         * Get the json path.
         */
        public abstract string GetJson();

        /*
         * Get the starting skin.
         */
        public abstract string GetSkin();

        /*
         * Get the starting animation.
         */
        public abstract string GetStartingAnim();

        /*
         * Get the current travel speed.
         */
        public float GetSpeed()
        {
            return Speed;
        }

        /*
         * Get the current x-axis position.
         */
        public float GetPositionX()
        {

            return PositionX;
        }

        /*
         * Get the current y-axis position.
         */
        public float GetPositionY()
        {
            return PositionY;
        }

        /*
         * Set the current x-axis position.
         */
        public void SetPositionX(float newPositionX)
        {
            PositionX = newPositionX;
        }

        /*
         * Set the current y-axis position.
         */
        public void SetPositionY(float newPositionY)
        {
            PositionY = newPositionY;
        }
    }
}