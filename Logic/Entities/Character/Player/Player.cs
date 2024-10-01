using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoR;
using SoR.Logic.Input;
using Spine;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Logic.Entities.Character.Player
{
    /*
     * Stores information unique to Player.
     */
    public class Player : Entity
    {
        private KeyboardState keyState;
        private KeyboardState lastKeyState;
        private bool switchSkin;

        [JsonConstructor]
        public Player(GraphicsDevice GraphicsDevice, List<Rectangle> impassableArea)
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
            atlas = new Atlas(Globals.GetPath("Content\\SoR Resources\\Entities\\Player\\Char sprites.atlas"), new XnaTextureLoader(GraphicsDevice));
            atlasAttachmentLoader = new AtlasAttachmentLoader(atlas);
            json = new SkeletonJson(atlasAttachmentLoader);

            // Initialise skeleton json
            skeletonData = json.ReadSkeletonData(Globals.GetPath("Content\\SoR Resources\\Entities\\Player\\skeleton.json"));
            skeleton = new Skeleton(skeletonData);

            // Set the skin
            skeleton.SetSkin(skeletonData.FindSkin("solarknight-0"));
            Skin = "solarknight-0";

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

            hitbox = new SkeletonBounds();
            hitbox.Update(skeleton, true);

            movement = new Movement(); // Environmental collision handling
            Player = true;

            Speed = 100f; // Set the entity's travel speed
            HitPoints = 100; // Set the starting number of hitpoints

            ImpassableArea = impassableArea;
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
         * Check whether the skin has changed.
         */
        public void CheckForSkinChange()
        {
            keyState = Keyboard.GetState(); // Get the current keyboard state
            switchSkin = false; // Space has not been pressed yet, the skin will not be switched

            if (keyState.IsKeyDown(Keys.Space) & !lastKeyState.IsKeyDown(Keys.Space))
            {
                switchSkin = true; // Space was pressed, so switch skins
            }
            lastKeyState = keyState;
        }

        /*
         * If the player pressed space, switch to the next skin.
         */
        public void UpdateSkin()
        {
            if (switchSkin)
            {
                switch (Skin)
                {
                    case "solarknight-0":
                        skeleton.SetSkin(skeletonData.FindSkin("lunarknight-0"));
                        Skin = "lunarknight-0";
                        break;
                    case "lunarknight-0":
                        skeleton.SetSkin(skeletonData.FindSkin("knight-0"));
                        Skin = "knight-0";
                        break;
                    case "knight-0":
                        skeleton.SetSkin(skeletonData.FindSkin("solarknight-0"));
                        Skin = "solarknight-0";
                        break;
                }
            }
        }

        /*
         * Define what happens on collision with an entity.
         */
        public override void EntityCollision(Entity entity, GameTime gameTime)
        {
            entity.TakeDamage(1);
            entity.ChangeAnimation("hit");
            movement.RepelledFromEntity(Position, 4, entity);
        }

        /*
         * Update entity position.
         */
        public override void UpdatePosition(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            BeMoved(gameTime);
            movement.CheckMovement(Position);

            movement.AdjustPosition(gameTime, this, ImpassableArea);
            Position = movement.UpdatePosition();

            prevPosition = Position;
        }

        /*
         * Update the skeleton position, skin and animation state.
         */
        public override void UpdateAnimations(GameTime gameTime)
        {
            CheckForSkinChange();
            UpdateSkin();
            ChangeAnimation(movement.AnimateMovement());

            base.UpdateAnimations(gameTime);
        }
    }
}