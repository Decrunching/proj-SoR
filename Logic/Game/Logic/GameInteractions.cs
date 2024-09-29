using Logic.Entities.Character.Player;
using Logic.Entities.Character.Mobs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Logic.Game.GameMap;
using Logic.Game.Graphics;
using SoR;
using Logic.Entities.Character;
using Logic.Entities.Interactables;

namespace Logic.Game.Screens
{
    /*
     * Part of the GameLogic class. Handles game component interactions. This part creates the map, entities and interactables, renders them to the
     * screen, and handles the interaction and rendering order.
     */
    public partial class GameLogic
    {

        /*
         * Load initial content into the game.
         */
        public void LoadGameContent(GraphicsDevice GraphicsDevice, MainGame game)
        {
            render = new Render(GraphicsDevice);

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

            Village(GraphicsDevice);


        }

        /*
         * Choose entity to create.
         */
        public void CreateEntity(GraphicsDevice GraphicsDevice, float positionX, float positionY)
        {
            switch (entityType)
            {
                case EntityType.Player:
                    entities.Add("player", new Player(GraphicsDevice, impassableArea) { Name = "player" });
                    if (entities.TryGetValue("player", out Entity player))
                    {
                        player.SetPosition(positionX, positionY);
                    }
                    break;
                case EntityType.Pheasant:
                    entities.Add("pheasant", new Pheasant(GraphicsDevice, impassableArea) { Name = "pheasant" });
                    if (entities.TryGetValue("pheasant", out Entity pheasant))
                    {
                        pheasant.SetPosition(positionX, positionY);
                    }
                    break;
                case EntityType.Chara:
                    entities.Add("chara", new Chara(GraphicsDevice, impassableArea) { Name = "chara" });
                    if (entities.TryGetValue("chara", out Entity chara))
                    {
                        chara.SetPosition(positionX, positionY);
                    }
                    break;
                case EntityType.Slime:
                    entities.Add("slime", new Slime(GraphicsDevice, impassableArea) { Name = "slime" });
                    if (entities.TryGetValue("slime", out Entity slime))
                    {
                        slime.SetPosition(positionX, positionY);
                    }
                    break;
                case EntityType.Fishy:
                    entities.Add("fishy", new Fishy(GraphicsDevice, impassableArea) { Name = "fishy" });
                    if (entities.TryGetValue("fishy", out Entity fishy))
                    {
                        fishy.SetPosition(positionX, positionY);
                    }
                    break;
            }
        }

        /*
         * Choose interactable object to create.
         */
        public void CreateObject(GraphicsDevice GraphicsDevice, float positionX, float positionY)
        {
            switch (sceneryType)
            {
                case SceneryType.Campfire:
                    scenery.Add("campfire", new Campfire(GraphicsDevice) { Name = "campfire" });
                    if (scenery.TryGetValue("campfire", out Scenery campfire))
                    {
                        campfire.SetPosition(positionX, positionY);
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