using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoR.Logic.Entities;
using SoR.Logic.Input;
using Spine;
using System;

namespace Logic.Entities.Character
{
    /*
     * Stores information unique to Player.
     */
    public class Player : Entity
    {
        private string skin;

        public Player(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice)
        {
            // Load texture atlas and attachment loader
            //atlas = new Atlas("F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Player\\Char sprites.atlas", new XnaTextureLoader(GraphicsDevice));
            atlas = new Atlas("D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Player\\Char sprites.atlas", new XnaTextureLoader(GraphicsDevice));
            atlasAttachmentLoader = new AtlasAttachmentLoader(atlas);
            json = new SkeletonJson(atlasAttachmentLoader);

            // Initialise skeleton json
            //skeletonData = json.ReadSkeletonData("F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Player\\skeleton.json");
            skeletonData = json.ReadSkeletonData("D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Player\\skeleton.json");
            skeleton = new Skeleton(skeletonData);

            // Set the skin
            skeleton.SetSkin(skeletonData.FindSkin("solarknight-0"));
            skin = "solarknight-0";

            // Setup animation
            animStateData = new AnimationStateData(skeleton.Data);
            animState = new AnimationState(animStateData);
            animState.Apply(skeleton);
            animStateData.DefaultMix = 0.1f;

            // Set the "fidle" animation on track 1 and leave it looping forever
            trackEntry = animState.SetAnimation(0, "idlebattle", true);

            // Create hitbox
            slot = skeleton.FindSlot("hitbox");
            hitboxAttachment = skeleton.GetAttachment("hitbox", "hitbox");
            slot.Attachment = hitboxAttachment;
            skeleton.SetAttachment("hitbox", "hitbox");

            // Initialise skeleton renderer with premultiplied alpha
            skeletonRenderer = new SkeletonRenderer(GraphicsDevice);
            skeletonRenderer.PremultipliedAlpha = true;

            hitbox = new SkeletonBounds();

            random = new Random();
            movementDirection = new Vector2(0, 0); // The direction of movement
            newDirectionTime = (float)random.NextDouble() * 1f + 0.25f; // After 0.25-1 seconds, choose a new movement direction
            sinceLastChange = 0; // Time elapsed since last direction change
            NewDirection(random.Next(4)); // Choose a random new direction to move in

            inMotion = true; // Move freely

            movement = new InputMovement(); // Environmental collision handling

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
         * If something changes to trigger a new animation, apply the animation.
         * If the animation is already applied, do nothing.
         * 
         * TO DO: Fix this.
         */
        public override void ChangeAnimation(string eventTrigger)
        {
            string reaction = "none"; // Default to "none" if there will be no animation change

            /*
             * 0 = no animation, 1 = rapidly transition to next, 2 = set new animation then queue
             * the next, 3 = start animation on the same frame the previous animation was at.
             */
            int animType = 0;

            if (prevTrigger != eventTrigger)
            {
                if (eventTrigger == "idlebattle")
                {
                    prevTrigger = eventTrigger;
                    animType = 1;
                    animOne = "idlebattle";
                    reaction = eventTrigger;
                    React(reaction, animType);
                }
                if (eventTrigger == "runup")
                {
                    prevTrigger = eventTrigger;
                    animType = 3;
                    animOne = "runup";
                    reaction = eventTrigger;
                    React(reaction, animType);
                }
                if (eventTrigger == "rundown")
                {
                    prevTrigger = eventTrigger;
                    animType = 3;
                    animOne = "rundown";
                    reaction = eventTrigger;
                    React(reaction, animType);
                }
                if (eventTrigger == "runleft")
                {
                    prevTrigger = eventTrigger;
                    animType = 3;
                    animOne = "runleft";
                    reaction = eventTrigger;
                    React(reaction, animType);
                }
                if (eventTrigger == "runright")
                {
                    prevTrigger = eventTrigger;
                    animType = 3;
                    animOne = "runright";
                    reaction = eventTrigger;
                    React(reaction, animType);
                }
            }
        }

        /*
         * Placeholder function for handling battles.
         */
        public void Battle(Entity entity)
        {
            /*
            if (entities.TryGetValue("player", out Entity playerChar))
            {
                if (playerChar is Player player)
                {
                    If (entity.CollidesWith(player))
                    {
                        player.Battle(entity);
                    }
                }
                else
                {
                    // Throw exception if playerChar is somehow not of the type Player
                    throw new System.InvalidOperationException("playerChar is not of type Player");
                }
            }
             */
        }

        /*
         * If the player pressed space, switch to the next skin.
         */
        public void CheckSwitchSkin()
        {
            if (movement.SkinHasChanged())
            {
                switch (skin)
                {
                    case "solarknight-0":
                        skeleton.SetSkin(skeletonData.FindSkin("lunarknight-0"));
                        skin = "lunarknight-0";
                        break;
                    case "lunarknight-0":
                        skeleton.SetSkin(skeletonData.FindSkin("knight-0"));
                        skin = "knight-0";
                        break;
                    case "knight-0":
                        skeleton.SetSkin(skeletonData.FindSkin("solarknight-0"));
                        skin = "solarknight-0";
                        break;
                }
            }
        }

        /*
         * Update entity position.
         */
        public override void UpdatePosition(GameTime gameTime, GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice)
        {
            // Process joypad inputs
            movement.ProcessJoypadInputs(gameTime, Speed);

            // Handle environmental collision
            movement.EnvironCollision(
                graphics,
                GraphicsDevice,
                GetEntityHitbox(),
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

            movement.CheckMovement(gameTime, animState, Speed, position.X, position.Y);
            ChangeAnimation(movement.AnimateMovement());


            // Set the new position according to player input
            position = new Vector2(movement.UpdatePositionX(), movement.UpdatePositionY());
        }

        /*
         * Update the skeleton position, skin and animation state.
         */
        public override void UpdateEntityAnimations(GameTime gameTime)
        {
            base.UpdateEntityAnimations(gameTime);

            // Check whether to change the skin
            CheckSwitchSkin();
        }

        /* 
         * Get the centre of the screen.
         */
        public override void SetStartPosition(Vector2 centreScreen)
        {
            position = centreScreen;

            position = new Vector2(position.X - 270, position.Y - 200);
        }
    }
}