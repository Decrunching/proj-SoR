using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoR.Logic.Input;
using SoR.Logic.Spine;
using Spine;

namespace SoR.Logic.Entities
{
    /*
     * Stores information unique to the player entity.
     */
    internal class PlayerChar : PlayerEntity
    {
        private SpineSetUp spineSetUp;
        private AnimationStateData animStateData;
        private AnimationState animState;
        protected SkeletonRenderer skeletonRenderer;
        private PlayerInput playerInput;

        public PlayerChar(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice)
        {
            // Instantiate Spine skeletons and animations
            spineSetUp = new SpineSetUp(graphics, GraphicsDevice, GetAtlas(), GetJson(), GetSkin(), GetStartingAnim());

            // Setup animation
            animStateData = new AnimationStateData(spineSetUp.GetSkeleton().Data);
            animState = new AnimationState(animStateData);
            animState.Apply(spineSetUp.GetSkeleton());

            // Set the "fidle" animation on track 1 and leave it looping forever
            animState.SetAnimation(0, GetStartingAnim(), true);
            // Create the skeleton renderer
            CreateSkeletonRenderer(GraphicsDevice);

            playerInput = new PlayerInput(); // Instantiate the keyboard input

            // Set the current position on the screen
            position = new Vector2(graphics.PreferredBackBufferWidth / 2,
                graphics.PreferredBackBufferHeight / 2);

            positionX = position.X; // Set the x-axis position
            positionY = position.Y; // Set the y-axis position

            Speed = 200f; // Set the entity's travel speed
        }

        /*
         * Get the atlas path.
         */
        public override string GetAtlas()
        {
            return "F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Player\\Char sprites.atlas";
        }

        /*
         * Get the json path.
         */
        public override string GetJson()
        {
            return "F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Player\\skeleton.json";
            //return "D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Player\\skeleton.json";
        }

        /*
         * Get the starting skin.
         */
        public override string GetSkin()
        {
            return "solarknight-0";
        }

        /*
         * Get the starting animation.
         */
        public override string GetStartingAnim()
        {
            return "idlebattle";
        }

        /*
         * Create the SkeletonRenderer.
         */
        public override void CreateSkeletonRenderer(GraphicsDevice GraphicsDevice)
        {
            // Initialise skeleton renderer with premultiplied alpha
            skeletonRenderer = new SkeletonRenderer(GraphicsDevice);
            skeletonRenderer.PremultipliedAlpha = true;
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
            skeletonRenderer.Draw(spineSetUp.GetSkeleton());
            skeletonRenderer.End();
        }

        /*
         * Get the animation state.
         */
        public AnimationState GetAnimState()
        {
            return animState;
        }

        /*
         * Update entity position according to player input.
         */
        public void UpdateEntityPosition(
            GameTime gameTime,
            KeyboardState keyState,
            GraphicsDeviceManager graphics,
            GraphicsDevice GraphicsDevice,
            AnimationState animState)
        {
            // Pass the speed, position and animation state to PlayerInput for keyboard input processing
            playerInput.ProcessKeyboardInputs(gameTime,
                keyState,
                animState,
                GetSpeed(),
                GetPositionX(),
                GetPositionY());

            // Pass the speed to PlayerInput for joypad input processing
            playerInput.ProcessJoypadInputs(gameTime, GetSpeed());

            // Set the new position according to player input
            SetPositionX(playerInput.UpdatePositionX());
            SetPositionY(playerInput.UpdatePositionY());

            // Prevent the user from leaving the visible screen area
            playerInput.CheckScreenEdges(graphics,
                GraphicsDevice,
                GetPositionX(),
                GetPositionY());

            // Set the new position according to player input
            SetPositionX(playerInput.UpdatePositionX());
            SetPositionY(playerInput.UpdatePositionY());

            // Update the animation state and apply animations to skeletons
            spineSetUp.SetSkeleton(positionX, positionY);
        }

        /*
         * Set up Spine animations and skeletons.
         */
        public override void UpdatePositionAndAnimation(
            GameTime gameTime,
            KeyboardState keyState,
            GraphicsDeviceManager graphics,
            GraphicsDevice GraphicsDevice)
        {
            // Update position according to user input
            UpdateEntityPosition(
                gameTime,
                keyState, graphics,
                GraphicsDevice,
                animState);

            // Update animations
            spineSetUp.UpdateEntitySkeleton(gameTime, animState);
            animState.SetAnimation(0, UpdatedAnimation(), true);
        }

        /*
         * Get the new animation state as determined by player input.
         */
        public string UpdatedAnimation()
        {
            string animName = "";

            switch (playerInput.EntityDirection)
            {
                case PlayerInput.Direction.RunUp:
                    animName = "runup";
                    break;
                case PlayerInput.Direction.RunDown:
                    animName = "rundown";
                    break;
                case PlayerInput.Direction.RunLeft:
                    animName = "runleft";
                    break;
                case PlayerInput.Direction.RunRight:
                    animName = "runright";
                    break;
                case PlayerInput.Direction.IdleUp:
                    animName = "idleup";
                    break;
                case PlayerInput.Direction.IdleDown:
                    animName = "idledown";
                    break;
                case PlayerInput.Direction.IdleLeft:
                    animName = "idleleft";
                    break;
                case PlayerInput.Direction.IdleRight:
                    animName = "idleright";
                    break;
                case PlayerInput.Direction.IdleBattle:
                    animName = "idlebattle";
                    break;
            }

            if (animName == "") throw new System.EntryPointNotFoundException("No valid entity type received");

            return animName;
        }
    }
}