using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoR.Logic.Entities;
using System.Collections.Generic;

namespace SoR.Logic.Game
{
    /*
     * Placeholder class for handling game progression.
     */
    public class GameLogic
    {
        private Dictionary<string, Entity> entities;
        private EntityType entityType;

        /*
         * Temporary Enums for differentiating between entities
         */
        enum EntityType
        {
            Player,
            Pheasant,
            Chara,
            Slime,
            Fire
        }

        static void playerCharRef(ref Entity playerChar) { }

        /*
         * Constructor for initial game setup.
         */
        public GameLogic(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice)
        {
            entities = new Dictionary<string, Entity>();

            entityType = EntityType.Player;
            CreateEntity(graphics, GraphicsDevice);

            /*entityType = EntityType.Slime;
            CreateEntity(graphics, GraphicsDevice);

            entityType = EntityType.Chara;
            CreateEntity(graphics, GraphicsDevice);

            entityType = EntityType.Pheasant;
            CreateEntity(graphics, GraphicsDevice);

            entityType = EntityType.Fire;
            CreateEntity(graphics, GraphicsDevice);*/
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
                    entities.Add("player", new Player(graphics, GraphicsDevice) { Name = "player" });
                    break;
                case EntityType.Pheasant:
                    entities.Add("pheasant", new Pheasant(graphics, GraphicsDevice) { Name = "pheasant" });
                    break;
                case EntityType.Chara:
                    entities.Add("chara", new Chara(graphics, GraphicsDevice) { Name = "chara" });
                    break;
                case EntityType.Slime:
                    entities.Add("slime", new Slime(graphics, GraphicsDevice) { Name = "slime" });
                    break;
                case EntityType.Fire:
                    entities.Add("fire", new Campfire(graphics, GraphicsDevice) { Name = "fire" });
                    break;
            }
        }

        /*
         * Render Spine skeletons.
         */
        public void SpineRenderSkeleton(GraphicsDevice GraphicsDevice)
        {
            foreach (var entity in entities)
            {
                entity.Value.RenderSkeleton(GraphicsDevice); // Render each skeleton to the screen
            }
        }

        /*
         * Set up Spine animations and skeletons.
         */
        public void UpdateEntities(
            GameTime gameTime,
            KeyboardState keyState,
            KeyboardState lastKeyState,
            GraphicsDeviceManager graphics,
            GraphicsDevice GraphicsDevice)
        {
            if (entities.TryGetValue("player", out Entity playerChar))
            {
                if (playerChar is Player player)
                {
                    // Update position according to user input
                    player.UpdateEntityPosition(
                    gameTime,
                    keyState,
                    lastKeyState,
                    graphics,
                    GraphicsDevice,
                    player.GetAnimState(),
                    player.GetSkeleton());
                }
                else
                {
                    // Throw exception if playerChar is somehow not of the type Player
                    throw new System.InvalidOperationException("playerChar is not of type Player");
                }

                foreach (var entity in entities)
                {
                    // Update animations
                    entity.Value.UpdateEntityAnimations(gameTime);
                }

            }
        }
    }
}