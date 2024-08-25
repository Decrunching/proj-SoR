using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoR.Logic.Entities;

namespace SoR.Logic.Game
{
    /*
     * Placeholder class for handling game progression.
     */
    public class GameLogic
    {
        private PlayerEntity playerChar;
        private NonPlayerEntity pheasant;
        private NonPlayerEntity chara;
        private NonPlayerEntity slime;
        private NonPlayerEntity campfire;
        private EntityType entityType;
        private bool gameStart;

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

        /*
         * Constructor for initial game setup.
         */
        public GameLogic(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice)
        {
            gameStart = true;

            entityType = EntityType.Player;
            CreateEntity(graphics, GraphicsDevice);


            /*entityType = EntityType.EnemySlime;
            CreateEntity(graphics);*/

        }

        /*
         * Placeholder function for choosing which entity to create. Only use for permanent
         * entities - transient entities are fine being transient.
         */
        public void CreateEntity(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice)
        {
            switch (entityType)
            {
                case EntityType.Player:
                    playerChar = new PlayerChar(graphics, GraphicsDevice);
                    break;
                case EntityType.NPCPheasant:
                    pheasant = new Pheasant(graphics, GraphicsDevice);
                    break;
                case EntityType.NPCChara:
                    chara = new Chara(graphics, GraphicsDevice);
                    break;
                case EntityType.EnemySlime:
                    slime = new Slime(graphics, GraphicsDevice);
                    break;
                case EntityType.EnvironmentFire:
                    campfire = new Campfire(graphics, GraphicsDevice);
                    break;
            }
        }

        /*
         * Get a permanent entity.
         */
        public PlayerEntity GetPlayerEntity(GraphicsDeviceManager graphics)
        {
            if (entityType == EntityType.Player)
            {
                return playerChar;
            }
            else throw new System.EntryPointNotFoundException("No valid entity type received");
        }

        /*
         * Get a permanent entity.
         */
        public NonPlayerEntity GetNonPlayerEntity(GraphicsDeviceManager graphics)
        {
            if (entityType == EntityType.NPCPheasant)
            {
                return pheasant;
            }
            else if (entityType == EntityType.NPCChara)
            {
                return chara;
            }
            else if (entityType == EntityType.EnemySlime)
            {
                return slime;
            }
            else if (entityType == EntityType.EnvironmentFire)
            {
                return campfire;
            }
            else throw new System.EntryPointNotFoundException("No valid entity type received");
        }

        /*
         * Render Spine skeletons.
         */
        public void SpineRenderSkeleton(GraphicsDevice GraphicsDevice)
        {
            playerChar.RenderSkeleton(GraphicsDevice); // Render the skeleton to the screen
        }

        public void UpdateSkeletonAndPosition(
            GameTime gameTime,
            KeyboardState keyState,
            GraphicsDeviceManager graphics,
            GraphicsDevice GraphicsDevice)
        {
            playerChar.UpdatePositionAndAnimation(gameTime, keyState, graphics, GraphicsDevice);
        }
    }
}