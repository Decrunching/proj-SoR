using SoR;
using Logic.Game.GameMap;
using Logic.Game.Graphics;
using Logic.Game.GameMap.TiledScenery;
using Logic.Entities.Character;
using Logic.Entities.Interactables;
using Logic.Entities.Character.Player;
using Logic.Entities.Character.Mobs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Logic.Game.Data;

namespace Logic.Game.Screens
{
    /*
     * Game logic. Manages interactions between game components. This part of the partial class sets which entities and interactables will appear in
     * each game location and positions them on the map.
     */
    public partial class GameLogic
    {
        private EntityType entityType;
        private SceneryType sceneryType;
        private CurrentMap currentMapEnum;
        private Map map;
        private Render render;
        private Camera camera;
        private SpriteFont font;
        private Dictionary<string, Vector2> mapLowerWalls;
        private Dictionary<string, Vector2> mapUpperWalls;
        private Dictionary<string, Vector2> mapFloor;
        private Dictionary<string, Vector2> mapFloorDecor;
        private List<Vector2> positions;
        private List<Rectangle> impassableArea;
        private KeyboardState keyState;
        private KeyboardState lastKeyState;
        private GamePadState gamePadState;
        private GamePadState lastGamePadState;
        private GamePadCapabilities gamePadCapabilities;
        private bool hasUpperWalls;
        private bool hasFloorDecor;
        public Dictionary<string, Entity> Entities { get; set; }
        public Dictionary<string, Scenery> Scenery { get; set; }
        public string CurrentMapString { get; set; }

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
         * The game map currently in use.
         */
        enum CurrentMap
        {
            Village,
            Temple
        }

        /*
         * Constructor for initial game setup.
         */
        public GameLogic(GraphicsDevice GraphicsDevice, GameWindow Window)
        {
            camera = new Camera(Window, GraphicsDevice, 800, 600);
            gamePadCapabilities = GamePad.GetCapabilities(PlayerIndex.One);

            CurrentMapString = "none";
            hasFloorDecor = false;
            hasUpperWalls = false;

            Entities = [];
            Scenery = [];
            mapLowerWalls = [];
            mapUpperWalls = [];
            mapFloor = [];
            mapFloorDecor = [];
            positions = [];
            impassableArea = [];
        }

        /*
         * Create or dismantle the Village scene.
         */
        public void Village(MainGame game, GraphicsDevice GraphicsDevice, bool create)
        {
            if (create)
            {
                // Get the map to be used
                map = new Map(1);
                currentMapEnum = CurrentMap.Village;
                LoadGameContent(GraphicsDevice, game);
                hasFloorDecor = true;
                hasUpperWalls = false;

                // Create the map
                map.LoadMap(game.Content, map.FloorSpriteSheet, map.WallSpriteSheet, map.FloorDecorSpriteSheet);
                mapLowerWalls = render.CreateMap(map, map.LowerWalls, true);
                mapUpperWalls = [];
                mapFloor = render.CreateMap(map, map.Floor);
                mapFloorDecor = render.CreateMap(map, map.FloorDecor);
                render.ImpassableMapArea();
                impassableArea = render.ImpassableTiles;

                // Re-initialise the entity and scenery arrays
                Entities = [];
                Scenery = [];

                // Create entities
                entityType = EntityType.Player;
                CreateEntity(GraphicsDevice, 250, 200);

                entityType = EntityType.Chara;
                CreateEntity(GraphicsDevice, 200, 250);

                entityType = EntityType.Pheasant;
                CreateEntity(GraphicsDevice, 250, 250);
            }
        }

