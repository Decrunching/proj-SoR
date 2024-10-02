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
using Microsoft.Xna.Framework.Input;
using Logic.Game.Data;
using System.Linq;
using Logic.Game.UI;
using System.Windows.Controls;

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
        private MainMenu mainMenu;
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
                            Village(game, GraphicsDevice);
                            break;
                        case "temple":
                            Temple(game, GraphicsDevice);
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
                        Village(game, GraphicsDevice);
                        break;
                    case "temple":
                        Temple(game, GraphicsDevice);
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

            currentMapEnum = CurrentMap.MainMenu;
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
         * Get the positions of game elements before rendering.
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
         * Render game elements in order of y-axis position.
         */
        public void Render(GraphicsDevice GraphicsDevice)
        {
            switch (currentMapEnum)
            {
                case CurrentMap.MainMenu:
                    GraphicsDevice.Clear(Color.Black); // Clear the graphics buffer and set the window background colour
                    render.StartDrawingSpriteBatch(camera.GetCamera());
                    render.MainMenuText(mainMenu.MenuOptions[0], mainMenu.TitlePosition, font, Color.GhostWhite, 2.5f);
                    render.MainMenuText(mainMenu.MenuOptions[1], mainMenu.NewGamePosition, font, Color.Gray, 1);
                    render.MainMenuText(mainMenu.MenuOptions[2], mainMenu.ContinueGamePosition, font, Color.Gray, 1);
                    render.MainMenuText(mainMenu.MenuOptions[3], mainMenu.LoadGamePosition, font, Color.Gray, 1);
                    render.MainMenuText(mainMenu.MenuOptions[4], mainMenu.GameSettingsPosition, font, Color.Gray, 1);

                    switch (mainMenu.NavigateMenu())
                    {
                        case 0:
                            render.MainMenuText(mainMenu.MenuOptions[1], mainMenu.NewGamePosition, font, Color.GhostWhite, 1);
                            break;
                        case 1:
                            render.MainMenuText(mainMenu.MenuOptions[2], mainMenu.ContinueGamePosition, font, Color.GhostWhite, 1);
                            break;
                        case 2:
                            render.MainMenuText(mainMenu.MenuOptions[3], mainMenu.LoadGamePosition, font, Color.GhostWhite, 1);
                            break;
                        case 3:
                            render.MainMenuText(mainMenu.MenuOptions[4], mainMenu.GameSettingsPosition, font, Color.GhostWhite, 1);
                            break;
                    }

                    render.FinishDrawingSpriteBatch();
                    break;

                default:
                    GraphicsDevice.Clear(Color.DarkSeaGreen); // Clear the graphics buffer and set the window background colour

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

                    // Draw elements to the screen in order of y-axis position
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
                    break;

            }
        }
    }
}