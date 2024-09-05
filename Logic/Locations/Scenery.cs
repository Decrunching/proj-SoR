using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spine;

namespace Logic.Locations
{
    /*
     * Common functions and fields for environmental entities.
     */
    public abstract class Scenery
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
        protected TrackEntry trackEntry;
        protected Vector2 position;
        protected string prevTrigger;
        protected string animOne;
        protected string animTwo;
        public bool Render { get; set; }

        /*
         * If something changes to trigger a new animation, apply the animation.
         * If the animation is already applied, do nothing.
         */
        public abstract void ChangeAnimation(string trigger);

        /*
         * Choose a method for playing the animation according to Player.ChangeAnimation(eventTrigger)
         * animType.
         */
        public void React(string reaction, int animType)
        {
            if (reaction != "none")
            {
                if (animType == 1)
                {
                    animState.AddAnimation(0, animOne, true, -trackEntry.TrackComplete);
                }
                if (animType == 2)
                {
                    animState.SetAnimation(0, animOne, false);
                    trackEntry = animState.AddAnimation(0, animTwo, true, 0);
                }
                if (animType == 3)
                {
                    animState.AddAnimation(0, animOne, true, -trackEntry.TrackTime);
                }
            }
        }

        /*
         * Update the hitbox after a collision.
         */
        public void UpdateHitbox(SkeletonBounds updatedHitbox)
        {
            hitbox = updatedHitbox;
        }

        /*
         * Render the current skeleton to the screen.
         */
        public void RenderScenery(GraphicsDevice GraphicsDevice)
        {
            // Create the skeleton renderer projection matrix
            ((BasicEffect)skeletonRenderer.Effect).Projection = Matrix.CreateOrthographicOffCenter(
            0,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height,
                0, 1, 0);

            // Update the animation state and apply animations to skeletons
            skeleton.X = position.X;
            skeleton.Y = position.Y;

            // Draw skeletons
            skeletonRenderer.Begin();
            skeletonRenderer.Draw(skeleton);
            skeletonRenderer.End();
        }

        /*
         * Update the entity position, animation state and skeleton.
         */
        public virtual void UpdateAnimations(GameTime gameTime)
        {
            hitbox.Update(skeleton, true);
            animState.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            skeleton.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            animState.Apply(skeleton);

            // Update skeletal transformations
            skeleton.UpdateWorldTransform(Skeleton.Physics.Update);
        }

        /*
         * Draw text to the screen.
         */
        public void DrawText(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(
                font,
                "Position: " + skeleton.X + ", " + skeleton.Y,
                new Vector2(position.X, position.Y + 200),
                Color.BlueViolet);
            spriteBatch.End();
        }

        /*
         * Set entity position to the centre of the screen +/- any x,y axis adjustment.
         */
        public void SetPosition(GraphicsDeviceManager graphics, float xAdjustment, float yAdjustment)
        {
            position = new Vector2(xAdjustment, yAdjustment);
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