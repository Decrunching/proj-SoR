using Microsoft.Xna.Framework;
using SoR.Logic.Scenes;
using Spine;

namespace SoR.Logic
{
    internal class Player : Chara
    {
        protected Atlas atlas;
        protected SkeletonJson json;
        protected SkeletonData skeletonData;
        protected Skeleton skeleton;
        protected BoneData root;
        protected AnimationState animState;
        protected AnimationStateData animStateData;

        public Player(Game1 game) : base(game)
        {
        }

        public void CreateSkeleton(Game1 game)
        {

            // Load texture atlas and attachment loader
            atlas = new Atlas("F:\\MonoGame\\SoR\\SoR\\Content\\skeleton.atlas", new XnaTextureLoader(game.GraphicsDevice));
            json = new SkeletonJson(atlas);

            // Initialise skeleton json to be loaded at 0.5x scale
            skeletonData = json.ReadSkeletonData("F:\\MonoGame\\SoR\\SoR\\Content\\skeleton.json");
            skeleton = new Skeleton(skeletonData);

            // Setup animation
            animStateData = new AnimationStateData(skeleton.Data);
            animState = new AnimationState(animStateData);

            // Scale skeleton to 4x image size
            root = skeletonData.FindBone("root");
            root.ScaleX = 4;
            root.ScaleY = 4;

            // Set the skeleton position to centre of screen
            skeleton.X = position.X;
            skeleton.Y = position.Y;

            // Set the "fidle" animation on track 1 and leave it looping forever
            animState.SetAnimation(0, "fdownidle", true);

            // 0.2 seconds of mixing time between animation transitions
            animStateData.DefaultMix = 0.2f;
        }

        public void DrawSkeletons()
        {
            // Draw skeletons
            GetSkeletonRenderer().Begin();
            GetSkeletonRenderer().Draw(skeleton);
            GetSkeletonRenderer().End();
        }
    }
}
