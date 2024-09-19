using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoR;
using SoR.Logic.Entities;
using SoR.Logic.Input;
using Spine;
using System;
using System.Collections.Generic;

namespace Logic.Entities.Character.Mobs
{
    /*
     * Stores information unique to Pheasant.
     */
    internal class Pheasant : Entity
    {
        public Pheasant(GraphicsDevice GraphicsDevice)
        {
            // The possible animations to play as a string and the method to use for playing them as an int
            animations = new Dictionary<string, int>()
            {
                { "idle", 1 },
                { "hit", 2 },
                { "run", 1 }
            };

            // Load texture atlas and attachment loader
            atlas = new Atlas(Globals.GetPath("Content\\SoR Resources\\Entities\\Pheasant\\savedthepheasant.atlas"), new XnaTextureLoader(GraphicsDevice));
            //atlas = new Atlas("D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Pheasant\\savedthepheasant.atlas", new XnaTextureLoader(GraphicsDevice));
            atlasAttachmentLoader = new AtlasAttachmentLoader(atlas);
            json = new SkeletonJson(atlasAttachmentLoader);

            // Initialise skeleton json
            skeletonData = json.ReadSkeletonData(Globals.GetPath("Content\\SoR Resources\\Entities\\Pheasant\\skeleton.json"));
            //skeletonData = json.ReadSkeletonData("D:\\GitHub projects\\Proj-SoR\\Content\\Entities\\Pheasant\\skeleton.json");
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

            hitbox = new SkeletonBounds();

            random = new Random();
            movementDirection = new Vector2(0, 0); // The direction of movement
            newDirectionTime = (float)random.NextDouble() * 1f + 0.25f; // After 0.25-1 seconds, choose a new movement direction
            sinceLastChange = 0; // Time elapsed since last direction change
            NewDirection(random.Next(4)); // Choose a random new direction to move in

            inMotion = true; // Move freely

            movement = new Movement(); // Environmental collision handling

            Speed = 80f; // Set the entity's travel speed

            hitpoints = 100; // Set the starting number of hitpoints

            countDistance = 0;

            Height = 1;
        }
    }
}