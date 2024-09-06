using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Spine;

namespace Logic.Game.GameMap.TiledScenery
{
    /*
     * Stores information unique to Campfire.
     */
    internal class Grass : Scenery
    {
        public Grass(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice)
        {


            // Load texture atlas and attachment loader
            atlas = new Atlas(GetPath("Content\\SoR Resources\\Locations\\TiledScenery\\Grass\\grasstiles.atlas"), new XnaTextureLoader(GraphicsDevice));
            //atlas = new Atlas("D:\\GitHub projects\\Proj-SoR\\Content\\Scenery\\Grass\\skeleton.atlas", new XnaTextureLoader(GraphicsDevice));
            atlasAttachmentLoader = new AtlasAttachmentLoader(atlas);
            json = new SkeletonJson(atlasAttachmentLoader);

            // Initialise skeleton json
            skeletonData = json.ReadSkeletonData(GetPath("Content\\SoR Resources\\Locations\\TiledScenery\\Grass\\skeleton.json"));
            //skeletonData = json.ReadSkeletonData("D:\\GitHub projects\\Proj-SoR\\Content\\Scenery\\Grass\\skeleton.json");
            skeleton = new Skeleton(skeletonData);

            // Set the skin
            skeleton.SetSkin(skeletonData.FindSkin("default"));

            // Setup animation
            animStateData = new AnimationStateData(skeleton.Data);
            animState = new AnimationState(animStateData);
            animState.Apply(skeleton);
            animStateData.DefaultMix = 0.1f;

            // Set the "fidle" animation on track 1 and leave it looping forever
            //trackEntry = animState.SetAnimation(0, "idle", true);

            // Create hitbox
            //slot = skeleton.FindSlot("hitbox");
            //hitboxAttachment = skeleton.GetAttachment("hitbox", "hitbox");
            //slot.Attachment = hitboxAttachment;
            //skeleton.SetAttachment("hitbox", "hitbox");

            // Initialise skeleton renderer with premultiplied alpha
            skeletonRenderer = new SkeletonRenderer(GraphicsDevice);
            skeletonRenderer.PremultipliedAlpha = true;

            hitbox = new SkeletonBounds();
        }

        /*
         * For setting the player position relative to the map.
         */
        public Vector2 RelativePosition()
        {
            return position;
        }

        /*
         * Placeholder for Grass animation changes.
         */
        public override void ChangeAnimation(string eventTrigger) { }
    }
}
