﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Spine;

namespace Logic.Game.GameMap.Interactables
{
    /*
     * Stores information unique to Campfire.
     */
    internal class Campfire : Scenery
    {
        public Campfire(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice)
        {
            // Load texture atlas and attachment loader
            atlas = new Atlas(GetPath("Content\\SoR Resources\\Locations\\Interactables\\Campfire\\templecampfire.atlas"), new XnaTextureLoader(GraphicsDevice));
            //atlas = new Atlas("D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Campfire\\templecampfire.atlas", new XnaTextureLoader(GraphicsDevice));
            atlasAttachmentLoader = new AtlasAttachmentLoader(atlas);
            json = new SkeletonJson(atlasAttachmentLoader);

            // Initialise skeleton json
            skeletonData = json.ReadSkeletonData(GetPath("Content\\SoR Resources\\Locations\\Interactables\\Campfire\\skeleton.json"));
            //skeletonData = json.ReadSkeletonData("D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Campfire\\skeleton.json");
            skeleton = new Skeleton(skeletonData);

            // Set the skin
            skeleton.SetSkin(skeletonData.FindSkin("default"));

            // Setup animation
            animStateData = new AnimationStateData(skeleton.Data);
            animState = new AnimationState(animStateData);
            animState.Apply(skeleton);
            animStateData.DefaultMix = 0.1f;

            // Set the "fidle" animation on track 1 and leave it looping forever
            trackEntry = animState.SetAnimation(0, "idle", true);

            // Create hitbox
            slot = skeleton.FindSlot("hitbox");
            hitboxAttachment = skeleton.GetAttachment("hitbox", "hitbox");
            slot.Attachment = hitboxAttachment;
            skeleton.SetAttachment("hitbox", "hitbox");

            // Initialise skeleton renderer with premultiplied alpha
            skeletonRenderer = new SkeletonRenderer(GraphicsDevice);
            skeletonRenderer.PremultipliedAlpha = true;

            hitbox = new SkeletonBounds();
        }

        /*
         * Placeholder for Campfire animation changes.
         */
        public override void ChangeAnimation(string eventTrigger) { }
    }
}