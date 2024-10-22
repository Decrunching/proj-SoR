using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using SoR.Hardware.Data;
using SoR.Hardware.Graphics;
using SoR.Hardware.Input;
using SoR.Logic.GameMap.TiledScenery;
using SoR.Logic.GameMap.Interactables;
using SoR.Logic.Character;
using SoR.Logic.Character.Player;
using SoR.Logic.Character.Mobs;
using SoR.Logic.UI;
using SoR.Logic.GameMap;
using System.IO;
using Spine;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace SoR.Logic
{
    /*
     * Game logic. Manages how game elements are created, destroyed, rendered and positioned,
     * as well as handling how, when and why various elements will interact.
     */
    public partial class GameLogic
    {
        private EntityType entityType;
        private SceneryType sceneryType;
        private CurrentMap currentMapEnum;
        private MainMenu mainMenu;
        private StartMenu startMenu;
        private Map map;
        private Render render;
        private Camera camera;
        private SpriteFont font;
        private GamePadInput gamePadInput;
        private KeyboardInput keyboardInput;
        private Entity player;
        private Color backgroundColour;
        private Dictionary<string, Vector2> mapLowerWalls;
        private Dictionary<string, Vector2> mapUpperWalls;
        private Dictionary<string, Vector2> mapFloor;
        private Dictionary<string, Vector2> mapFloorDecor;
        private List<Vector2> positions;
        private List<Rectangle> impassableArea;
        private string currentMenuItem;
        private bool menu;
        private bool exitMenu;
        private bool freezeGame;
        private bool loadingGame;
        private bool newGame;
        private bool hasUpperWalls;
        private bool hasFloorDecor;
        private bool fadingOut;
        private float curtainOpacity;
        private float curtainTimer;
        public Dictionary<string, Entity> Entities { get; set; }
        public Dictionary<string, Scenery> Scenery { get; set; }
        public string InGameScreen { get; set; }
        public string PlayerLocation { get; set; }
        public string SaveFile { get; set; }
        public bool FadingIn { get; set; }
        public bool CurtainUp { get; set; }
        public bool ExitGame { get; set; }

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
        public GameLogic(MainGame game, GraphicsDevice GraphicsDevice)
        {
            camera = new Camera(game.Window, GraphicsDevice, 800, 600);
            gamePadInput = new GamePadInput();
            keyboardInput = new KeyboardInput();

            SaveFile = Globals.GetSavePath("SoR\\saveFile.json");

            PlayerLocation = "none";
            InGameScreen = "mainMenu";
            hasFloorDecor = false;
            hasUpperWalls = false;

            currentMenuItem = "none";
            menu = false;
            exitMenu = false;
            ExitGame = false;
            freezeGame = false;
            loadingGame = false;
            FadingIn = false;
            CurtainUp = false;
            fadingOut = false;
            curtainOpacity = 0f;
            curtainTimer = 0f;
            backgroundColour = new Color(0, 11, 8);

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
         * Save the game to a file.
         */
        public void SaveGame()
        {
            switch (currentMapEnum)
            {
                case CurrentMap.Village:
                    PlayerLocation = "village";
                    break;
                case CurrentMap.Temple:
                    PlayerLocation = "temple";
                    break;
            }
            GameState.SaveFile(player, PlayerLocation);
            PlayerLocation = "none";
        }

        /*
         * Load the game from a file once the curtain is fully opaque.
         */
        public void LoadGame(MainGame game, GameTime gameTime, GraphicsDevice GraphicsDevice)
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

            player.Position = gameState.Position;
            player.HitPoints = gameState.HitPoints;
            player.Skin = gameState.Skin;
            player.UpdateSkin(gameState.Skin);
            loadingGame = false;
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
                        this.player = player;
                        player.Frozen = true;
                    }
                    break;
                case EntityType.Pheasant:
                    Entities.Add("pheasant", new Pheasant(GraphicsDevice, impassableArea) { Type = "pheasant" });
                    if (Entities.TryGetValue("pheasant", out Entity pheasant))
                    {
                        pheasant.SetPosition(positionX, positionY);
                        pheasant.Frozen = true;
                    }
                    break;
                case EntityType.Chara:
                    Entities.Add("chara", new Chara(GraphicsDevice, impassableArea) { Type = "chara" });
                    if (Entities.TryGetValue("chara", out Entity chara))
                    {
                        chara.SetPosition(positionX, positionY);
                        chara.Frozen = true;
                    }
                    break;
                case EntityType.Slime:
                    Entities.Add("slime", new Slime(GraphicsDevice, impassableArea) { Type = "slime" });
                    if (Entities.TryGetValue("slime", out Entity slime))
                    {
                        slime.SetPosition(positionX, positionY);
                        slime.Frozen = true;
                    }
                    break;
                case EntityType.Fishy:
                    Entities.Add("fishy", new Fishy(GraphicsDevice, impassableArea) { Type = "fishy" });
                    if (Entities.TryGetValue("fishy", out Entity fishy))
                    {
                        fishy.SetPosition(positionX, positionY);
                        fishy.Frozen = true;
                    }
                    break;
            }
        }

        /*
         * Choose interactable object to create.
         */
        public void CreateScenery(GraphicsDevice GraphicsDevice, float positionX, float positionY)
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
        public void UpdateWorld(MainGame game, GameTime gameTime, GraphicsDevice GraphicsDevice, GraphicsDeviceManager graphics)
        {
            camera.FollowPlayer(player.Position);

            if (!freezeGame)
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
            }
        }

        /*
         * Get the current game time.
         */
        public static float GetTime(GameTime gameTime)
        {
            return (float)gameTime.ElapsedGameTime.TotalSeconds;
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
                if (!positions.Contains(entity.Position))
                {
                    positions.Add(entity.Position);
                }
            }
            foreach (var tile in mapLowerWalls.Values)
            {
                positions.Add(tile);
            }
        }

        /*
         * Draw the curtain.
         */
        public void DrawCurtain(float fadeAlpha = 1f)
        {
            Vector2 backgroundPosition = new Vector2(camera.PlayerPosition.X, camera.PlayerPosition.Y);

            render.StartDrawingSpriteBatch(camera.GetCamera());
            render.Curtain(mainMenu.Curtain, camera.NewWidth, camera.NewHeight, backgroundPosition, fadeAlpha);
            render.FinishDrawingSpriteBatch();
        }

        /*
         * Draw the MainMenu.
         * 
         * *** TO DO *** Reposition *** TO DO ***
         */
        public void DrawMainMenu(GameTime gameTime)
        {
            // Use camera.PlayerPosition as point of reference for positioning since it's updated with screen resolution
            Vector2 backgroundPosition = new Vector2(camera.PlayerPosition.X, camera.PlayerPosition.Y);
            Vector2 titlePosition = new Vector2(camera.PlayerPosition.X - 125, camera.PlayerPosition.Y - 156);
            Vector2 newGamePosition = new Vector2(camera.PlayerPosition.X - 250, camera.PlayerPosition.Y + 20);
            Vector2 continueGamePosition = new Vector2(camera.PlayerPosition.X - 250, camera.PlayerPosition.Y + 50);
            Vector2 loadGamePosition = new Vector2(camera.PlayerPosition.X - 250, camera.PlayerPosition.Y + 80);
            Vector2 gameSettingsPosition = new Vector2(camera.PlayerPosition.X - 250, camera.PlayerPosition.Y+ 110);
            Vector2 toDesktopPosition = new Vector2(camera.PlayerPosition.X - 250, camera.PlayerPosition.Y + 140);

            render.StartDrawingSpriteBatch(camera.GetCamera());
            render.Curtain(mainMenu.Curtain, camera.NewWidth, camera.NewHeight, backgroundPosition);
            render.MenuText(mainMenu.MenuOptions[0], titlePosition, font, Color.GhostWhite, 2.5f);
            render.MenuText(mainMenu.MenuOptions[1], newGamePosition, font, Color.Gray, 1);
            render.MenuText(mainMenu.MenuOptions[2], continueGamePosition, font, Color.Gray, 1);
            render.MenuText(mainMenu.MenuOptions[3], loadGamePosition, font, Color.Gray, 1);
            render.MenuText(mainMenu.MenuOptions[4], gameSettingsPosition, font, Color.Gray, 1);
            render.MenuText(mainMenu.MenuOptions[5], toDesktopPosition, font, Color.Gray, 1);
            render.FinishDrawingSpriteBatch();

            switch (mainMenu.NavigateMenu(gameTime))
            {
                case 0:
                    render.StartDrawingSpriteBatch(camera.GetCamera());
                    render.MenuText(mainMenu.MenuOptions[1], newGamePosition, font, Color.GhostWhite, 1);
                    render.FinishDrawingSpriteBatch();
                    currentMenuItem = mainMenu.MenuOptions[1];
                    break;
                case 1:
                    if (File.Exists(SaveFile))
                    {
                        render.StartDrawingSpriteBatch(camera.GetCamera());
                        render.MenuText(mainMenu.MenuOptions[2], continueGamePosition, font, Color.GhostWhite, 1);
                        render.FinishDrawingSpriteBatch();
                        currentMenuItem = mainMenu.MenuOptions[2];
                    }
                    else
                    {
                        render.StartDrawingSpriteBatch(camera.GetCamera());
                        render.MenuText(mainMenu.MenuOptions[2], continueGamePosition, font, Color.LightCoral, 1);
                        render.FinishDrawingSpriteBatch();
                        currentMenuItem = "none";
                    }
                    break;
                case 2:
                    if (File.Exists(SaveFile))
                    {
                        render.StartDrawingSpriteBatch(camera.GetCamera());
                        render.MenuText(mainMenu.MenuOptions[3], loadGamePosition, font, Color.GhostWhite, 1);
                        render.FinishDrawingSpriteBatch();
                        currentMenuItem = mainMenu.MenuOptions[3];
                    }
                    else
                    {
                        render.StartDrawingSpriteBatch(camera.GetCamera());
                        render.MenuText(mainMenu.MenuOptions[3], loadGamePosition, font, Color.LightCoral, 1);
                        render.FinishDrawingSpriteBatch();
                        currentMenuItem = "none";
                    }
                    break;
                case 3:
                    render.StartDrawingSpriteBatch(camera.GetCamera());
                    render.MenuText(mainMenu.MenuOptions[4], gameSettingsPosition, font, Color.GhostWhite, 1);
                    render.FinishDrawingSpriteBatch();
                    currentMenuItem = mainMenu.MenuOptions[4];
                    break;
                case 4:
                    render.StartDrawingSpriteBatch(camera.GetCamera());
                    render.MenuText(mainMenu.MenuOptions[5], toDesktopPosition, font, Color.GhostWhite, 1);
                    render.FinishDrawingSpriteBatch();
                    currentMenuItem = mainMenu.MenuOptions[5];
                    break;
            }
        }

        /*
         * Draw the StartMenu.
         */
        public void DrawStartMenu(GameTime gameTime)
        {
            // Use camera.PlayerPosition as point of reference for positioning since it's updated with screen resolution
            Vector2 backgroundPosition = new Vector2(camera.PlayerPosition.X, camera.PlayerPosition.Y);
            Vector2 inventoryPosition = new Vector2(camera.PlayerPosition.X - 350, camera.PlayerPosition.Y - 156);
            Vector2 gameSettingsPosition = new Vector2(camera.PlayerPosition.X - 350, camera.PlayerPosition.Y - 56);
            Vector2 loadGamePosition = new Vector2(camera.PlayerPosition.X - 350, camera.PlayerPosition.Y + 44);
            Vector2 exitGamePosition = new Vector2(camera.PlayerPosition.X - 350, camera.PlayerPosition.Y + 144);
            Vector2 toMainMenuPosition = new Vector2(camera.PlayerPosition.X - 50, camera.PlayerPosition.Y - 56);
            Vector2 toDesktopPosition = new Vector2(camera.PlayerPosition.X - 50, camera.PlayerPosition.Y + 44);

            render.StartDrawingSpriteBatch(camera.GetCamera());
            render.Curtain(startMenu.Curtain, camera.NewWidth, camera.NewHeight, backgroundPosition, 0.75f);
            render.MenuText(startMenu.MenuOptions[0], inventoryPosition, font, Color.Gray, 1);
            render.MenuText(startMenu.MenuOptions[1], gameSettingsPosition, font, Color.Gray, 1);
            render.MenuText(startMenu.MenuOptions[2], loadGamePosition, font, Color.Gray, 1);
            render.MenuText(startMenu.MenuOptions[3], exitGamePosition, font, Color.Gray, 1);
            render.FinishDrawingSpriteBatch();

            if (!exitMenu)
            {
                switch (startMenu.NavigateMenu(gameTime))
                {
                    case 0:
                        render.StartDrawingSpriteBatch(camera.GetCamera());
                        render.MenuText(startMenu.MenuOptions[0], inventoryPosition, font, Color.GhostWhite, 1);
                        render.FinishDrawingSpriteBatch();
                        currentMenuItem = startMenu.MenuOptions[0];
                        break;
                    case 1:
                        render.StartDrawingSpriteBatch(camera.GetCamera());
                        render.MenuText(startMenu.MenuOptions[1], gameSettingsPosition, font, Color.GhostWhite, 1);
                        render.FinishDrawingSpriteBatch();
                        currentMenuItem = startMenu.MenuOptions[1];
                        break;
                    case 2:
                        if (File.Exists(SaveFile))
                        {
                            render.StartDrawingSpriteBatch(camera.GetCamera());
                            render.MenuText(startMenu.MenuOptions[2], loadGamePosition, font, Color.GhostWhite, 1);
                            render.FinishDrawingSpriteBatch();
                            currentMenuItem = startMenu.MenuOptions[2];
                        }
                        else
                        {
                            render.StartDrawingSpriteBatch(camera.GetCamera());
                            render.MenuText(startMenu.MenuOptions[2], loadGamePosition, font, Color.LightCoral, 1);
                            render.FinishDrawingSpriteBatch();
                            currentMenuItem = "none";
                        }
                        break;
                    case 3:
                        render.StartDrawingSpriteBatch(camera.GetCamera());
                        render.MenuText(startMenu.MenuOptions[3], exitGamePosition, font, Color.GhostWhite, 1);
                        render.FinishDrawingSpriteBatch();
                        currentMenuItem = startMenu.MenuOptions[3];
                        break;
                }
            }
            else
            {
                render.StartDrawingSpriteBatch(camera.GetCamera());
                render.Curtain(startMenu.Curtain, camera.NewWidth, camera.NewHeight, backgroundPosition, 0.15f);
                render.MenuText(startMenu.MenuOptions[4], toMainMenuPosition, font, Color.Gray, 1.25f);
                render.MenuText(startMenu.MenuOptions[5], toDesktopPosition, font, Color.Gray, 1.25f);
                render.FinishDrawingSpriteBatch();

                switch (startMenu.NavigateMenu(gameTime))
                {
                    case 0:
                        render.StartDrawingSpriteBatch(camera.GetCamera());
                        render.MenuText(startMenu.MenuOptions[4], toMainMenuPosition, font, Color.GhostWhite, 1.25f);
                        render.FinishDrawingSpriteBatch();
                        currentMenuItem = startMenu.MenuOptions[4];
                        break;
                    case 1:
                        render.StartDrawingSpriteBatch(camera.GetCamera());
                        render.MenuText(startMenu.MenuOptions[5], toDesktopPosition, font, Color.GhostWhite, 1.25f);
                        render.FinishDrawingSpriteBatch();
                        currentMenuItem = startMenu.MenuOptions[5];
                        break;
                }
            }
        }

        /*
         * Render game elements in order of y-axis position.
         */
        public void Render(MainGame game, GameTime gameTime, GraphicsDevice GraphicsDevice, GraphicsDeviceManager graphics)
        {
            GraphicsDevice.Clear(backgroundColour); // Clear the graphics buffer and set the window background colour

            switch (currentMapEnum)
            {
                case CurrentMap.MainMenu: // If current screen is MainMenu
                    DrawMainMenu(gameTime);
                    ScreenFadeIn(gameTime, game, GraphicsDevice);
                    break;

                default: // Otherwise default to drawing game
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
                            if (entity.Position.Y == position.Y)
                            {
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

                    if (freezeGame)
                    {
                        DrawStartMenu(gameTime);
                    }

                    ScreenFadeIn(gameTime, game, GraphicsDevice);
                    ScreenCurtainHold(gameTime);
                    ScreenFadeOut(gameTime);
                    break;

            }
        }

        /*
         * Check for player input.
         */
        public void CheckInput(MainGame game, GameTime gameTime, GraphicsDevice GraphicsDevice)
        {
            string button = gamePadInput.CheckButtonInput();
            string key = keyboardInput.CheckKeyInput();
            string input = "none";

            if (button != "none")
            {
                input = button;
            }
            if (key != "none")
            {
                input = key;
            }

            if (menu) // Only applicable within menus
            {
                if (input == "A" || input == "Enter")
                {
                    switch (currentMenuItem)
                    {
                        case "Start new game":
                            FadingIn = true;
                            newGame = true;
                            ScreenFadeIn(gameTime, game, GraphicsDevice);
                            break;
                        case "Continue":
                            FadingIn = true;
                            loadingGame = true;
                            ScreenFadeIn(gameTime, game, GraphicsDevice);
                            break;
                        case "Load game":
                            FadingIn = true;
                            loadingGame = true;
                            ScreenFadeIn(gameTime, game, GraphicsDevice);
                            break;
                        case "Settings":
                            break;
                        case "Inventory":
                            break;
                        case "Exit game":
                            exitMenu = true;
                            startMenu.Select = 0;
                            startMenu.ItemCount = 1;
                            break;
                        case "Exit to main menu":
                            exitMenu = false;
                            InGameScreen = "mainMenu";
                            break;
                        case "Exit to desktop":
                            ExitGame = true;
                            break;
                    }
                }
            }
            if (!menu) // Only applicable outside of menus
            {
                if (input == "Up" || input == "F8")
                {
                    SaveGame(); // Save the current game state
                }
                if (input == "Down" || input == "F9")
                {
                    if (File.Exists(SaveFile))
                    {
                        // *** TO DO *** Change later to add "Load game?" before actually loading *** TO DO ***
                        loadingGame = true; // Game data is being loaded from file
                        FadingIn = true; // The curtain will start fading in
                        ScreenFadeIn(gameTime, game, GraphicsDevice); // Start fading in the curtain
                    }
                    else System.Diagnostics.Debug.WriteLine("No save file found."); // Otherwise write to debug console "No save file found."
                }
            }

            if (input == "Start" || input == "Escape")
            {
                switch (freezeGame) // Freeze the game whilst the menu is open
                {
                    case true: // If the menu is open
                        switch (exitMenu)
                        {
                            case true: // If in the exitMenu
                                exitMenu = false; // Leave the exitMenu
                                break;
                            case false: // Otherwise
                                InGameScreen = "game"; // Go back to the game screen
                                freezeGame = false; // Unfreeze the game
                                menu = false; // Exit the StartMenu
                                break;
                        }
                        break;
                    case false: // Otherwise
                        if (!menu)
                        {
                            InGameScreen = "startMenu"; // Switch to the StartMenu screen
                            freezeGame = true; // Freeze the game
                        }
                        break;
                }
            }
        }
    }
}