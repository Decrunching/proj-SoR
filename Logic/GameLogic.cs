using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoR.Logic.Entities;

namespace SoR.Logic
{
    /*
     * Placeholder class for handling game progression.
     */
    public class GameLogic
    {
        private Entity playerChar;
        private Entity pheasant;
        private Entity chara;
        private Entity slime;
        private Entity campfire;
        private Entity entity;
        private SpineSetUp spineSetUp;
        private EntityType entityType;

        /*
         * Temporary Enums for differentiating between entities
         */
        enum EntityType
        {
            Player,
            NPCPheasant,
            NPCChara,
            EnemySlime,
            EnvironmentFire
        }

        public GameLogic(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice)
        {
            CreateEntity(graphics);
            spineSetUp = new SpineSetUp(graphics, GraphicsDevice, playerChar); // Instantiate Spine entities
            spineSetUp.CreateSkeletonRenderer(GraphicsDevice);  // Create the skeleton renderer
        }

        /*
         * Placeholder function for choosing which entity to create.
         */
        public void CreateEntity(GraphicsDeviceManager graphics)
        {
            EntityType entityType = EntityType.Player;

            switch (entityType)
            {
                case EntityType.Player:
                    playerChar = new PlayerChar(graphics);
                    break;
                case EntityType.NPCPheasant:
                    pheasant = new Pheasant(graphics);
                    break;
                case EntityType.NPCChara:
                    chara = new Chara(graphics);
                    break;
                case EntityType.EnemySlime:
                    slime = new Slime(graphics);
                    break;
                case EntityType.EnvironmentFire:
                    campfire = new Campfire(graphics);
                    break;
            }
        }

        /*
         * Get the appropriate entity.
         */
        public Entity GetEntity()
        {
            return playerChar;
        }

        /*
         * Set up Spine animations and skeletons.
         */
        public void SetUpSpine(
            GameTime gameTime,
            KeyboardState keyState,
            GraphicsDeviceManager graphics,
            GraphicsDevice GraphicsDevice)
        {
            spineSetUp.UpdateInputPosition(gameTime, keyState, graphics, GraphicsDevice); // Update entity positions according to user input
            spineSetUp.UpdateEntityAnimations(gameTime); // Update entity animations
        }

        /*
         * Render Spine skeletons.
         */
        public void SpineRenderSkeleton(GraphicsDevice GraphicsDevice)
        {
            spineSetUp.RenderSkeleton(GraphicsDevice); // Render the skeleton to the screen
        }
    }
}