using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spine;

namespace SoR.Logic.Entities
{
    /*
     * Stores information unique to the campfire entity.
     */
    internal class Campfire : Entity
    {
        public Campfire(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice)
        {
            // Load texture atlas and attachment loader
            //atlas = new Atlas("F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Campfire\\templecampfire.atlas", new XnaTextureLoader(GraphicsDevice));
            atlas = new Atlas("D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Campfire\\templecampfire.atlas", new XnaTextureLoader(GraphicsDevice));
            atlasAttachmentLoader = new AtlasAttachmentLoader(atlas);
            json = new SkeletonJson(atlasAttachmentLoader);

            // Initialise skeleton json
            //skeletonData = json.ReadSkeletonData("F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Campfire\\skeleton.json");
            skeletonData = json.ReadSkeletonData("D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Campfire\\skeleton.json");
            skeleton = new Skeleton(skeletonData);

            // Set the skin
            skeleton.SetSkin(skeletonData.FindSkin("default"));

            // Setup animation
            animStateData = new AnimationStateData(skeleton.Data);
            animState = new AnimationState(animStateData);
            animState.Apply(skeleton);

            // Set the "fidle" animation on track 1 and leave it looping forever
            animState.SetAnimation(0, "idle", true);

            // Create hitbox
            slot = skeleton.FindSlot("hitbox");
            hitboxAttachment = skeleton.GetAttachment("hitbox", "hitbox");
            slot.Attachment = hitboxAttachment;
            skeleton.SetAttachment("hitbox", "hitbox");

            // Initialise skeleton renderer with premultiplied alpha
            skeletonRenderer = new SkeletonRenderer(GraphicsDevice);
            skeletonRenderer.PremultipliedAlpha = true;

            // Set the current position on the screen
            position = new Vector2(graphics.PreferredBackBufferWidth / 2,
                graphics.PreferredBackBufferHeight / 2);

            positionX = position.X; // Set the x-axis position
            positionY = position.Y; // Set the y-axis position

            Speed = 200f; // Set the entity's travel speed

            hitpoints = 100; // Set the starting number of hitpoints
        }

        /*
         * Check for collision with other entities.
         */
        public override bool CollidesWith(Entity entity)
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
        public override void UpdateHitbox(SkeletonBounds updatedHitbox)
        {
            hitbox = updatedHitbox;
        }

        /*
         * Get the animation state.
         */
        public override AnimationState GetAnimState()
        {
            return animState;
        }

        /*
         * Get the skeleton.
         */
        public override Skeleton GetSkeleton()
        {
            return skeleton;
        }

        /*
         * Get the hitbox.
         */
        public override SkeletonBounds GetHitbox()
        {
            return hitbox;
        }

        /*
         * Update the entity position, animation state and skeleton.
         */
        public override void UpdateEntityAnimations(GameTime gameTime)
        {
            // Update the animation state and apply animations to skeletons
            skeleton.X = positionX;
            skeleton.Y = positionY;

            hitbox.Update(skeleton, true);
            animState.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            skeleton.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            animState.Apply(skeleton);

            // Update skeletal transformations
            skeleton.UpdateWorldTransform(Skeleton.Physics.Update);
        }

        /*
         * Render the current skeleton to the screen.
         */
        public override void RenderSkeleton(GraphicsDevice GraphicsDevice)
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
         * Draw text to the screen.
         */
        public override void DrawText(SpriteBatch spriteBatch, SpriteFont font)
        {
            /*spriteBatch.Begin();
            spriteBatch.DrawString(font, "File", new Vector2(100, 100), Color.BlueViolet);
            spriteBatch.End();*/
        }

        /* 
         * Get the centre of the screen.
         */
        public override void GetScreenCentre(Vector2 centreScreen)
        {
            position = centreScreen;

            position = new Vector2(position.X - 50, position.Y - 150);

            positionX = position.X; // Set the x-axis position
            positionY = position.Y; // Set the y-axis position
        }

        /*
         * Get the current x-axis position.
         */
        public override float GetPositionX()
        {

            return positionX;
        }

        /*
         * Get the current y-axis position.
         */
        public override float GetPositionY()
        {
            return positionY;
        }
    }
}