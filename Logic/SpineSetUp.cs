using Spine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoR.Logic.Input;
using SoR.Logic.Entities;

namespace SoR.Logic
{
    /*
     * The Player object class. Used to create and update the Player object. Currently includes
     * initialisation, position/movement and animation.
     */
    internal class SpineSetUp
    {
        protected SkeletonData skeletonData;
        protected Skeleton skeleton;
        protected Skin skin;
        protected AnimationState animState;
        protected SkeletonRenderer skeletonRenderer;

        private PlayerInput playerInput;
        private GameLogic gameLogic;
        private Entity entity;

        /*
         * Constructor for creating the player object.
         */
        public SpineSetUp(GraphicsDeviceManager _graphics, GraphicsDevice GraphicsDevice)
        {
            // Instantiate the game logic
            gameLogic = new GameLogic();

            // Instantiate the entity
            entity = gameLogic.CreatePlayer(_graphics);

            // Instantiate the keyboard input
            playerInput = new PlayerInput();

            // Load texture atlas and attachment loader
            Atlas atlas = new Atlas(entity.GetAtlas(), new XnaTextureLoader(GraphicsDevice));
            AtlasAttachmentLoader atlasAttachmentLoader = new AtlasAttachmentLoader(atlas);
            SkeletonJson json = new SkeletonJson(atlasAttachmentLoader);

            // Initialise skeleton json
            skeletonData = json.ReadSkeletonData(entity.GetJson());
            skeleton = new Skeleton(skeletonData);

            // Set the skin
            skin = skeletonData.FindSkin(entity.GetSkin());
            if (skin == null) throw new System.ArgumentException("Can't find skin: " + entity.GetSkin());
            skeleton.SetSkin(skin);
            skeleton.SetSlotsToSetupPose();

            // Setup animation
            AnimationStateData animStateData = new AnimationStateData(skeleton.Data);
            animState = new AnimationState(animStateData);
            animState.Apply(skeleton);

            // Set the "fidle" animation on track 1 and leave it looping forever
            animState.SetAnimation(0, entity.GetStartingAnim(), true);
        }

        /*
         * Update the player position, animation state and skeleton.
         */
        public void UpdatePlayerAnimations(
            GameTime gameTime,
            KeyboardState keyState,
            GraphicsDeviceManager _graphics,
            GraphicsDevice GraphicsDevice)
        {
            playerInput.ProcessKeyboardInputs(gameTime,
                keyState,
                animState,
                entity.GetSpeed(),
                entity.GetPositionX(),
                entity.GetPositionY());

            playerInput.ProcessJoypadInputs(gameTime, entity.GetSpeed());

            entity.SetPositionX(playerInput.UpdatePositionX());
            entity.SetPositionY(playerInput.UpdatePositionY());

            playerInput.CheckScreenEdges(_graphics,
                GraphicsDevice,
                entity.GetPositionX(),
                entity.GetPositionY());

            entity.SetPositionX(playerInput.UpdatePositionX());
            entity.SetPositionY(playerInput.UpdatePositionY());

            // Update the animation state and apply animations to skeletons
            skeleton.X = entity.GetPositionX();
            skeleton.Y = entity.GetPositionY();
            animState.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            skeleton.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            animState.Apply(skeleton);

            // Update skeletal transformations
            skeleton.UpdateWorldTransform(Skeleton.Physics.Update);
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
    }
}