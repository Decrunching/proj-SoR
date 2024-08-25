using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoR.Logic.Input;
using SoR.Logic.Spine;
using Spine;

namespace SoR.Logic.Entities
{
    /*
     * Stores information unique to the campfire entity.
     */
    internal class Campfire : NonPlayerEntity
    {
        private SpineSetUp spineSetUp;
        protected SkeletonRenderer skeletonRenderer;
        private PlayerInput playerInput;

        public Campfire(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice)
        {
            // Instantiate Spine skeletons and animations
            spineSetUp = new SpineSetUp(graphics, GraphicsDevice, GetAtlas(), GetJson(), GetSkin(), GetStartingAnim());

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
         * Get the Atlas path.
         */
        public override string GetAtlas()
        {
            return "F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Campfire\\templecampfire.atlas";
            //return "D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Campfire\\templecampfire.atlas";
        }

        /*
         * Get the json path.
         */
        public override string GetJson()
        {
            return "F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Campfire\\skeleton.json";
            //return "D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Campfire\\skeleton.json";
        }

        /*
         * Get the starting skin.
         */
        public override string GetSkin()
        {
            return "default";
        }

        /*
         * Get the starting animation.
         */
        public override string GetStartingAnim()
        {
            return "idle";
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
    }
}