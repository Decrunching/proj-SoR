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
using MonoGame.Extended.Tiled;
using Spine;

namespace SoR.Logic.Game
{
    /*
     * Placeholder class for handling game progression.
     */
    public class GameLogic
    {
        private EntityType entityType;
        private SceneryType sceneryType;
        private TileType tileType;
        private Camera camera;
        private Dictionary<string, Entity> entities;
        private Dictionary<string, Scenery> scenery;
        private DrawTiles drawTiles;
        private TileLocations tileLocations;
        private SpriteFont font;
        private Render render;
        private Vector2 playerPosition;
        private float relativePositionX;
        private float relativePositionY;

        /*
         * Differentiate between entities.
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
         * Differentiate between environmental ojects.
         */
        enum SceneryType
        {
            Campfire,
            Grass
        }

        /*
         * Differentiate between tilesets.
         */
        enum TileType
        {
            Temple
        }

        /*
         * Constructor for initial game setup.
         */
        public GameLogic(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice, GraphicsSettings graphicsSettings, GameWindow Window)
        {
            // Set up the camera
            camera = new Camera (Window, GraphicsDevice, 800, 600);

            // Create dictionaries for game components
            render = new Render(GraphicsDevice);
            entities = new Dictionary<string, Entity>();
            scenery = new Dictionary<string, Scenery>();
            tileLocations = new TileLocations();
            drawTiles = new DrawTiles(0, tileLocations.UseTileset(0, 0), tileLocations.UseTileset(0, 1));
        }

        /*
         * Load initial content into the game.
         */
        public void LoadGameContent(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice, MainGame game)
        {

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

            drawTiles.LoadMap(game.Content,
                tileLocations.GetTempleLayout().GetLength(0),
                tileLocations.GetTempleLayout().GetLength(1));
        }

        /*
         * Choose entity to create, give it a unique string identifier as a key and set Render to true.
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
         * Choose environmental object to create, give it a unique string identifier as a key and set Render to true.
         */
        public void CreateObject(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice)
        {
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
         * Update world progress.
         */
        public void UpdateWorld(
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
        public void UpdateViewportGraphics(GameWindow Window, int screenWidth, int screenHeight)
        {
            camera.GetResolutionUpdate(screenWidth, screenHeight);
            camera.UpdateViewportAdapter(Window);
        }

        /*
         * Render Spine objects in order of y-axis position.
         */
        public void Render(GraphicsDevice GraphicsDevice)
        {
            render.StartDrawingSpriteBatch(camera.GetCamera());

            Vector2 mapLocation = new Vector2(playerPosition.X - 400, playerPosition.Y - 400);

            var sortSceneryByYAxis = scenery.Values.OrderBy(scenery => scenery.GetHitbox().MaxY);
            var sortEntitiesByYAxis = entities.Values.OrderBy(entity => entity.GetHitbox().MaxY);



            foreach (var scenery in sortSceneryByYAxis)
            {
                if (scenery.Render)
                {
                    // Draw skeletons
                    render.StartDrawingSkeleton(GraphicsDevice, camera);
                    render.DrawScenerySkeleton(scenery);
                    render.FinishDrawingSkeleton();

                    render.DrawScenerySpriteBatch(scenery, font);
                }
            }

            foreach (var entity in sortEntitiesByYAxis)
            {
                if (entity.Render)
                {
                    // Draw skeletons
                    render.StartDrawingSkeleton(GraphicsDevice, camera);
                    render.DrawEntitySkeleton(entity);
                    render.FinishDrawingSkeleton();

                    render.DrawEntitySpriteBatch(entity, font);
                }
            }



            render.FinishDrawingSpriteBatch();
        }
    }
}