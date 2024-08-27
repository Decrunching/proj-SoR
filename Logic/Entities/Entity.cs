using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spine;

namespace SoR.Logic.Entities
{
    /*
     * All other entities are currently based on this class to reduce code repetition.
     */
    public abstract class Entity
    {
        protected Atlas atlas;
        protected AtlasAttachmentLoader atlasAttachmentLoader;
        protected SkeletonJson json;
        protected SkeletonData skeletonData;
        protected SkeletonRenderer skeletonRenderer;
        protected Skeleton skeleton;
        protected AnimationStateData animStateData;
        protected AnimationState animState;
        protected SkeletonBounds hitbox;
        protected Vector2 position;
        protected float positionX;
        public float PositionY {  get; set; }
        public float Speed { get; set; }
        public string Name { get; set; }

        /*
         * Get the animation state.
         */
        public abstract AnimationState GetAnimState();

        /*
         * Get the skeleton.
         */
        public abstract Skeleton GetSkeleton();

        /*
         * Update entity position according to player input.
         
        public abstract void UpdateEntityPosition(
            GameTime gameTime,
            KeyboardState keyState,
            KeyboardState lastKeyState,
            GraphicsDeviceManager graphics,
            GraphicsDevice GraphicsDevice,
            AnimationState animState,
            Skeleton skeleton);*/

        /*
         * Update the entity position, animation state and skeleton.
         */
        public abstract void UpdateEntityAnimations(GameTime gameTime);

        /*
         * Render the current skeleton to the screen.
         */
        public abstract void RenderSkeleton(GraphicsDevice GraphicsDevice);

        /* 
         * Get the centre of the screen.
         */
        public abstract void GetScreenCentre(Vector2 centreScreen);

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
            return PositionY;
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
            PositionY = newPositionY;
        }
    }
}