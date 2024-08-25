using Spine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SoR.Logic.Spine
{
    /*
     * Used to create and update Spine skeletons, skins and animations.
     */
    public class SpineSetUp
    {
        private Atlas atlas;
        private AtlasAttachmentLoader atlasAttachmentLoader;
        private SkeletonJson json;
        private SkeletonData skeletonData;
        private Skeleton skeleton;
        private Skin skin;

        /*
         * Constructor for instantiating entities and Spine animations.
         */
        public SpineSetUp(GraphicsDeviceManager graphics,
            GraphicsDevice GraphicsDevice,
            string atlasPath,
            string jsonPath,
            string skinPath,
            string idlePath)
        {
            // Load texture atlas and attachment loader
            atlas = new Atlas(atlasPath, new XnaTextureLoader(GraphicsDevice)); // Create the atlas
            atlasAttachmentLoader = new AtlasAttachmentLoader(atlas);
            json = new SkeletonJson(atlasAttachmentLoader);

            // Initialise skeleton json
            skeletonData = json.ReadSkeletonData(jsonPath);
            skeleton = new Skeleton(skeletonData);

            // Set the skin
            skin = skeletonData.FindSkin(skinPath);
            if (skin == null) throw new System.ArgumentException("Can't find skin: " + skinPath);
            skeleton.SetSkin(skin);
        }

        /*
         * Get the skeleton's position.
         */
        public Skeleton GetSkeleton()
        {
            return skeleton;
        }

        /*
         * Set the position of the skeleton according to player input.
         */
        public void SetSkeleton(float positionX, float positionY)
        {
            skeleton.X = positionX;
            skeleton.Y = positionY;
        }

        /*
         * Update the entity position, animation state and skeleton.
         */
        public void UpdateEntitySkeleton(GameTime gameTime, AnimationState animState)
        {
            animState.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            skeleton.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            animState.Apply(skeleton);

            // Update skeletal transformations
            skeleton.UpdateWorldTransform(Skeleton.Physics.Update);
        }
    }
}