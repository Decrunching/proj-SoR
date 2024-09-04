using Logic.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoR.Logic.Input;
using Spine;
using System;
using System.Collections.Generic;

namespace SoR.Logic.Entities
{
    /*
     * Common functions and fields for player and non-player characters.
     */
    public abstract class Entity
    {
        protected Dictionary<string, int> animations;
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
        protected Random random;
        protected UserInput movement;
        protected GraphicsSettings settings;
        protected Vector2 position;
        protected Vector2 maxPosition;  
        protected Vector2 movementDirection;
        protected float prevPositionX;
        protected float prevPositionY;
        protected int hitpoints;
        protected string prevTrigger;
        protected string animOne;
        protected string animTwo;
        protected float newDirectionTime;
        protected float sinceLastChange;
        protected bool inMotion;

        public float Speed { get; set; }
        public bool Render { get; set; }

        /*
         * Placeholder function for dealing damage.
         */
        public abstract int Damage(Entity entity);

        /*
         * If something changes to trigger a new animation, apply the animation.
         * If the animation is already applied, do nothing.
         */
        public abstract void ChangeAnimation(string trigger);

        /* 
         * Get the centre of the screen.
         */
        public abstract void SetStartPosition(GraphicsDeviceManager graphics);

        /*
         * Check if moving.
         */
        public bool IsMoving()
        {
            return inMotion;
        }

        /*
         * Start moving and switch to running animation.
         */
        public void StartMoving()
        {
            inMotion = true;
            ChangeAnimation("move");
        }

        /*
         * Stop moving.
         */
        public void StopMoving()
        {
            inMotion = false;
            ChangeAnimation("collision"); // TO DO: Fix - see collision
            position.X = prevPositionX;
            position.Y = prevPositionY;
        }

        /*
         * Choose a method for playing the animation according to Player.ChangeAnimation(eventTrigger)
         * animType.
         * 
         * 1 = rapidly transition to next animation
         * 2 = set new animation then queue the next
         * 3 = start next animation on the same frame the previous animation finished on
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
         * Update entity position.
         */
        public virtual void UpdatePosition(GameTime gameTime, GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice)
        {
            // Handle environmental collision
            if (movement.EnvironCollision(
                graphics,
                GraphicsDevice,
                GetEntityHitbox(),
                position.X,
                position.Y,
                maxPosition.X,
                maxPosition.Y))
            {
                NewDirection(movement.TurnAround());
            }

            // Set the new position
            position = new Vector2(movement.UpdatePositionX(), movement.UpdatePositionY());
        }

        /*
         * Choose a new direction to face.
         */
        public void NewDirection(int direction)
        {
            switch (direction)
            {
                case 1:
                    movementDirection = new Vector2(-1, 0); // Left
                    ChangeAnimation("turnleft");
                    break;
                case 2:
                    movementDirection = new Vector2(1, 0); // Right
                    ChangeAnimation("turnright");
                    break;
                case 3:
                    movementDirection = new Vector2(0, -1); // Up
                    break;
                case 4:
                    movementDirection = new Vector2(0, 1); // Down
                    break;
            }
        }

        /*
         * Move to new position.
         */
        public virtual void Movement(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            prevPositionX = position.X;
            prevPositionY = position.Y;

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            float newSpeed = Speed * deltaTime;

            sinceLastChange += deltaTime;

            if (IsMoving())
            {
                if (sinceLastChange >= newDirectionTime)
                {
                    int direction = random.Next(4);
                    NewDirection(direction);
                    newDirectionTime = (float)random.NextDouble() * 3f + 0.33f;
                    sinceLastChange = 0;
                }

                position += movementDirection * newSpeed;
            }
        }

        /*
         * Check for collision with other entities.
         */
        public bool CollidesWith(Entity entity)
        {
            entity.UpdateEntityHitbox(new SkeletonBounds());
            entity.GetEntityHitbox().Update(entity.GetEntitySkeleton(), true);

            hitbox = new SkeletonBounds();
            hitbox.Update(skeleton, true);

            if (hitbox.AabbIntersectsSkeleton(entity.GetEntityHitbox()))
            {
                return true;
            }

            return false;
        }

        /*
         * Update the hitbox after a collision.
         */
        public void UpdateEntityHitbox(SkeletonBounds updatedHitbox)
        {
            hitbox = updatedHitbox;
        }

        /*
         * Render the current skeleton to the screen.
         */
        public virtual void RenderEntity(GraphicsDevice GraphicsDevice)
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
         * Update the entity position, animation state and skeleton.
         */
        public virtual void UpdateEntityAnimations(GameTime gameTime)
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
        public void DrawEntityText(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(
                font,
                "newDirection: " + newDirectionTime,
                new Vector2(position.X - 50, position.Y + hitbox.Height / 2),
                Color.BlueViolet);
            spriteBatch.End();
        }

        /*
         * Get the skeleton.
         */
        public Skeleton GetEntitySkeleton()
        {
            return skeleton;
        }

        /*
         * Get the hitbox.
         */
        public SkeletonBounds GetEntityHitbox()
        {
            return hitbox;
        }

        /*
         * Get the X-axis position.
         */
        public float GetPositionX()
        {
            return position.X;
        }

        /*
         * Get the Y-axis position.
         */
        public float GetPositionY()
        {
            return position.Y;
        }

        /*
         * 
         */
        public Vector2 GetPosition()
        {
            return position;
        }
    }
}