using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoR.Logic.Input;
using Spine;
using System;

namespace SoR.Logic.Entities
{
    /*
     * Stores information unique to the slime entity.
     */
    internal class Slime : Entity
    {
        public Slime(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice)
        {
            // Load texture atlas and attachment loader
            atlas = new Atlas("F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Slime\\Slime.atlas", new XnaTextureLoader(GraphicsDevice));
            //atlas = new Atlas("D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Slime\\Slime.atlas", new XnaTextureLoader(GraphicsDevice));
            atlasAttachmentLoader = new AtlasAttachmentLoader(atlas);
            json = new SkeletonJson(atlasAttachmentLoader);

            // Initialise skeleton json
            skeletonData = json.ReadSkeletonData("F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Slime\\skeleton.json");
            //skeletonData = json.ReadSkeletonData("D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Slime\\skeleton.json");
            skeleton = new Skeleton(skeletonData);

            // Set the skin
            skeleton.SetSkin(skeletonData.FindSkin("default"));

            // Setup animation
            animStateData = new AnimationStateData(skeleton.Data);
            animState = new AnimationState(animStateData);
            animState.Apply(skeleton);

            prevTrigger = "none";
            nextAnim = null;

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

            random = new Random();
            moving = new Vector2(0, 0);
            newDirectionTime = (float)random.NextDouble() * 5f + 2f;
            sinceLastChange = 0;
            NewDirection();

            movement = new Movement();

            // Set the current position on the screen
            position = new Vector2(graphics.PreferredBackBufferWidth / 2,
                graphics.PreferredBackBufferHeight / 2);

            Speed = 80f; // Set the entity's travel speed

            hitpoints = 100; // Set the starting number of hitpoints
        }

        /*
         * Placeholder function for dealing damage.
         */
        public override int Damage(Entity entity)
        {
            int damage = 5;
            return damage;
        }

        /*
         * On first collision, play collision animation.
         */
        public override void React(string eventTrigger)
        {
            if (eventTrigger != "none")
            {
                animState.SetAnimation(0, nextAnim, false);
                animState.AddAnimation(0, "idle", true, 0);
            }
        }

        /*
         * If something changes to trigger a new animation, apply the animation.
         * If the animation is already applied, do nothing.
         * 
         * TO DO: Fix this.
         */
        public override void ChangeAnimation(string eventTrigger)
        {
            if (prevTrigger != eventTrigger)
            {
                if (eventTrigger == "collision")
                {
                    prevTrigger = "collision";
                    nextAnim = "attack";
                }
                React(eventTrigger);
            }
        }

        /*
         * No longer in collision.
         */
        public override void ResetCollision()
        {
            prevTrigger = "none";
        }

        /*
         * Update entity position.
         */
        public override void UpdatePosition(GameTime gameTime, GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice)
        {
            // Handle environmental collision
            movement.EnvironCollision(graphics,
                GraphicsDevice,
                position.X,
                position.Y);

            // Set the new position
            position = new Vector2(movement.UpdatePositionX(), movement.UpdatePositionY());
        }

        /*
         * Move to new position.
         */
        public override void Movement(GameTime gameTime)
        {
            prevPositionX = position.X;
            prevPositionY = position.Y;

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            float newSpeed = Speed * deltaTime;

            sinceLastChange += deltaTime;

            if (sinceLastChange >= newDirectionTime)
            {
                NewDirection();
                newDirectionTime = (float)random.NextDouble() * 1f + 0.25f;
                sinceLastChange = 0;
            }

                position += moving * newSpeed;
        }

        /*
         * Choose a new direction to face.
         */
        public void NewDirection()
        {
            int direction = random.Next(4);

            switch (direction)
            {
                case 0:
                    moving = new Vector2(0, -1); // Up
                    break;
                case 1:
                    moving = new Vector2(0, 1); // Down
                    break;
                case 2:
                    moving = new Vector2(-1, 0); // Left
                    break;
                case 3:
                    moving = new Vector2(1, 0); // Right
                    break;
            }
        }

        /*
         * Handle entity collision.
         * 
         * TO DO:
         * Player should still be able to move perpendicular to hitbox edge when in collision.
         */
        public override void Collision()
        {
            position.X = prevPositionX;
            position.Y = prevPositionY;
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

            position = new Vector2(position.X + 200, position.Y + 200);
        }
    }
}