using Spine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoR.Logic.Input;
using SoR.Logic.Entities;

namespace SoR.Logic
{
    /*
     * Used to create and update Spine skeletons, skins and animations.
     */
    public class SpineSetUp
    {
        private SkeletonData skeletonData;
        private SkeletonRenderer skeletonRenderer;
        public Skeleton skeleton { get; private set; }
        public Skin skin { get; private set; }
        public AnimationState animState { get; set; }
        private PlayerInput playerInput;
        private GameLogic gameLogic;
        private Entity entity;

        /*
         * Constructor for instantiating entities and Spine animations.
         */
        public SpineSetUp(GraphicsDeviceManager _graphics, GraphicsDevice GraphicsDevice)
        {
            // Instantiate the game logic
            gameLogic = new GameLogic();

            // Instantiate the entity
            entity = gameLogic.StartGame(_graphics);

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
         * Update entity position according to player input.
         */
        public void UpdateInputPosition(
            GameTime gameTime,
            KeyboardState keyState,
            GraphicsDeviceManager _graphics,
            GraphicsDevice GraphicsDevice)
        {
            // Pass the speed, position and animation state to PlayerInput for keyboard input processing
            playerInput.ProcessKeyboardInputs(gameTime,
                keyState,
                animState,
                entity.GetSpeed(),
                entity.GetPositionX(),
                entity.GetPositionY());

            // Pass the speed to PlayerInput for joypad input processing
            playerInput.ProcessJoypadInputs(gameTime, entity.GetSpeed());

            // Set the new position according to player input
            entity.SetPositionX(playerInput.UpdatePositionX());
            entity.SetPositionY(playerInput.UpdatePositionY());

            // Prevent the user from leaving the visible screen area
            playerInput.CheckScreenEdges(_graphics,
                GraphicsDevice,
                entity.GetPositionX(),
                entity.GetPositionY());

            // Set the new position according to player input
            entity.SetPositionX(playerInput.UpdatePositionX());
            entity.SetPositionY(playerInput.UpdatePositionY());

            // Update the animation state and apply animations to skeletons
            skeleton.X = entity.GetPositionX();
            skeleton.Y = entity.GetPositionY();
        }

        /*
         * Update the entity position, animation state and skeleton.
         */
        public void UpdateEntityAnimations(GameTime gameTime)
        {
            animState.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            skeleton.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            animState.Apply(skeleton);

            // Update skeletal transformations
            skeleton.UpdateWorldTransform(Skeleton.Physics.Update);
        }

        /*
         * Create the SkeletonRenderer.
         */
        public void CreateSkeletonRenderer(GraphicsDevice GraphicsDevice)
        {
            // Initialise skeleton renderer with premultiplied alpha
            skeletonRenderer = new SkeletonRenderer(GraphicsDevice);
            skeletonRenderer.PremultipliedAlpha = true;
        }

        /*
         * Render the current skeleton to the screen.
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