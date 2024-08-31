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
            atlas = new Atlas("F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Campfire\\templecampfire.atlas", new XnaTextureLoader(GraphicsDevice));
            //atlas = new Atlas("D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Campfire\\templecampfire.atlas", new XnaTextureLoader(GraphicsDevice));
            atlasAttachmentLoader = new AtlasAttachmentLoader(atlas);
            json = new SkeletonJson(atlasAttachmentLoader);

            // Initialise skeleton json
            skeletonData = json.ReadSkeletonData("F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Campfire\\skeleton.json");
            //skeletonData = json.ReadSkeletonData("D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Campfire\\skeleton.json");
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

            Speed = 200f; // Set the entity's travel speed

            hitpoints = 100; // Set the starting number of hitpoints
        }

        /*
         * Placeholder function for dealing damage.
         */
        public override int Damage(Entity entity)
        {
            if (CollidesWith(entity))
            {
                int damage = 5;
                return damage;
            }
            return 0;
        }

        /*
         * On first collision, play collision animation.
         */
        public override void React(string animation)
        {
        }

        /*
         * If something changes to trigger a new animation, apply the animation.
         * If the animation is already applied, do nothing.
         */
        public override void ChangeAnimation(string trigger)
        {
            prevTrigger = trigger;
        }

        /*
         * No longer in collision.
         */
        public override void ResetCollision()
        {
            prevTrigger = "none";
        }

        /*
         * Update the entity position, animation state and skeleton.
         */
        public override void UpdateEntityAnimations(GameTime gameTime)
        {
            // Update the animation state and apply animations to skeletons
            skeleton.X = position.X;
            skeleton.Y = position.Y;

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
        public override void DrawText(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(
                font,
                "",
                new Vector2(position.X - 150, position.Y + hitbox.Height / 2),
                Color.BlueViolet);
            spriteBatch.End();
        }

        /* 
         * Get the centre of the screen.
         */
        public override void SetStartPosition(Vector2 centreScreen)
        {
            position = centreScreen;

            position = new Vector2(position.X - 50, position.Y - 150);
        }
    }
}