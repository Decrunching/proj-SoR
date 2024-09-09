using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using SoR.Logic.Entities;
using Spine;
using System.IO;

namespace Logic.Game.GameMap
{
    /*
     * Spine Runtimes License
     */
    /**************************************************************************************************************************
     * Copyright (c) 2013-2024, Esoteric Software LLC
     * 
     * Integration of the Spine Runtimes into software or otherwise creating derivative works of the Spine Runtimes is
     * permitted under the terms and conditions of Section 2 of the Spine Editor License Agreement:
     * http://esotericsoftware.com/spine-editor-license
     * 
     * Otherwise, it is permitted to integrate the Spine Runtimes into software or otherwise create derivative works of the
     * Spine Runtimes (collectively, "Products"), provided that each user of the Products must obtain their own Spine Editor
     * license and redistribution of the Products in any form must include this license and copyright notice.
     * 
     * THE SPINE RUNTIMES ARE PROVIDED BY ESOTERIC SOFTWARE LLC "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT
     * NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO
     * EVENT SHALL ESOTERIC SOFTWARE LLC BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
     * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES, BUSINESS INTERRUPTION, OR LOSS OF
     * USE, DATA, OR PROFITS) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
     * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THE SPINE RUNTIMES, EVEN IF ADVISED OF THE
     * POSSIBILITY OF SUCH DAMAGE.
     **************************************************************************************************************************/

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
         * Perform an interaction.
         */
        public abstract void InteractWith(Entity entity);

        /*
         * Define what happens on collision with an entity.
         */
        public abstract void Collision(Entity entity, GameTime gameTime);

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
        public virtual void RenderScenery(GraphicsDevice GraphicsDevice, OrthographicCamera camera)
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
         * Update the hitbox after a collision.
         */
        public void UpdateHitbox(SkeletonBounds updatedHitbox)
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
         * Draw text to the screen.
         */
        public void DrawText(SpriteBatch spriteBatch, SpriteFont font, OrthographicCamera camera)
        {
            spriteBatch.Begin(transformMatrix: camera.GetViewMatrix());
            spriteBatch.DrawString(
                font,
                "",
                new Vector2(position.X - 80, position.Y + 100),
                Color.BlueViolet);
            spriteBatch.End();
        }

        /*
         * Set entity position to the centre of the screen +/- any x,y axis adjustment.
         */
        public virtual void SetPosition(float xAdjustment, float yAdjustment)
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

        public Vector2 GetPosition()
        {
            return position;
        }
    }
}