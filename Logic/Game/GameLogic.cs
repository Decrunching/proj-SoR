using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoR.Logic.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SoR.Logic.Game
{
    /*
     * Placeholder class for handling game progression.
     */
    public class GameLogic
    {
        private Dictionary<string, Entity> entities;
        private EntityType entityType;
        private Vector2 centreScreen;
        private SpriteBatch spriteBatch;
        private SpriteFont font;

        /*
         * Enums for differentiating between entities.
         */
        enum EntityType
        {
            Player,
            Pheasant,
            Chara,
            Slime,
            Fire
        }

        /*
         * Constructor for initial game setup.
         */
        public GameLogic(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice, SoR game)
        {
            // Initialise SpriteBatch
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Font used for drawing text
            font = game.Content.Load<SpriteFont>("Fonts/File");

            // Find the centre of the game window
            centreScreen = new Vector2(graphics.PreferredBackBufferWidth / 2,
                graphics.PreferredBackBufferHeight / 2);

            // Create dictionary for storing entities as values with string labels for keys
            entities = new Dictionary<string, Entity>();

            // Create the Player entity
            entityType = EntityType.Player;
            CreateEntity(graphics, GraphicsDevice);

            // Create the Slime entity
            entityType = EntityType.Slime;
            CreateEntity(graphics, GraphicsDevice);

            // Create the Chara entity
            entityType = EntityType.Chara;
            CreateEntity(graphics, GraphicsDevice);

            // Create the Pheasant entity
            entityType = EntityType.Pheasant;
            CreateEntity(graphics, GraphicsDevice);

            // Create the Campfire entity
            //entityType = EntityType.Fire;
            //CreateEntity(graphics, GraphicsDevice);
        }

        /*
         * Choose entity to create and place it as a value in the entities dictionary with
         * a unique string identifier as a key.
         */
        public void CreateEntity(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice)
        {
            switch (entityType)
            {
                case EntityType.Player:
                    entities.Add("player", new Player(graphics, GraphicsDevice) { Name = "player", Render = true });
                    if (entities.TryGetValue("player", out Entity player))
                    {
                        player.SetStartPosition(centreScreen);
                    }
                    break;
                case EntityType.Pheasant:
                    entities.Add("pheasant", new Pheasant(graphics, GraphicsDevice) { Name = "pheasant", Render = true });
                    if (entities.TryGetValue("pheasant", out Entity pheasant))
                    {
                        pheasant.SetStartPosition(centreScreen);
                    }
                    break;
                case EntityType.Chara:
                    entities.Add("chara", new Chara(graphics, GraphicsDevice) { Name = "chara", Render = true });
                    if (entities.TryGetValue("chara", out Entity chara))
                    {
                        chara.SetStartPosition(centreScreen);
                    }
                    break;
                case EntityType.Slime:
                    entities.Add("slime", new Slime(graphics, GraphicsDevice) { Name = "slime", Render = true });
                    if (entities.TryGetValue("slime", out Entity slime))
                    {
                        slime.SetStartPosition(centreScreen);
                    }
                    break;
                // TO DO: Fire to move into separate environmental entity dictionary
                case EntityType.Fire:
                    entities.Add("fire", new Campfire(graphics, GraphicsDevice) { Name = "fire", Render = true });
                    if (entities.TryGetValue("fire", out Entity fire))
                    {
                        fire.SetStartPosition(centreScreen);
                    }
                    break;
            }
        }

        /*
         * Render Spine skeletons, and ensure entities that appear further down on the screen are in front
         * of those that are higher up.
         */
        public void SpineRenderSkeleton(GraphicsDevice GraphicsDevice)
        {
            // Sort entities by their y-axis position
            var sortByYAxis = entities.Values.OrderBy(entity => entity.GetHitbox().MaxY);

            foreach (var entity in sortByYAxis)
            {
                if (entity.Render)
                {
                    entity.RenderSkeleton(GraphicsDevice); // Render each skeleton to the screen
                    entity.DrawText(spriteBatch, font); // Draw any text associated with entity
                }
            }
        }

        /*
         * Update Spine animations and skeletons.
         */
        public void UpdateEntities(
            GameTime gameTime,
            GraphicsDeviceManager graphics,
            GraphicsDevice GraphicsDevice)
        {
            foreach (var entity in entities)
            {
                if (entity.Value.Render)
                {
                    entity.Value.Movement(gameTime);

                    // Update position according to user input
                    entity.Value.UpdatePosition(
                    gameTime,
                    graphics,
                    GraphicsDevice);

                    if (entities.TryGetValue("player", out Entity playerChar))
                    {
                        if (playerChar is Player player)
                        {
                            if (entity.Value != player & player.CollidesWith(entity.Value))
                            {
                                player.Collision();

                                entity.Value.ChangeAnimation("collision");
                                player.ChangeAnimation("collision");
                            }
                            else
                            {
                                entity.Value.ResetCollision();
                                player.ResetCollision();
                            }
                        }
                        else
                        {
                            // Throw exception if playerChar is somehow not of the type Player
                            throw new System.InvalidOperationException("playerChar is not of type Player");
                        }
                    }
                }

                // Update animations
                entity.Value.UpdateEntityAnimations(gameTime);
            }
        }
    }
}