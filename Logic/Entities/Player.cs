using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Spine;

namespace SoR.Logic.Entities
{
    internal class Player : Game1, Chara
    {
        private SkeletonRenderer skeletonRenderer;
        private AtlasAttachmentLoader atlasAttachmentLoader;
        private Atlas atlas;
        private SkeletonJson json;
        protected SkeletonData skeletonData;
        protected Skeleton skeleton;
        protected AnimationState animState;
        protected AnimationStateData animStateData;

        public Player(Game1 game)
        {
            // Initialise skeleton renderer with premultiplied alpha
            skeletonRenderer = new SkeletonRenderer(game.GetGraphicsDevice(game));
            skeletonRenderer.PremultipliedAlpha = true;

            // Load texture atlas and attachment loader
            atlas = new Atlas("F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Player\\skeleton.atlas", new XnaTextureLoader(game.GraphicsDevice));
            //atlas = new Atlas("D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Player\\skeleton.atlas", new XnaTextureLoader(game.GraphicsDevice));
            atlasAttachmentLoader = new AtlasAttachmentLoader(atlas);
            json = new SkeletonJson(atlasAttachmentLoader);

            // Initialise skeleton json to be loaded at 0.5x scale
            skeletonData = json.ReadSkeletonData("F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Player\\skeleton.json");
            //skeletonData = json.ReadSkeletonData("D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Player\\skeleton.json");
            skeleton = new Skeleton(skeletonData);

            // Setup animation
            animStateData = new AnimationStateData(skeleton.Data);
            animState = new AnimationState(animStateData);

            // Set the "fidle" animation on track 1 and leave it looping forever
            animState.SetAnimation(0, "fdownidle", true);

            // 0.2 seconds of mixing time between animation transitions
            animStateData.DefaultMix = 0.2f;
        }

        public SkeletonRenderer GetSkeletonRenderer()
        {
            return skeletonRenderer;
        }

        public AnimationState GetAnimState()
        {
            return animState;
        }

        public Skeleton GetSkeleton()
        {
            return skeleton;
        }

        public void UpdateAnimations()
        {
            //Anims: fdown, fdownidle, fside, fsideidle, fup, fupidle, mdown, mdownidle, mside, msideidle, mup, mupidle
            if (GetKeyState().IsKeyDown(Keys.Up) & !GetLastKeyState().IsKeyDown(Keys.Up))
            {
                animState.AddAnimation(0, "fup", true, 0);
            }

            if (GetKeyState().IsKeyDown(Keys.Up) & !GetLastKeyState().IsKeyDown(Keys.Down))
            {
                animState.AddAnimation(0, "fdown", true, 0);
            }

            if (GetKeyState().IsKeyDown(Keys.Up) & !GetLastKeyState().IsKeyDown(Keys.Left))
            {
                animState.AddAnimation(0, "fside", true, 0);
                skeleton.ScaleX = -1;
            }

            if (GetKeyState().IsKeyDown(Keys.Up) & !GetLastKeyState().IsKeyDown(Keys.Right))
            {
                animState.AddAnimation(0, "fside", true, 0);
                skeleton.ScaleX = 1;
            }
            /*
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
            }*/
        }

        public void Render(GameTime gameTime, Game1 game)
        {
            // Update the animation state and apply animations to skeletons
            skeleton.X = game.GetPositionX();
            skeleton.Y = game.GetPositionY();
            animState.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            skeleton.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            animState.Apply(skeleton);

            // Update skeletal transformations
            skeleton.UpdateWorldTransform(Skeleton.Physics.Update);

            // Clear the screen and set up the skeleton renderer's projection matrix
            GetGraphicsDevice(game).Clear(Color.DarkSeaGreen);
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
