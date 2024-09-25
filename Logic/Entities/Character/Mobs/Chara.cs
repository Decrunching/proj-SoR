using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoR;
using SoR.Logic.Entities;
using SoR.Logic.Input;
using Spine;
using System.Collections.Generic;

namespace Logic.Entities.Character.Mobs
{
    /*
     * Stores information unique to Chara.
     */
    internal class Chara : Entity
    {
        public Chara(GraphicsDevice GraphicsDevice, List<Rectangle> impassableArea)
        {
            // The possible animations to play as a string and the method to use for playing them as an int
            animations = new Dictionary<string, int>()
            {
                { "idle", 1 },
                { "hit", 2 },
                { "attack", 2 },
                { "run", 1 },
                { "jump", 2 }
            };

            // Load texture atlas and attachment loader
            atlas = new Atlas(Globals.GetPath("Content\\SoR Resources\\Entities\\Chara\\savedit.atlas"), new XnaTextureLoader(GraphicsDevice));
            //atlas = new Atlas("D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Chara\\savedit.atlas", new XnaTextureLoader(GraphicsDevice));
            atlasAttachmentLoader = new AtlasAttachmentLoader(atlas);
            json = new SkeletonJson(atlasAttachmentLoader);

            // Initialise skeleton json
            skeletonData = json.ReadSkeletonData(Globals.GetPath("Content\\SoR Resources\\Entities\\Chara\\skeleton.json"));
            //skeletonData = json.ReadSkeletonData("D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Chara\\skeleton.json");
            skeleton = new Skeleton(skeletonData);

            // Set the skin
            skeleton.SetSkin(skeletonData.FindSkin("default"));

            // Setup animation
            animStateData = new AnimationStateData(skeleton.Data);
            animState = new AnimationState(animStateData);
            animState.Apply(skeleton);
            animStateData.DefaultMix = 0.1f;

            // Set the "fidle" animation on track 1 and leave it looping forever
            trackEntry = animState.SetAnimation(0, "run", true);

            // Create hitbox
            slot = skeleton.FindSlot("hitbox");
            hitboxAttachment = skeleton.GetAttachment("hitbox", "hitbox");
            slot.Attachment = hitboxAttachment;
            skeleton.SetAttachment("hitbox", "hitbox");

            // Initialise skeleton renderer with premultiplied alpha
            skeletonRenderer = new SkeletonRenderer(GraphicsDevice);
            skeletonRenderer.PremultipliedAlpha = true;

            hitbox = new SkeletonBounds();
            hitbox.Update(skeleton, true);

            inMotion = true; // Move freely
            Player = false;

            movement = new Movement(); // Environmental collision handling

            Speed = 80f; // Set the entity's travel speed
            hitpoints = 100; // Set the starting number of hitpoints

            ImpassableArea = impassableArea;
        }
    }
}