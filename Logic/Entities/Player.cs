using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Spine;

namespace SoR.Logic.Entities
{
    internal class Player
    {
        protected AtlasAttachmentLoader atlasAttachmentLoader;
        protected Atlas atlas;
        protected SkeletonJson json;
        protected SkeletonData skeletonData;
        protected Skeleton skeleton;
        protected Skin skin;
        protected AnimationState animState;
        protected AnimationStateData animStateData;

        public Player(GraphicsDevice GraphicsDevice)
        {
            // Load texture atlas and attachment loader
            atlas = new Atlas("F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Player\\haltija.atlas", new XnaTextureLoader(GraphicsDevice));
            //atlas = new Atlas("F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Player\\skeleton.atlas", new XnaTextureLoader(GraphicsDevice));
            //atlas = new Atlas("D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Player\\skeleton.atlas", new XnaTextureLoader(game.GraphicsDevice));
            atlasAttachmentLoader = new AtlasAttachmentLoader(atlas);
            json = new SkeletonJson(atlasAttachmentLoader);

            // Initialise skeleton json to be loaded at 0.5x scale
            skeletonData = json.ReadSkeletonData("F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Player\\skeleton.json");
            //skeletonData = json.ReadSkeletonData("F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Player\\skeleton.json");
            //skeletonData = json.ReadSkeletonData("D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Player\\skeleton.json");
            skeleton = new Skeleton(skeletonData);

            //Add a new skin
            SetSkin("1");

            // Setup animation
            animStateData = new AnimationStateData(skeleton.Data);
            animState = new AnimationState(animStateData);

            // Set the "fidle" animation on track 1 and leave it looping forever
            skeleton.SetSkin(skin);
            skeleton.SetSlotsToSetupPose();
            animState.Apply(skeleton);
            animState.SetAnimation(0, "idle", true);

            // 0.2 seconds of mixing time between animation transitions
            animStateData.DefaultMix = 0.6f;
        }

        public void SetSkin(string skinName)
        {
            skin = skeletonData.FindSkin(skinName);
            if (skin == null) throw new System.ArgumentException("Can't find skin: " + skinName);
        }

        public void SetAnimState(KeyboardState keyState, KeyboardState lastKeyState)
        {
            if (keyState.IsKeyDown(Keys.Up) & !lastKeyState.IsKeyDown(Keys.Up))
            {
                animState.AddAnimation(0, "runup", true, 0);
            }

            if (keyState.IsKeyDown(Keys.Down) & !lastKeyState.IsKeyDown(Keys.Down))
            {
                animState.AddAnimation(0, "rundown", true, 0);
            }

            if (keyState.IsKeyDown(Keys.Left) & !lastKeyState.IsKeyDown(Keys.Left))
            {
                animState.AddAnimation(0, "runleft", true, 0);
                //skeleton.ScaleX = -1;
            }

            if (keyState.IsKeyDown(Keys.Right) & !lastKeyState.IsKeyDown(Keys.Right))
            {
                animState.AddAnimation(0, "runright", true, 0);
                //skeleton.ScaleX = 1;
            }
        }

        public void UpdateSkeletalAnimations(GameTime gameTime, Vector2 position)
        {
            // Update the animation state and apply animations to skeletons
            skeleton.X = position.X;
            skeleton.Y = position.Y;
            animState.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            skeleton.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            animState.Apply(skeleton);

            // Update skeletal transformations
            skeleton.UpdateWorldTransform(Skeleton.Physics.Update);
        }

        public Skeleton GetSkeleton()
        {
            return skeleton;
        }
    }
}
