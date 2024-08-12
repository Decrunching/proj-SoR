using Microsoft.Xna.Framework;
using Spine;
using System.Numerics;

namespace SoR.Logic
{
    internal class Player : Game1
    {
        private SkeletonRenderer skeletonRenderer;
        protected Atlas atlas;
        protected SkeletonJson json;
        protected SkeletonData skeletonData;
        protected Skeleton skeleton;
        protected AtlasAttachmentLoader attachmentLoader;
        protected BoneData root;
        protected AnimationState animState;
        protected AnimationStateData animStateData;
        protected Microsoft.Xna.Framework.Vector2 position;

        public Player(Game1 game)
        {
            position = new Microsoft.Xna.Framework.Vector2(game.GetGraphics().PreferredBackBufferWidth / 2,
                game.GetGraphics().PreferredBackBufferHeight / 2);

            // Initialise skeleton renderer with premultiplied alpha
            skeletonRenderer = new SkeletonRenderer(game.GetGraphicsDevice());
            skeletonRenderer.PremultipliedAlpha = true;
        }

        public SkeletonRenderer GetSkeletonRenderer()
        {
            return skeletonRenderer;
        }

        public void SetupSkeleton()
        {

            // Load texture atlas and attachment loader
            atlas = new Atlas("C:\\Users\\Kat\\Dropbox\\work\\IT\\CS\\SpineProject\\SpineProject\\Project\\Content\\sprites\\player\\char\\skeleton.atlas", new XnaTextureLoader(game.GraphicsDevice));
            attachmentLoader = new AtlasAttachmentLoader(atlas);
            json = new SkeletonJson(attachmentLoader);

            // Initialise skeleton json to be loaded at 0.5x scale
            skeletonData = json.ReadSkeletonData("C:\\Users\\Kat\\Dropbox\\work\\IT\\CS\\SpineProject\\SpineProject\\Project\\Content\\sprites\\player\\char\\skeleton.json");
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
