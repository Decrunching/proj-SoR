using SoR.Logic.Entities;
using Logic.Entities.Character.Player;
using Logic.Entities.Character.Mobs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Logic.Game.GameMap.TiledScenery;
using Logic.Game.GameMap.Interactables;
using Logic.Game.GameMap;
using Logic.Game.Graphics;

namespace SoR.Logic.Game
{
    /*
     * Placeholder class for handling game progression.
     */
    public class GameLogic
    {
        private EntityType entityType;
        private SceneryType sceneryType;
        private Camera camera;
        private Dictionary<string, Entity> entities;
        private Dictionary<string, Scenery> scenery;
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private Vector2 playerPosition;
        private float relativePositionX;
        private float relativePositionY;

        /*
         * Enums for differentiating between entities.
         */
        enum EntityType
        {
            Player,
            Pheasant,
            Chara,
            Slime,
            Fishy
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
        public GameLogic(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice, GraphicsSettings graphicsSettings, GameWindow Window)
        {
            // Instantiate the game camera
            camera = new Camera (Window, GraphicsDevice, 800, 600);

            // Create dictionary for storing entities as values with string labels for keys
            entities = new Dictionary<string, Entity>();

            // Create dictionary for storing entities as values with string labels for keys
            scenery = new Dictionary<string, Scenery>();
        }

        /*
         * Load initial content into the game.
         */
        public void LoadGameContent(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice, MainGame game)
        {
            // Initialise SpriteBatch
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Font used for drawing text
            font = game.Content.Load<SpriteFont>("Fonts/File");

            // The centre of the screen
            relativePositionX = graphics.PreferredBackBufferWidth / 2;
            relativePositionY = graphics.PreferredBackBufferHeight / 2;

            // Create entities
            entityType = EntityType.Player;
            CreateEntity(graphics, GraphicsDevice);

            entityType = EntityType.Slime;
            CreateEntity(graphics, GraphicsDevice);

            entityType = EntityType.Chara;
            CreateEntity(graphics, GraphicsDevice);

            entityType = EntityType.Pheasant;
            CreateEntity(graphics, GraphicsDevice);

            entityType = EntityType.Fishy;
            CreateEntity(graphics, GraphicsDevice);

            // Create scenery
            sceneryType = SceneryType.Campfire;
            CreateObject(graphics, GraphicsDevice);

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
                        player.SetPosition(relativePositionX + 100, relativePositionY + 100);
                    }
                    break;
                case EntityType.Pheasant:
                    entities.Add("pheasant", new Pheasant(graphics, GraphicsDevice) { Render = true });
                    if (entities.TryGetValue("pheasant", out Entity pheasant))
                    {
                        pheasant.SetPosition(relativePositionX + 40, relativePositionY - 200);
                    }
                    break;
                case EntityType.Chara:
                    entities.Add("chara", new Chara(graphics, GraphicsDevice) { Render = true });
                    if (entities.TryGetValue("chara", out Entity chara))
                    {
                        chara.SetPosition(relativePositionX + 420, relativePositionY + 350);
                    }
                    break;
                case EntityType.Slime:
                    entities.Add("slime", new Slime(graphics, GraphicsDevice) { Render = true });
                    if (entities.TryGetValue("slime", out Entity slime))
                    {
                        slime.SetPosition(relativePositionX - 300, relativePositionY + 250);
                    }
                    break;
                case EntityType.Fishy:
                    entities.Add("fishy", new Fishy(graphics, GraphicsDevice) { Render = true });
                    if (entities.TryGetValue("fishy", out Entity fishy))
                    {
                        fishy.SetPosition(relativePositionX + 340, relativePositionY + 100);
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
                case SceneryType.Grass:
                    scenery.Add("grass", new Grass(graphics, GraphicsDevice) { Render = true });
                    if (scenery.TryGetValue("grass", out Scenery grass))
                    {
                        grass.SetPosition(relativePositionX - 96, relativePositionY - 96);
                    }
                    break;
            }
            switch (sceneryType)
            {
                case SceneryType.Campfire:
                    scenery.Add("campfire", new Campfire(graphics, GraphicsDevice) { Render = true });
                    if (scenery.TryGetValue("campfire", out Scenery campfire))
                    {
                        campfire.SetPosition(relativePositionX + 32, relativePositionY + 32);
                    }
                    break;
            }
        }

        /*
         * Update Spine animations and skeletons.
         */
        public void UpdateEntities(
            GameWindow Window,
            GameTime gameTime,
            GraphicsDeviceManager graphics,
            GraphicsSettings graphicsSettings,
            GraphicsDevice GraphicsDevice)
        {
            foreach (var scenery in scenery.Values)
            {
                if (scenery.Render)
                {
                    // Update animations
                    scenery.UpdateAnimations(gameTime);
                }
            }

            foreach (var entity in entities.Values)
            {
                if (entity.Render)
                {
                    entity.Movement(gameTime, graphics);

                    // Update position according to user input
                    entity.UpdatePosition(
                    gameTime,
                    graphics);

                    // Update animations
                    entity.UpdateAnimations(gameTime);

                    if (entities.TryGetValue("player", out Entity playerChar))
                    {
                        if (playerChar is Player player)
                        {
                            camera.FollowPlayer(player.GetPosition());

                            if (entity != player & player.CollidesWith(entity))
                            {
                                player.ChangeAnimation("collision");

                                entity.StopMoving();

                                player.Collision(entity, gameTime);
                                entity.Collision(player, gameTime);
                            }
                            else if (!entity.IsMoving())
                            {
                                entity.StartMoving();
                            }

                            foreach (var scenery in scenery.Values)
                            {
                                if (scenery.CollidesWith(entity))
                                {
                                    scenery.Collision(entity, gameTime);
                                }
                            }
                        }
                        else
                        {
                            // Throw exception if playerChar is somehow not of the type Player
                            throw new System.InvalidOperationException("playerChar is not of type Player");
                        }
                    }
                }
            }
        }

        /*
         * Update the graphics device with the new screen resolution after a resolution change.
         */
        public void UpdateViewportGraphics(int screenWidth, int screenHeight)
        {
            camera.UpdateGraphicsDevice(screenWidth, screenHeight);
        }

        /*
         * Render Spine objects in order of y-axis position.
         */
        public void Render(GraphicsDevice GraphicsDevice)
        {
            var sortSceneryByYAxis = scenery.Values.OrderBy(scenery => scenery.GetHitbox().MaxY);
            var sortEntitiesByYAxis = entities.Values.OrderBy(entity => entity.GetHitbox().MaxY);

            foreach (var scenery in sortSceneryByYAxis)
            {
                if (scenery.Render)
                {
                    scenery.RenderScenery(GraphicsDevice, camera.GetCamera());
                    scenery.DrawText(spriteBatch, font, camera.GetCamera());
                }
            }

            foreach (var entity in sortEntitiesByYAxis)
            {
                if (entity.Render)
                {
                    entity.RenderEntity(GraphicsDevice, camera.GetCamera());
                    entity.DrawText(spriteBatch, font, camera.GetCamera());
                }
            }
        }
    }
}