        /*
         * Create or dismantle the Temple scene.
         */
        public void Temple(MainGame game, GraphicsDevice GraphicsDevice, bool create)
        {
            if (create)
            {
                // Get the map to be used
                map = new Map(0);
                currentMapEnum = CurrentMap.Temple;
                LoadGameContent(GraphicsDevice, game);
                hasFloorDecor = false;
                hasUpperWalls = true;

                // Create the map
                map.LoadMap(game.Content, map.FloorSpriteSheet, map.WallSpriteSheet);
                mapLowerWalls = render.CreateMap(map, map.LowerWalls, true);
                mapUpperWalls = render.CreateMap(map, map.UpperWalls);
                mapFloor = render.CreateMap(map, map.Floor);
                mapFloorDecor = [];
                render.ImpassableMapArea();
                impassableArea = render.ImpassableTiles;

                // Re-initialise the entity and scenery arrays
                Entities = [];
                Scenery = [];

                // Create entities
                entityType = EntityType.Player;
                CreateEntity(GraphicsDevice, 220, 100);

                entityType = EntityType.Fishy;
                CreateEntity(GraphicsDevice, 280, 180);

                entityType = EntityType.Slime;
                CreateEntity(GraphicsDevice, 250, 200);

                // Create scenery
                sceneryType = SceneryType.Campfire;
                CreateObject(GraphicsDevice, 224, 160);
            }
        }

        /*
         * Save or load game data according to player input.
         */
        public void SaveLoadGameData(MainGame game, GraphicsDevice GraphicsDevice)
        {
            keyState = Keyboard.GetState(); // Get the current keyboard state

            if (gamePadCapabilities.IsConnected) // If the gamepad is connected
            {
                gamePadState = GamePad.GetState(PlayerIndex.One); // Get the current gamepad state

                if (gamePadState.Buttons.B == ButtonState.Pressed && lastGamePadState.Buttons.B != ButtonState.Pressed)
                {
                    foreach (var entity in Entities.Values)
                    {
                        if (Entities.TryGetValue("player", out Entity player))
                        {
                            switch (currentMapEnum)
                            {
                                case CurrentMap.Village:
                                    CurrentMapString = "village";
                                    break;
                                case CurrentMap.Temple:
                                    CurrentMapString = "temple";
                                    break;
                            }
                            GameState.SaveFile(player, CurrentMapString);
                            CurrentMapString = "none";
                            break;
                        }
                    }
                }
                if (gamePadState.Buttons.A == ButtonState.Pressed && lastGamePadState.Buttons.A != ButtonState.Pressed)
                {
                    GameState gameState = GameState.LoadFile();

                    switch (gameState.CurrentMap)
                    {
                        case "village":
                            Village(game, GraphicsDevice, true);
                            break;
                        case "temple":
                            Temple(game, GraphicsDevice, true);
                            break;
                    }

                    foreach (var entity in Entities.Values)
                    {
                        if (Entities.TryGetValue("player", out Entity player))
                        {
                            player.Position = gameState.Position;
                            player.HitPoints = gameState.HitPoints;
                            player.Skin = gameState.Skin;
                        }
                    }
                }

                lastGamePadState = gamePadState;
            }

            if (keyState.IsKeyDown(Keys.F8) && !lastKeyState.IsKeyDown(Keys.F8))
            {
                foreach (var entity in Entities.Values)
                {
                    if (Entities.TryGetValue("player", out Entity player))
                    {
                        switch (currentMapEnum)
                        {
                            case CurrentMap.Village:
                                CurrentMapString = "village";
                                break;
                            case CurrentMap.Temple:
                                CurrentMapString = "temple";
                                break;
                        }
                        GameState.SaveFile(player, CurrentMapString);
                        CurrentMapString = "none";
                        break;
                    }
                }
            }
            if (keyState.IsKeyDown(Keys.F9) && !lastKeyState.IsKeyDown(Keys.F9))
            {
                GameState gameState = GameState.LoadFile();

                switch (gameState.CurrentMap)
                {
                    case "village":
                        Village(game, GraphicsDevice, true);
                        break;
                    case "temple":
                        Temple(game, GraphicsDevice, true);
                        break;
                }

                foreach (var entity in Entities.Values)
                {
                    if (Entities.TryGetValue("player", out Entity player))
                    {
                        player.Position = gameState.Position;
                        player.HitPoints = gameState.HitPoints;
                        player.Skin = gameState.Skin;
                    }
                }
            }

            lastKeyState = keyState;
        }

