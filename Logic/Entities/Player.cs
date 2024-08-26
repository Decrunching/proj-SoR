using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoR.Logic.Input;
using Spine;

namespace SoR.Logic.Entities
{
    /*
     * Stores information unique to the player entity.
     */
    public class Player : Entity
    {
        private PlayerInput playerInput;
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

            // Set the "fidle" animation on track 1 and leave it looping forever
            animState.SetAnimation(0, "idlebattle", true);

            // Initialise skeleton renderer with premultiplied alpha
            skeletonRenderer = new SkeletonRenderer(GraphicsDevice);
            skeletonRenderer.PremultipliedAlpha = true;

            playerInput = new PlayerInput(); // Instantiate the keyboard input

            // Set the current position on the screen
            position = new Vector2(graphics.PreferredBackBufferWidth / 2,
                graphics.PreferredBackBufferHeight / 2);

            positionX = position.X; // Set the x-axis position
            PositionY = position.Y; // Set the y-axis position

            Speed = 200f; // Set the entity's travel speed
        }

        /*
         * If the player pressed space, switch to the next skin.
         */
        public void CheckSwitchSkin()
        {
            if (playerInput.SkinHasChanged())
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
         * Get the animation state.
         */
        public override AnimationState GetAnimState()
        {
            return animState;
        }

        /*
         * Get the skeleton.
         */
        public override Skeleton GetSkeleton()
        {
            return skeleton;
        }

        /*
         * Update entity position according to player input.
         */
        public void UpdateEntityPosition(
            GameTime gameTime,
            KeyboardState keyState,
            KeyboardState lastKeyState,
            GraphicsDeviceManager graphics,
            GraphicsDevice GraphicsDevice,
            AnimationState animState,
            Skeleton skeleton)
        {
            // Pass the speed, position and animation state to PlayerInput for keyboard input processing
            playerInput.ProcessKeyboardInputs(gameTime,
                keyState,
                lastKeyState,
                animState,
                Speed,
                positionX,
                PositionY);

            // Pass the speed to PlayerInput for joypad input processing
            playerInput.ProcessJoypadInputs(gameTime, Speed);

            // Set the new position according to player input
            positionX = playerInput.UpdatePositionX();
            PositionY = playerInput.UpdatePositionY();

            // Prevent the user from leaving the visible screen area
            playerInput.CheckScreenEdges(graphics,
                GraphicsDevice,
                positionX,
                PositionY);

            // Set the new position according to player input
            positionX = playerInput.UpdatePositionX();
            PositionY = playerInput.UpdatePositionY();
        }

        /*
         * Update the skeleton position, skin and animation state.
         */
        public override void UpdateEntityAnimations(GameTime gameTime)
        {
            // Check whether to change the skin
            CheckSwitchSkin();

            // Update the animation state and apply animations to skeletons
            skeleton.X = positionX;
            skeleton.Y = PositionY;

            animState.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            skeleton.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            animState.Apply(skeleton);

            // Update skeletal transformations
            skeleton.UpdateWorldTransform(Skeleton.Physics.Update);
        }

        /*
         * Render the current skeleton to the screen.
         */
        public override void RenderSkeleton(GraphicsDevice GraphicsDevice)
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
         * Get the centre of the screen.
         */
        public override void GetScreenCentre(Vector2 centreScreen)
        {
            position = centreScreen;

            position = new Vector2(position.X - 270, position.Y - 150);

            positionX = position.X; // Set the x-axis position
            PositionY = position.Y; // Set the y-axis position
        }
    }
}