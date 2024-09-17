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
        private Map map;
        private Dictionary<string, Entity> entities;
        private Dictionary<string, Scenery> scenery;
        private Dictionary<string, string> renderAll;
        private SpriteFont font;
        private Render render;
        private float relativePositionX;
        private float relativePositionY;
        private int counter;

        public Dictionary<string, string> RenderAll { get { return renderAll; } }

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
            Campfire
        }

        /*
         * Manage the draw order of game components
         */
        enum DrawOrder
        {
            Floor,
            Interactables,
            Entities,
            Walls
        }

        /*
         * Constructor for initial game setup.
         */
        public GameLogic(GraphicsDevice GraphicsDevice, GameWindow Window)
        {
            // Set up the camera
            camera = new Camera (Window, GraphicsDevice, 800, 600);

            // Create dictionaries for game components
            entities = new Dictionary<string, Entity>();
            scenery = new Dictionary<string, Scenery>();

            // Manage the draw order of game components
            renderAll = new Dictionary<string, string>();
        }

        /*
         * Load initial content into the game.
         */
        public void LoadGameContent(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice, MainGame game)
        {
            render = new Render(GraphicsDevice);

            // Get the map to be used
            map = new Map(render.UseTileset(0, 0), render.UseTileset(0, 1));

            // Load map content
            map.LoadMap(game.Content);

            // Font used for drawing text
            font = game.Content.Load<SpriteFont>("Fonts/File");

            // The centre of the screen
            relativePositionX = graphics.PreferredBackBufferWidth / 2;
            relativePositionY = graphics.PreferredBackBufferHeight / 2;

            // Create entities
            entityType = EntityType.Player;
            CreateEntity(GraphicsDevice);
            renderAll.Add("player", "");

            entityType = EntityType.Slime;
            CreateEntity(GraphicsDevice);
            renderAll.Add("slime", "");

            entityType = EntityType.Chara;
            CreateEntity(GraphicsDevice);
            renderAll.Add("chara", "");

            entityType = EntityType.Pheasant;
            CreateEntity(GraphicsDevice);
            renderAll.Add("pheasant", "");

            entityType = EntityType.Fishy;
            CreateEntity(GraphicsDevice);
            renderAll.Add("fishy", "");

            // Create scenery
            sceneryType = SceneryType.Campfire;
            CreateObject(GraphicsDevice);
            renderAll.Add("campfire", "");


        }

        /*
         * Choose entity to create, give it a unique string identifier as a key and set Render to true.
         */
        public void CreateEntity(GraphicsDevice GraphicsDevice)
        {
            switch (entityType)
            {
                case EntityType.Player:
                    entities.Add("player", new Player(GraphicsDevice));
                    if (entities.TryGetValue("player", out Entity player))
                    {
                        player.SetPosition(relativePositionX, relativePositionY);
                    }
                    break;
                case EntityType.Pheasant:
                    entities.Add("pheasant", new Pheasant(GraphicsDevice));
                    if (entities.TryGetValue("pheasant", out Entity pheasant))
                    {
                        pheasant.SetPosition(relativePositionX + 40, relativePositionY - 200);
                    }
                    break;
                case EntityType.Chara:
                    entities.Add("chara", new Chara(GraphicsDevice));
                    if (entities.TryGetValue("chara", out Entity chara))
                    {
                        chara.SetPosition(relativePositionX + 420, relativePositionY + 350);
                    }
                    break;
                case EntityType.Slime:
                    entities.Add("slime", new Slime(GraphicsDevice));
                    if (entities.TryGetValue("slime", out Entity slime))
                    {
                        slime.SetPosition(relativePositionX - 300, relativePositionY + 250);
                    }
                    break;
                case EntityType.Fishy:
                    entities.Add("fishy", new Fishy(GraphicsDevice));
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
        public void CreateObject(GraphicsDevice GraphicsDevice)
        {
            switch (sceneryType)
            {
                case SceneryType.Campfire:
                    scenery.Add("campfire", new Campfire(GraphicsDevice));
                    if (scenery.TryGetValue("campfire", out Scenery campfire))
                    {
                        campfire.SetPosition(160, 256);
                    }
                    break;
            }
        }

        /*
         * Update world progress.
         */
        public void UpdateWorld(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            foreach (var scenery in scenery.Values)
            {
                // Update animations
                scenery.UpdateAnimations(gameTime);
            }

            foreach (var entity in entities.Values)
            {
                entity.Movement(gameTime);

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
            GraphicsDevice.Clear(Color.DarkSeaGreen); // Clear the graphics buffer and set the window background colour to "dark sea green"

            render.StartDrawingSpriteBatch(camera.GetCamera());
            render.DrawMap(map.GetFloorAtlas(), map, render.GetTempleFloor());
            render.FinishDrawingSpriteBatch();

            var sortSceneryByYAxis = scenery.Values.OrderBy(scenery => scenery.GetHitbox().MaxY);
            var sortEntitiesByYAxis = entities.Values.OrderBy(entity => entity.GetHitbox().MaxY);

            render.StartDrawingSpriteBatch(camera.GetCamera());
            foreach (var scenery in sortSceneryByYAxis)
            {
                // Draw skeletons
                render.StartDrawingSkeleton(GraphicsDevice, camera);
                render.DrawScenerySkeleton(scenery);
                render.FinishDrawingSkeleton();

                render.DrawScenerySpriteBatch(scenery, font);
            }

            foreach (var entity in sortEntitiesByYAxis)
            {
                // Draw skeletons
                render.StartDrawingSkeleton(GraphicsDevice, camera);
                render.DrawEntitySkeleton(entity);
                render.FinishDrawingSkeleton();

                render.DrawEntitySpriteBatch(entity, font);
            }
            render.FinishDrawingSpriteBatch();

            render.StartDrawingSpriteBatch(camera.GetCamera());
            render.DrawMap(map.GetWallAtlas(), map, render.GetTempleWalls());
            render.FinishDrawingSpriteBatch();
        }
    }
}