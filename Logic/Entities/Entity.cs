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
        protected Attachment hitboxAttachment;
        protected SkeletonBounds hitbox;
        protected Slot slot;
        protected Vector2 position;
        protected float positionX;
        protected float positionY;
        protected int hitpoints;
        public float Speed { get; set; }
        public string Name { get; set; }
        public bool Render { get; set; }

        /*
         * Check for collision with other entities.
         */
        public abstract bool CollidesWith(Entity entity);

        /*
         * Update the hitbox after a collision.
         */
        public abstract void UpdateHitbox(SkeletonBounds updatedHitbox);

        /*
         * Get the animation state.
         */
        public abstract AnimationState GetAnimState();

        /*
         * Get the skeleton.
         */
        public abstract Skeleton GetSkeleton();

        /*
         * Get the hitbox.
         */
        public abstract SkeletonBounds GetHitbox();

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
         * Get the current x-axis position.
         */
        public abstract float GetPositionX();

        /*
         * Get the current y-axis position.
         */
        public abstract float GetPositionY();
    }
}