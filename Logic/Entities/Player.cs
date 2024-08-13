using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spine;
using System.Numerics;

namespace SoR.Logic.Entities
{
    internal class Player : Chara
    {
        private AtlasAttachmentLoader atlasAttachmentLoader;
        private Atlas atlas;
        private SkeletonJson json;
        protected SkeletonData skeletonData;
        protected Skeleton skeleton;
        protected AnimationState animState;
        protected AnimationStateData animStateData;

        public Player(Game1 game) : base(game)
        {
            // Load texture atlas and attachment loader
            atlas = new Atlas("F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Player\\skeleton.atlas", new XnaTextureLoader(game.GraphicsDevice));
            atlasAttachmentLoader = new AtlasAttachmentLoader(atlas);
            json = new SkeletonJson(atlasAttachmentLoader);

            // Initialise skeleton json to be loaded at 0.5x scale
            skeletonData = json.ReadSkeletonData("F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Player\\skeleton.json");
            skeleton = new Skeleton(skeletonData);

            // Setup animation
            animStateData = new AnimationStateData(skeleton.Data);
            animState = new AnimationState(animStateData);

            // Set the skeleton position to centre of screen
            skeleton.X = position.X;
            skeleton.Y = position.Y;

            // Set the "fidle" animation on track 1 and leave it looping forever
            animState.SetAnimation(0, "fdownidle", true);

            // 0.2 seconds of mixing time between animation transitions
            animStateData.DefaultMix = 0.2f;
        }

        public override void Render(GameTime gameTime, Game1 game)
        {
            // Update the animation state and apply animations to skeletons
            animState.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            skeleton.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            animState.Apply(skeleton);

            // Update skeletal transformations
            skeleton.UpdateWorldTransform(Skeleton.Physics.Update);

            // Clear the screen and set up the skeleton renderer's projection matrix
            game.GraphicsDevice.Clear(Color.CornflowerBlue);
            ((BasicEffect)GetSkeletonRenderer().Effect).Projection = Matrix.CreateOrthographicOffCenter(
                0,
                game.GraphicsDevice.Viewport.Width,
                game.GraphicsDevice.Viewport.Height,
                0, 1, 0);

            // Draw skeletons
            GetSkeletonRenderer().Begin();
            GetSkeletonRenderer().Draw(skeleton);
            GetSkeletonRenderer().End();
        }
    }
}
