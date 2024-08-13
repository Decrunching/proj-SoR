using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Spine;

namespace SoR.Logic.Entities
{
    internal class Player : Input, Chara
    {
        private SkeletonRenderer skeletonRenderer;
        private AtlasAttachmentLoader atlasAttachmentLoader;
        private Atlas atlas;
        private SkeletonJson json;
        protected SkeletonData skeletonData;
        protected Skeleton skeleton;
        protected AnimationState animState;
        protected AnimationStateData animStateData;

        public Player(Game1 game) : base(game)
        {
            // Initialise skeleton renderer with premultiplied alpha
            skeletonRenderer = new SkeletonRenderer(game.GetGraphicsDevice(game));
            skeletonRenderer.PremultipliedAlpha = true;

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

        public SkeletonRenderer GetSkeletonRenderer()
        {
            return skeletonRenderer;
        }
        public void UpdateAnimations(GameTime gameTime, Game1 game)
        {
            //Anims: fdown, fdownidle, fside, fsideidle, fup, fupidle, mdown, mdownidle, mside, msideidle, mup, mupidle
            if (!keyPressed)
            {
                switch (lastKey)
                {
                    case "up":
                        animState.SetAnimation(0, "fupidle", true);
                        break;
                    case "down":
                        animState.SetAnimation(0, "fdownidle", true);
                        break;
                    case "left":
                        skeleton.ScaleX = -1;
                        animState.SetAnimation(0, "fsideidle", true);
                        break;
                    case "right":
                        skeleton.ScaleX = 1;
                        animState.SetAnimation(0, "fsideidle", true);
                        break;
                }
            }
            else if (keyPressed)
            {
                switch (lastKey)
                {
                    case "up":
                        animState.AddAnimation(0, "fup", true, 0);
                        break;
                    case "down":
                        animState.AddAnimation(0, "fdown", true, 0);
                        break;
                    case "left":
                        animState.AddAnimation(0, "fside", true, 0);
                        skeleton.ScaleX = -1;
                        break;
                    case "right":
                        animState.AddAnimation(0, "fside", true, 0);
                        skeleton.ScaleX = 1;
                        break;
                }
            }

            if (position.X > screenWidth - skeletonData.Width)
            {
                position.X = screenWidth - skeletonData.Width;
            }
            else if (position.X < skeletonData.Width)
            {
                position.X = skeletonData.Width;
            }

            if (position.Y > screenHeight + skeletonData.Height / 3)
            {
                position.Y = screenHeight + skeletonData.Height / 3;
            }
            else if (position.Y < skeletonData.Height * 3)
            {
                position.Y = skeletonData.Height * 3;
            }
        }

        public void Render(GameTime gameTime, Game1 game)
        {
            UpdateAnimations(gameTime, game);

            // Update the animation state and apply animations to skeletons
            animState.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            skeleton.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            animState.Apply(skeleton);

            // Update skeletal transformations
            skeleton.UpdateWorldTransform(Skeleton.Physics.Update);

            // Clear the screen and set up the skeleton renderer's projection matrix
            GetGraphicsDevice(game).Clear(Color.CornflowerBlue);
            ((BasicEffect)GetSkeletonRenderer().Effect).Projection = Matrix.CreateOrthographicOffCenter(
                0,
                GetGraphicsDevice(game).Viewport.Width,
                GetGraphicsDevice(game).Viewport.Height,
                0, 1, 0);

            // Draw skeletons
            GetSkeletonRenderer().Begin();
            GetSkeletonRenderer().Draw(skeleton);
            GetSkeletonRenderer().End();
        }
    }
}
