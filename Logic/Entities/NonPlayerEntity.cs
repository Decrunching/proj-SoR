using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SoR.Logic.Entities
{
    /*
     * All other entities are currently based on this class to reduce code repetition.
     */
    public abstract class NonPlayerEntity
    {
        protected Vector2 position;
        protected float positionX;
        protected float positionY;
        public float Speed { get; set; }

        /*
         * Get the atlas path.
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
         * Create the SkeletonRenderer.
         */
        public abstract void CreateSkeletonRenderer(GraphicsDevice GraphicsDevice);

        /*
         * Render the current skeleton to the screen.
         */
        public abstract void RenderSkeleton(GraphicsDevice GraphicsDevice);

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
        }

        /*
         * Set the current y-axis position.
         */
        public void SetPositionY(float newPositionY)
        {
            positionY = newPositionY;
        }
    }
}