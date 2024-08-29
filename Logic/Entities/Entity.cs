using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoR.Logic.Input;
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
        protected PlayerInput playerInput;
        protected Vector2 position;
        protected float positionX;
        protected float positionY;
        protected float prevPositionX;
        protected float prevPositionY;
        protected int hitpoints;

        // DEBUGGING
        protected string showMaxX;
        protected string showMaxY;
        protected string showMinX;
        protected string showMinY;
        protected string showHitboxWidth;
        protected string showHitboxHeight;
        protected string showPositionX;
        protected string showPositionY;

        public float Speed { get; set; }
        public string Name { get; set; }
        public bool Render { get; set; }

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
         * Update the hitbox after a collision.
         */
        public void UpdateHitbox(SkeletonBounds updatedHitbox)
        {
            hitbox = updatedHitbox;
        }

        /*
         * Get the animation state.
         */
        public AnimationState GetAnimState()
        {
            return animState;
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

        /*
         * Update the entity position, animation state and skeleton.
         */
        public abstract void UpdateEntityAnimations(GameTime gameTime);

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

            // Set the text above the character to show
            // MaxX, MaxY, MinX, MinY, positionX, positionY, hitbox width, and/or hitbox height
            showMaxX = hitbox.MaxX.ToString();
            showMaxY = hitbox.MaxY.ToString();
            showMinX = hitbox.MinX.ToString();
            showMinY = hitbox.MinY.ToString();
            showPositionX = positionX.ToString();
            showPositionY = positionY.ToString();
            showHitboxWidth = hitbox.Width.ToString();
            showHitboxHeight = hitbox.Height.ToString();
        }

        /*
         * Draw text to the screen.
         */
        public abstract void DrawText(SpriteBatch spriteBatch, SpriteFont font);

        /* 
         * Get the centre of the screen.
         */
        public abstract void SetStartPosition(Vector2 centreScreen);

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
    }
}