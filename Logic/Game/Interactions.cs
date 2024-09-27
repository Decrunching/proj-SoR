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
    public class Interactions
    {
        private EntityType entityType;
        private SceneryType sceneryType;
        private Camera camera;
        private Map map;
        private Dictionary<string, Entity> entities;
        private Dictionary<string, Scenery> scenery;
        private Dictionary<string, Vector2> mapLowerWalls;
        private Dictionary<string, Vector2> mapUpperWalls;
        private Dictionary<string, Vector2> mapFloor;
        private List<Vector2> positions;
        private List<Rectangle> impassableArea;
        private SpriteFont font;
        private Render render;

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
         * Constructor for initial game setup.
         */
        public Interactions(GraphicsDevice GraphicsDevice, GameWindow Window)
        {
            // Set up the camera
            camera = new Camera (Window, GraphicsDevice, 800, 600);

            // Create dictionaries for interactable game components
            entities = [];
            scenery = [];
        }

        /*
         * Load initial content into the game.
         */
        public void LoadGameContent(GraphicsDevice GraphicsDevice, MainGame game)
        {
            render = new Render(GraphicsDevice);

            // Get the map to be used
            map = new Map(0);

            // Load map content
            map.LoadMap(game.Content, map.FloorSpriteSheet, map.WallSpriteSheet);

            // Font used for drawing text
            font = game.Content.Load<SpriteFont>("Fonts/File");

            // Map the tile drawing positions to their atlases
            mapLowerWalls = render.CreateMap(map, map.LowerWalls, true);
            mapUpperWalls = render.CreateMap(map, map.UpperWalls);
            mapFloor = render.CreateMap( map, map.Floor);
            render.ImpassableMapArea();
            impassableArea = render.ImpassableTiles;
        }

        /*
         * Choose entity to create.
         */
        public void CreateEntity(GraphicsDevice GraphicsDevice)
        {
            switch (entityType)
            {
                case EntityType.Player:
                    entities.Add("player", new Player(GraphicsDevice, impassableArea) { Name = "player" });
                    if (entities.TryGetValue("player", out Entity player))
                    {
                        player.SetPosition(250, 200);
                    }
                    break;
                case EntityType.Pheasant:
                    entities.Add("pheasant", new Pheasant(GraphicsDevice, impassableArea) { Name = "pheasant" });
                    if (entities.TryGetValue("pheasant", out Entity pheasant))
                    {
                        pheasant.SetPosition(270, 200);
                    }
                    break;
                case EntityType.Chara:
                    entities.Add("chara", new Chara(GraphicsDevice, impassableArea) { Name = "chara" });
                    if (entities.TryGetValue("chara", out Entity chara))
                    {
                        chara.SetPosition(250, 250);
                    }
                    break;
                case EntityType.Slime:
                    entities.Add("slime", new Slime(GraphicsDevice, impassableArea) { Name = "slime" });
                    if (entities.TryGetValue("slime", out Entity slime))
                    {
                        slime.SetPosition(250, 130);
                    }
                    break;
                case EntityType.Fishy:
                    entities.Add("fishy", new Fishy(GraphicsDevice, impassableArea) { Name = "fishy" });
                    if (entities.TryGetValue("fishy", out Entity fishy))
                    {
                        fishy.SetPosition(280, 180);
                    }
                    break;
            }
        }

        /*
         * Choose interactable object to create.
         */
        public void CreateObject(GraphicsDevice GraphicsDevice)
        {
            switch (sceneryType)
            {
                case SceneryType.Campfire:
                    scenery.Add("campfire", new Campfire(GraphicsDevice) { Name = "campfire" });
                    if (scenery.TryGetValue("campfire", out Scenery campfire))
                    {
                        campfire.SetPosition(224, 160);
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
                // Update position according to user input
                entity.UpdatePosition(gameTime, graphics);

                // Update animations
                entity.UpdateAnimations(gameTime);

                if (entities.TryGetValue("player", out Entity playerChar))
                {
                    if (playerChar is Player player)
                    {
                        camera.FollowPlayer(player.GetPosition());

                        if (entity != player & player.CollidesWith(entity))
                        {
                            entity.StopMoving();

                            player.EntityCollision(entity, gameTime);
                            entity.EntityCollision(player, gameTime);
                        }
                        else if (!entity.IsMoving())
                        {
                            entity.StartMoving();
                        }

                        foreach (var scenery in scenery.Values)
                        {
                            if (scenery.CollidesWith(entity))
                            {
                                entity.StopMoving();

                                scenery.Collision(entity, gameTime);
                            }
                            else if (!entity.IsMoving())
                            {
                                entity.StartMoving();
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
         * Get the positions of game components before rendering.
         */
        public void RefreshPositions()
        {
            positions = new List<Vector2>();
            foreach (var scenery in scenery.Values)
            {
                if (!positions.Contains(scenery.GetPosition()))
                {
                    positions.Add(scenery.GetPosition());
                }
            }
            foreach (var entity in entities.Values)
            {
                if (!positions.Contains(entity.GetPosition()))
                {
                    positions.Add(entity.GetPosition());
                }
            }
            foreach (var tile in mapLowerWalls.Values)
            {
                positions.Add(tile);
            }
        }

        /*
         * Render game components in order of y-axis position.
         */
        public void Render(GraphicsDevice GraphicsDevice)
        {
            GraphicsDevice.Clear(Color.DarkSeaGreen); // Clear the graphics buffer and set the window background colour to "dark sea green"

            foreach (var tileName in mapFloor)
            {
                render.StartDrawingSpriteBatch(camera.GetCamera());
                render.DrawMap(map.GetFloorAtlas(), map, tileName.Key, tileName.Value, font);
                render.FinishDrawingSpriteBatch();
            }

            RefreshPositions();
            var sortPositionsByYAxis = positions.OrderBy(position => position.Y);

            // Draw components to the screen in order of y-axis position
            foreach (var position in sortPositionsByYAxis)
            {
                render.StartDrawingSpriteBatch(camera.GetCamera());
                foreach (var entity in entities.Values)
                {
                    if (entity.GetPosition().Y == position.Y)
                    {
                        // Draw skeletons
                        render.StartDrawingSkeleton(GraphicsDevice, camera);
                        render.DrawEntitySkeleton(entity);
                        render.FinishDrawingSkeleton();

                        render.DrawEntitySpriteBatch(entity, font);
                    }
                }
                foreach (var scenery in scenery.Values)
                {
                    if (scenery.GetPosition().Y == position.Y)
                    {
                        // Draw skeletons
                        render.StartDrawingSkeleton(GraphicsDevice, camera);
                        render.DrawScenerySkeleton(scenery);
                        render.FinishDrawingSkeleton();

                        render.DrawScenerySpriteBatch(scenery, font);
                    }
                }
                render.FinishDrawingSpriteBatch();

                foreach (var tileName in mapLowerWalls)
                {
                    if (tileName.Value.Y == position.Y && tileName.Value.X == position.X)
                    {
                        render.StartDrawingSpriteBatch(camera.GetCamera());
                        render.DrawMap(map.GetWallAtlas(), map, tileName.Key, position, font);
                        render.FinishDrawingSpriteBatch();
                    }
                }
            }

            foreach (var tileName in mapUpperWalls)
            {
                render.StartDrawingSpriteBatch(camera.GetCamera());
                render.DrawMap(map.GetWallAtlas(), map, tileName.Key, tileName.Value, font);
                render.FinishDrawingSpriteBatch();
            }
        }
    }
}