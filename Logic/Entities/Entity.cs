using Logic.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ECS;
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
        protected Vector2 prevPosition;
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
            position = prevPosition;
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
         * Check for collision with other entities.
         */
        public bool CollidesWith(Entity entity)
        {
            entity.UpdateEntityHitbox(new SkeletonBounds());
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
         * Move to new position.
         */
        public virtual void Movement(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            prevPosition = position;

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
         * Update entity position.
         */
        public virtual void UpdatePosition(GameTime gameTime, GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice)
        {
            // Handle environmental collision
            if (movement.EnvironCollision(
                graphics,
                GraphicsDevice,
                GetHitbox(),
                position,
                maxPosition))
            {
                NewDirection(movement.TurnAround());
            }

            // Set the new position
            position = movement.UpdatePosition();
        }

        /*
         * Update the hitbox after a collision.
         */
        public void UpdateEntityHitbox(SkeletonBounds updatedHitbox)
        {
            hitbox = updatedHitbox;
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
         * Render the current skeleton to the screen.
         */
        public virtual void RenderEntity(GraphicsDevice GraphicsDevice, OrthographicCamera camera)
        {
            // Create the skeleton renderer projection matrix
            ((BasicEffect)skeletonRenderer.Effect).Projection = Matrix.CreateOrthographicOffCenter(
            0,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height,
                0, 1, 0);
            ((BasicEffect)skeletonRenderer.Effect).View = camera.GetViewMatrix();

            // Draw skeletons
            skeletonRenderer.Begin();

            // Update the animation state and apply animations to skeletons
            skeleton.X = position.X;
            skeleton.Y = position.Y;

            skeletonRenderer.Draw(skeleton);
            skeletonRenderer.End();
        }

        /*
         * Draw text to the screen.
         */
        public void DrawText(SpriteBatch spriteBatch, SpriteFont font, OrthographicCamera camera)
        {
            spriteBatch.Begin(transformMatrix: camera.GetViewMatrix());
            spriteBatch.DrawString(
                font,
                "",
                new Vector2(position.X - 80, position.Y + 50),
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
         * Get the entity position.
         */
        public Vector2 GetPosition()
        {
            return position;
        }
    }
}