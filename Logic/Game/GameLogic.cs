using Logic.Entities.Character;
using Logic.Locations;
using Logic.Locations.Interactables;
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
        private Dictionary<string, Scenery> scenery;
        private EntityType entityType;
        private SceneryType sceneryType;
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
            Slime
        }

        /*
         * Enums for differentiating between environmental ojects.
         */
        enum SceneryType
        {
            Campfire,
            Grass
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

            // Create dictionary for storing entities as values with string labels for keys
            scenery = new Dictionary<string, Scenery>();

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

            // Create the Campfire object
            sceneryType = SceneryType.Campfire;
            CreateObject(graphics, GraphicsDevice);

            // Create the Campfire object
            sceneryType = SceneryType.Grass;
            CreateObject(graphics, GraphicsDevice);
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
                    entities.Add("player", new Player(graphics, GraphicsDevice) { Render = true });
                    if (entities.TryGetValue("player", out Entity player))
                    {
                        player.SetStartPosition(centreScreen);
                    }
                    break;
                case EntityType.Pheasant:
                    entities.Add("pheasant", new Pheasant(graphics, GraphicsDevice) { Render = true });
                    if (entities.TryGetValue("pheasant", out Entity pheasant))
                    {
                        pheasant.SetStartPosition(centreScreen);
                    }
                    break;
                case EntityType.Chara:
                    entities.Add("chara", new Chara(graphics, GraphicsDevice) { Render = true });
                    if (entities.TryGetValue("chara", out Entity chara))
                    {
                        chara.SetStartPosition(centreScreen);
                    }
                    break;
                case EntityType.Slime:
                    entities.Add("slime", new Slime(graphics, GraphicsDevice) { Render = true });
                    if (entities.TryGetValue("slime", out Entity slime))
                    {
                        slime.SetStartPosition(centreScreen);
                    }
                    break;
            }
        }

        /*
         * Choose environmental object to create and place it as a value in the objects dictionary with
         * a unique string identifier as a key.
         */
        public void CreateObject(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice)
        {
            switch (sceneryType)
            {
                case SceneryType.Campfire:
                    scenery.Add("campfire", new Campfire(graphics, GraphicsDevice) { Render = true });
                    if (scenery.TryGetValue("campfire", out Scenery campfire))
                    {
                        campfire.SetStartPosition();
                    }
                    break;
            }
            switch (sceneryType)
            {
                case SceneryType.Grass:
                    scenery.Add("grass", new Grass(graphics, GraphicsDevice) { Render = true });
                    if (scenery.TryGetValue("grass", out Scenery grass))
                    {
                        grass.SetStartPosition();
                    }
                    break;
            }
        }

        /*
         * Render Spine objects in order of y-axis position.
         */
        public void Render(GraphicsDevice GraphicsDevice)
        {
            var sortSceneryByYAxis = scenery.Values.OrderBy(scenery => scenery.GetSceneryHitbox().MaxY);
            var sortEntitiesByYAxis = entities.Values.OrderBy(entity => entity.GetEntityHitbox().MaxY);

            foreach (var scenery in sortSceneryByYAxis)
            {
                if (scenery.Render)
                {
                    scenery.RenderScenery(GraphicsDevice);
                    scenery.DrawSceneryText(spriteBatch, font);
                }
            }

            foreach (var entity in sortEntitiesByYAxis)
            {
                if (entity.Render)
                {
                    entity.RenderEntity(GraphicsDevice);
                    entity.DrawEntityText(spriteBatch, font);
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
            foreach (var scenery in scenery.Values)
            {
                if (scenery.Render)
                {


                    // Update animations
                    scenery.UpdateSceneryAnimations(gameTime);
                }
            }

            foreach (var entity in entities.Values)
            {
                if (entity.Render)
                {
                    entity.Movement(gameTime);

                    // Update position according to user input
                    entity.UpdatePosition(
                    gameTime,
                    graphics,
                    GraphicsDevice);

                    if (entities.TryGetValue("player", out Entity playerChar))
                    {
                        if (playerChar is Player player)
                        {
                            if (entity != player & player.CollidesWith(entity))
                            {
                                player.ChangeAnimation("collision");

                                entity.StopMoving();
                            }
                            else if (!entity.IsMoving())
                            {
                                entity.StartMoving();
                            }
                        }
                        else
                        {
                            // Throw exception if playerChar is somehow not of the type Player
                            throw new System.InvalidOperationException("playerChar is not of type Player");
                        }
                    }

                    // Update animations
                    entity.UpdateEntityAnimations(gameTime);
                }
            }
        }
    }
}