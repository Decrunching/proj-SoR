using Spine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace SoR.Logic.Entities
{
    internal class Player
    {
        private SkeletonData skeletonData;
        private Skeleton skeleton;
        private Skin skin;
        private AnimationState animState;
        private SkeletonRenderer skeletonRenderer;
        private Vector2 position;
        private KeyboardState lastKeyState;
        private float speed;
        private int deadZone;

        /*
         * Constructor for creating the player object.
         */
        public Player(GraphicsDeviceManager _graphics, GraphicsDevice GraphicsDevice)
        {
            // Set the player's current position on the screen
            position = new Vector2(_graphics.PreferredBackBufferWidth / 2,
                _graphics.PreferredBackBufferHeight / 2);

            // Set the player's speed
            speed = 200f;

            // Set the joystick deadzone
            deadZone = 4096;

            // Load texture atlas and attachment loader
            //Atlas atlas = new Atlas("F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Player\\haltija.atlas", new XnaTextureLoader(GraphicsDevice));
            Atlas atlas = new Atlas("D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Player\\haltija.atlas", new XnaTextureLoader(GraphicsDevice));
            AtlasAttachmentLoader atlasAttachmentLoader = new AtlasAttachmentLoader(atlas);
            SkeletonJson json = new SkeletonJson(atlasAttachmentLoader);

            // Initialise skeleton json
            //skeletonData = json.ReadSkeletonData("F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Player\\skeleton.json");
            skeletonData = json.ReadSkeletonData("D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Player\\skeleton.json");
            skeleton = new Skeleton(skeletonData);

            // Set the skin (can be moved to a dependent class later)
            SetInitialSkin("1");

            // Set the required skin (needs to be its own function later if using this class as a base for other entities)
            skeleton.SetSkin(skin);
            skeleton.SetSlotsToSetupPose();

            // Setup animation
            AnimationStateData animStateData = new AnimationStateData(skeleton.Data);
            animState = new AnimationState(animStateData);
            animState.Apply(skeleton);

            // Set the "fidle" animation on track 1 and leave it looping forever
            animState.SetAnimation(0, "idle", true);
            
        }

        /*
         * Create the SkeletonRenderer for this player.
         */
        public void CreateSkeletonRenderer(GraphicsDevice GraphicsDevice)
        {
            // Initialise skeleton renderer with premultiplied alpha
            skeletonRenderer = new SkeletonRenderer(GraphicsDevice);
            skeletonRenderer.PremultipliedAlpha = true;
        }

        /*
         * Render the current player skeleton to the screen.
         */
        public void RenderSkeleton(GraphicsDevice GraphicsDevice)
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
         * Apply the default starting skin to the current player. Is currently a separate function as
         * it will likely be moved to another class later on if this current 'Player' class is later
         * repurposed for initialising other non-player entities in addition to the actual player.
         */
        public void SetInitialSkin(string skinName)
        {
            skin = skeletonData.FindSkin(skinName);
            if (skin == null) throw new System.ArgumentException("Can't find skin: " + skinName);
        }

        /*
         * Set the current running animation and move the player character across the screen according
         * to keyboard inputs. Set back to the idle animation if there are no current valid movement
         * inputs.
         */
        public void SetAnimRunning(GameTime gameTime, KeyboardState keyState)
        {
            Keys[] keysPressed = keyState.GetPressedKeys(); // Collect a list of keys currently being pressed
            Keys[] lastKeysPressed = new Keys[0]; // Collect a list of previously pressed keys that have just been released

            // Dictionary to store the input keys and whether they are currently up or pressed.
            Dictionary<Keys, bool> keyIsUp =
                new Dictionary<Keys, bool>()
                {
                    { Keys.Up, keyState.IsKeyUp(Keys.Up) },
                    { Keys.Down, keyState.IsKeyUp(Keys.Down) },
                    { Keys.Left, keyState.IsKeyUp(Keys.Left) },
                    { Keys.Right, keyState.IsKeyUp(Keys.Right) }
                };

            if (keyState.IsKeyDown(Keys.Up) & !lastKeyState.IsKeyDown(Keys.Up))
            {
                animState.SetAnimation(0, "runup", true);
            }
            if (keyState.IsKeyDown(Keys.Down) & !lastKeyState.IsKeyDown(Keys.Down))
            {
                animState.SetAnimation(0, "rundown", true);
            }
            if (keyState.IsKeyDown(Keys.Left) & !lastKeyState.IsKeyDown(Keys.Left))
            {
                animState.SetAnimation(0, "runleft", true);
            }
            if (keyState.IsKeyDown(Keys.Right) & !lastKeyState.IsKeyDown(Keys.Right))
            {
                animState.SetAnimation(0, "runright", true);
            }

            if (keyState.IsKeyDown(Keys.Up))
            {
                position.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                position.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (keyState.IsKeyDown(Keys.Left))
            {
                position.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            /*
             * If a key has just been released, set the running animation back to a direction the character
             * is currently moving in.
             */
            foreach (Keys key in keyIsUp.Keys)
            {
                if (!keyState.IsKeyDown(key) & lastKeyState.IsKeyDown(key))
                {
                    if (keyState.IsKeyDown(Keys.Right) &
                        !keyState.IsKeyDown(Keys.Left))
                    {
                        animState.SetAnimation(0, "runright", true);
                    }
                    if (keyState.IsKeyDown(Keys.Left) &
                        !keyState.IsKeyDown(Keys.Right))
                    {
                        animState.SetAnimation(0, "runleft", true);
                    }
                    if (keyState.IsKeyDown(Keys.Down) &
                        !keyState.IsKeyDown(Keys.Up))
                    {
                        animState.SetAnimation(0, "rundown", true);
                    }
                    if (keyState.IsKeyDown(Keys.Up) &
                        !keyState.IsKeyDown(Keys.Down))
                    {
                        animState.SetAnimation(0, "runup", true);
                    }
                    else if (!keyIsUp.Values.Contains(false))
                    {
                        animState.SetAnimation(0, "idle", true);
                    }
                }
            }

            lastKeyState = keyState;
            lastKeysPressed = keysPressed;

            if (Joystick.LastConnectedIndex == 0)
            {
                JoystickState jstate = Joystick.GetState(0);

                float updatedcharSpeed = speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (jstate.Axes[1] < -deadZone)
                {
                    position.Y -= updatedcharSpeed;
                }
                else if (jstate.Axes[1] > deadZone)
                {
                    position.Y += updatedcharSpeed;
                }

                if (jstate.Axes[0] < -deadZone)
                {
                    position.X -= updatedcharSpeed;
                }
                else if (jstate.Axes[0] > deadZone)
                {
                    position.X += updatedcharSpeed;
                }
            }
        }

        public void UpdateSkeletalAnimations(GameTime gameTime)
        {
            // Update the animation state and apply animations to skeletons
            skeleton.X = position.X;
            skeleton.Y = position.Y;
            animState.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            skeleton.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            animState.Apply(skeleton);

            // Update skeletal transformations
            skeleton.UpdateWorldTransform(Skeleton.Physics.Update);
        }
    }
}
