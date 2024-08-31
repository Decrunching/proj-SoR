using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoR.Logic.Input;
using Spine;
using System;

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
        protected Movement movement;
        protected Random random;
        protected Vector2 position;
        protected Vector2 moving;
        protected float prevPositionX;
        protected float prevPositionY;
        protected int hitpoints;
        protected string prevTrigger;
        protected string nextAnim;
        protected float newDirectionTime;
        protected float sinceLastChange;

        public float Speed { get; set; }
        public string Name { get; set; }
        public bool Render { get; set; }

        /*
         * Placeholder function for dealing damage.
         */
        public abstract int Damage(Entity player);

        /*
         * On first collision, play collision animation.
         */
        public abstract void React(string animation);

        /*
         * If something changes to trigger a new animation, apply the animation.
         * If the animation is already applied, do nothing.
         */
        public abstract void ChangeAnimation(string trigger);

        /*
         * No longer in collision.
         */
        public abstract void ResetCollision();

        /*
         * Update entity position.
         */
        public abstract void UpdatePosition(GameTime gameTime, GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice);

        /*
         * Move to new position.
         */
        public abstract void Movement(GameTime gameTime);

        /*
         * Handle entity collision.
         * 
         * TO DO:
         * Player should still be able to move perpendicular to hitbox edge when in collision.
         */
        public abstract void Collision();

        /*
         * Update the entity position, animation state and skeleton.
         */
        public abstract void UpdateEntityAnimations(GameTime gameTime);

        /*
         * Draw text to the screen.
         */
        public abstract void DrawText(SpriteBatch spriteBatch, SpriteFont font);

        /* 
         * Get the centre of the screen.
         */
        public abstract void SetStartPosition(Vector2 centreScreen);

        /*
         * Check for collision with other entities.
         */
        public bool CollidesWith(Entity entity)
        {
            entity.UpdateHitbox(new SkeletonBounds());
            entity.GetHitbox().Update(entity.GetSkeleton(), true);

            hitbox = new SkeletonBounds();
            hitbox.Update(skeleton, true);

            if (hitbox.AabbIntersectsSkeleton(entity.GetHitbox()))
            {
                return true;
            }

            return false;
        }

        /*
         * Render the current skeleton to the screen.
         */
        public void RenderSkeleton(GraphicsDevice GraphicsDevice)
        {
            // Create the skeleton renderer projection matrix
            ((BasicEffect)skeletonRenderer.Effect).Projection = Matrix.CreateOrthographicOffCenter(
            0,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height,
                0, 1, 0);

            // Draw skeletons
            skeletonRenderer.Begin();
            skeletonRenderer.Draw(skeleton);
            skeletonRenderer.End();
        }

        /*
         * Update the hitbox after a collision.
         */
        public void UpdateHitbox(SkeletonBounds updatedHitbox)
        {
            hitbox = updatedHitbox;
        }

        /*
         * Get the skeleton.
         */
        public Skeleton GetSkeleton()
        {
            return skeleton;
        }

        /*
         * Get the hitbox.
         */
        public SkeletonBounds GetHitbox()
        {
            return hitbox;
        }
    }
}