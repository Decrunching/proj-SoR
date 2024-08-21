using Spine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoR.Logic.Input;

namespace SoR.Logic.Entities
{
    /*
     * The Player object class. Used to create and update the Player object. Currently includes
     * initialisation, position/movement and animation.
     */
    internal class Player
    {
        private SkeletonData skeletonData;
        private Skeleton skeleton;
        private Skin skin;
        private AnimationState animState;
        private SkeletonRenderer skeletonRenderer;

        private PlayerInput keyboardInput;

        /*
         * Constructor for creating the player object.
         */
        public Player(GraphicsDeviceManager _graphics, GraphicsDevice GraphicsDevice)
        {
            // Instantiate the keyboard input
            keyboardInput = new PlayerInput(_graphics);

            // Load texture atlas and attachment loader
            Atlas atlas = new Atlas("F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Player\\haltija.atlas", new XnaTextureLoader(GraphicsDevice));
            //Atlas atlas = new Atlas("D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Player\\haltija.atlas", new XnaTextureLoader(GraphicsDevice));
            AtlasAttachmentLoader atlasAttachmentLoader = new AtlasAttachmentLoader(atlas);
            SkeletonJson json = new SkeletonJson(atlasAttachmentLoader);

            // Initialise skeleton json
            skeletonData = json.ReadSkeletonData("F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Player\\skeleton.json");
            //skeletonData = json.ReadSkeletonData("D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Player\\skeleton.json");
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
         * Prevent the player from leaving the visible screen area.
         */
        public void CheckScreenEdges(GraphicsDeviceManager _graphics, GraphicsDevice GraphicsDevice)
        {
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
         * Update the player position, animation state and skeleton.
         */
        public void UpdatePlayerAnimations(GameTime gameTime, KeyboardState keyState, GraphicsDeviceManager _graphics, GraphicsDevice GraphicsDevice)
        {
            keyboardInput.ProcessKeyboardInputs(gameTime, keyState, animState);
            keyboardInput.ProcessJoypadInputs(gameTime);
            keyboardInput.CheckScreenEdges(_graphics, GraphicsDevice);

            // Update the animation state and apply animations to skeletons
            skeleton.X = keyboardInput.GetPositionX();
            skeleton.Y = keyboardInput.GetPositionY();
            animState.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            skeleton.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            animState.Apply(skeleton);

            // Update skeletal transformations
            skeleton.UpdateWorldTransform(Skeleton.Physics.Update);
        }
    }
}