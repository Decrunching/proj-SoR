using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoR.Logic.Input;
using Spine;
using System;

namespace SoR.Logic.Entities
{
    /*
     * Stores information unique to the pheasant entity.
     */
    internal class Pheasant : Entity
    {
        public Pheasant(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice)
        {
            // Load texture atlas and attachment loader
            atlas = new Atlas("F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Pheasant\\savedthepheasant.atlas", new XnaTextureLoader(GraphicsDevice));
            //atlas = new Atlas("D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Pheasant\\savedthepheasant.atlas", new XnaTextureLoader(GraphicsDevice));
            atlasAttachmentLoader = new AtlasAttachmentLoader(atlas);
            json = new SkeletonJson(atlasAttachmentLoader);

            // Initialise skeleton json
            skeletonData = json.ReadSkeletonData("F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Pheasant\\skeleton.json");
            //skeletonData = json.ReadSkeletonData("D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Pheasant\\skeleton.json");
            skeleton = new Skeleton(skeletonData);

            // Set the skin
            skeleton.SetSkin(skeletonData.FindSkin("default"));

            // Setup animation
            animStateData = new AnimationStateData(skeleton.Data);
            animState = new AnimationState(animStateData);
            animState.Apply(skeleton);

            // Set the "fidle" animation on track 1 and leave it looping forever
            animState.SetAnimation(0, "run", true);

            // Create hitbox
            slot = skeleton.FindSlot("hitbox");
            hitboxAttachment = skeleton.GetAttachment("hitbox", "hitbox");
            slot.Attachment = hitboxAttachment;
            skeleton.SetAttachment("hitbox", "hitbox");

            // Initialise skeleton renderer with premultiplied alpha
            skeletonRenderer = new SkeletonRenderer(GraphicsDevice);
            skeletonRenderer.PremultipliedAlpha = true;

            random = new Random();
            movementDirection = new Vector2(0, 0); // The direction of movement
            newDirectionTime = (float)random.NextDouble() * 1f + 0.25f; // After 0.25-1 seconds, choose a new movement direction
            sinceLastChange = 0; // Time elapsed since last direction change
            NewDirection(random.Next(4)); // Choose a random new direction to move in
            inMotion = true; // Is currently moving freely

            movement = new InputMovement(); // Environmental collision handling

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
        public override void React(string eventTrigger)
        {
            if (eventTrigger != "none")
            {
                animState.SetAnimation(0, nextAnim, false);
                animState.AddAnimation(0, "run", true, 0);
                inMotion = false;
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
            string reaction = "none";

            if (prevTrigger != eventTrigger)
            {
                if (eventTrigger == "collision")
                {
                    prevTrigger = "collision";
                    nextAnim = "attack";
                    reaction = eventTrigger;
                }
                if(eventTrigger == "turnleft")
                {
                    skeleton.ScaleX = 1;
                }
                if (eventTrigger == "turnright")
                {
                    skeleton.ScaleX = -1;
                }
                React(reaction);
            }
        }

        /* 
         * Get the centre of the screen.
         */
        public override void SetStartPosition(Vector2 centreScreen)
        {
            position = centreScreen;

            position = new Vector2(position.X + 240, position.Y - 150);
        }
    }
}