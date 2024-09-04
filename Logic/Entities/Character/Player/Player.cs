using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoR.Logic.Entities;
using SoR.Logic.Input;
using Spine;
using System.Collections.Generic;
using Logic.Game;

namespace Logic.Entities.Character.Player
{
    /*
     * Stores information unique to Player.
     */
    public class Player : Entity
    {
        private string skin;

        public Player(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice)
        {
            // The possible animations to play as a string and the method to use for playing them as an int
            animations = new Dictionary<string, int>()
            {
                { "idlebattle", 1 },
                { "runup", 3 },
                { "rundown", 3 },
                { "runleft", 3 },
                { "runright", 3 }
            };

            // Load texture atlas and attachment loader
            atlas = new Atlas("F:\\MonoGame\\SoR\\SoR\\Content\\SoR Resources\\Entities\\Player\\Char sprites.atlas", new XnaTextureLoader(GraphicsDevice));
            //atlas = new Atlas("D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Player\\Char sprites.atlas", new XnaTextureLoader(GraphicsDevice));
            atlasAttachmentLoader = new AtlasAttachmentLoader(atlas);
            json = new SkeletonJson(atlasAttachmentLoader);

            // Initialise skeleton json
            skeletonData = json.ReadSkeletonData("F:\\MonoGame\\SoR\\SoR\\Content\\SoR Resources\\Entities\\Player\\skeleton.json");
            //skeletonData = json.ReadSkeletonData("D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Player\\skeleton.json");
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

            movement = new UserInput(); // Environmental collision handling

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
         */
        public override void ChangeAnimation(string eventTrigger)
        {
            string reaction = "none"; // Default to "none" if there will be no animation change

            if (prevTrigger != eventTrigger)
            {
                foreach (string animation in animations.Keys)
                {
                    if (eventTrigger == animation)
                    {
                        prevTrigger = animOne = reaction = animation;

                        React(reaction, animations[animation]);
                    }
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
                position.Y,
                maxPosition.X,
                maxPosition.Y);

            // Set the new position
            position = new Vector2(movement.UpdatePositionX(), movement.UpdatePositionY());
        }

        /*
         * Move to new position.
         */
        public override void Movement(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            prevPositionX = position.X;
            prevPositionY = position.Y;

            movement.CheckMovement(gameTime, graphics, animState, Speed, position.X, position.Y);
            ChangeAnimation(movement.AnimateMovement());

            // Set the new position according to player input
            position = new Vector2(movement.UpdatePositionX(), movement.UpdatePositionY());
        }

        /*
         * Update the skeleton position, skin and animation state.
         */
        public override void UpdateEntityAnimations(GameTime gameTime, Camera camera)
        {
            // Update the animation state and apply animations to skeletons
            skeleton.X = position.X - camera.GetCameraPositionX();
            skeleton.Y = position.Y - camera.GetCameraPositionY();

            hitbox.Update(skeleton, true);
            animState.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            skeleton.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            animState.Apply(skeleton);

            // Update skeletal transformations
            skeleton.UpdateWorldTransform(Skeleton.Physics.Update);

            // Check whether to change the skin
            CheckSwitchSkin();
        }

        /* 
         * Get the centre of the screen.
         */
        public override void SetStartPosition(GraphicsDeviceManager graphics)
        {
            // Set the default starting position on the screen
            position = new Vector2(position.X - 270, position.Y - 200);

            maxPosition = new Vector2(graphics.PreferredBackBufferWidth * 2,
                graphics.PreferredBackBufferHeight * 2); // Set the maximum range of movement
        }

        /*
         * Render the current skeleton to the screen.
         */
        public override void RenderEntity(GraphicsDevice GraphicsDevice)
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
    }
}