        /*
         * Load initial content into the game.
         */
        public void LoadGameContent(GraphicsDevice GraphicsDevice, MainGame game)
        {
            render = new Render(GraphicsDevice);

            // Font used for drawing text
            font = game.Content.Load<SpriteFont>("Fonts/File");
        }

        /*
         * Choose entity to create.
         */
        public void CreateEntity(GraphicsDevice GraphicsDevice, float positionX, float positionY)
        {
            switch (entityType)
            {
                case EntityType.Player:
                    Entities.Add("player", new Player(GraphicsDevice, impassableArea) { Type = "player" });
                    if (Entities.TryGetValue("player", out Entity player))
                    {
                        player.SetPosition(positionX, positionY);
                    }
                    break;
                case EntityType.Pheasant:
                    Entities.Add("pheasant", new Pheasant(GraphicsDevice, impassableArea) { Type = "pheasant" });
                    if (Entities.TryGetValue("pheasant", out Entity pheasant))
                    {
                        pheasant.SetPosition(positionX, positionY);
                    }
                    break;
                case EntityType.Chara:
                    Entities.Add("chara", new Chara(GraphicsDevice, impassableArea) { Type = "chara" });
                    if (Entities.TryGetValue("chara", out Entity chara))
                    {
                        chara.SetPosition(positionX, positionY);
                    }
                    break;
                case EntityType.Slime:
                    Entities.Add("slime", new Slime(GraphicsDevice, impassableArea) { Type = "slime" });
                    if (Entities.TryGetValue("slime", out Entity slime))
                    {
                        slime.SetPosition(positionX, positionY);
                    }
                    break;
                case EntityType.Fishy:
                    Entities.Add("fishy", new Fishy(GraphicsDevice, impassableArea) { Type = "fishy" });
                    if (Entities.TryGetValue("fishy", out Entity fishy))
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
                    Scenery.Add("campfire", new Campfire(GraphicsDevice) { Name = "campfire" });
                    if (Scenery.TryGetValue("campfire", out Scenery campfire))
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
            foreach (var scenery in Scenery.Values)
            {
                // Update animations
                scenery.UpdateAnimations(gameTime);
            }

            foreach (var entity in Entities.Values)
            {
                // Update position according to user input
                entity.UpdatePosition(gameTime, graphics);

                // Update animations
                entity.UpdateAnimations(gameTime);

                if (Entities.TryGetValue("player", out Entity playerChar))
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

                        foreach (var scenery in Scenery.Values)
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
            positions = [];
            foreach (var scenery in Scenery.Values)
            {
                if (!positions.Contains(scenery.GetPosition()))
                {
                    positions.Add(scenery.GetPosition());
                }
            }
            foreach (var entity in Entities.Values)
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
                render.DrawMap(map.GetFloorAtlas(), map, tileName.Key, tileName.Value);
                render.FinishDrawingSpriteBatch();
            }
            if (hasFloorDecor)
            {
                foreach (var tileName in mapFloorDecor)
                {
                    render.StartDrawingSpriteBatch(camera.GetCamera());
                    render.DrawMap(map.GetFloorDecorAtlas(), map, tileName.Key, tileName.Value);
                    render.FinishDrawingSpriteBatch();
                }
            }

            RefreshPositions();
            var sortPositionsByYAxis = positions.OrderBy(position => position.Y);

            // Draw components to the screen in order of y-axis position
            foreach (var position in sortPositionsByYAxis)
            {
                render.StartDrawingSpriteBatch(camera.GetCamera());
                foreach (var entity in Entities.Values)
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
                foreach (var scenery in Scenery.Values)
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
                        render.DrawMap(map.GetWallAtlas(), map, tileName.Key, position);
                        render.FinishDrawingSpriteBatch();
                    }
                }
            }

            if (hasUpperWalls)
            {
                foreach (var tileName in mapUpperWalls)
                {
                    render.StartDrawingSpriteBatch(camera.GetCamera());
                    render.DrawMap(map.GetWallAtlas(), map, tileName.Key, tileName.Value);
                    render.FinishDrawingSpriteBatch();
                }
            }
        }
    }
}