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

namespace SoR.Logic
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
        private bool hasUpperWalls;
        private bool hasFloorDecor;
        private bool fadingIn;
        private bool curtainUp;
        private bool fadingOut;
        private float fadeAlpha;
        private float timer;
        public Dictionary<string, Entity> Entities { get; set; }
        public Dictionary<string, Scenery> Scenery { get; set; }
        public string CurrentInputScreen { get; set; }
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
        public GameLogic(MainGame game, GraphicsDevice GraphicsDevice)
        {
            camera = new Camera(game.Window, GraphicsDevice, 800, 600);
            gamePadInput = new GamePadInput();
            keyboardInput = new KeyboardInput();

            CurrentMapString = "mainMenu";
            CurrentInputScreen = "none";
            hasFloorDecor = false;
            hasUpperWalls = false;

            fadingIn = false;
            curtainUp = false;
            fadingOut = false;
            fadeAlpha = 0f;
            timer = 0f;
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
                    CurrentMapString = "village";
                    break;
                case CurrentMap.Temple:
                    CurrentMapString = "temple";
                    break;
            }
            GameState.SaveFile(player, CurrentMapString);
            CurrentMapString = "none";
        }

        /*
         * Load the game from a file.
         */
        public void LoadGame(MainGame game, GraphicsDevice GraphicsDevice)
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
        }

        /*
         * Fade in the curtain.
         */
        public void ScreenFadeIn(MainGame game, GameTime gameTime, GraphicsDevice GraphicsDevice)
        {
            if (fadingIn)
            {
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                float fadeInTime = 0.3f;
                timer += deltaTime;
                fadeAlpha += deltaTime * 3.33f;

                if (timer < fadeInTime)
                {
                    timer += deltaTime;

                    if (fadeAlpha > 1f)
                    {
                        fadeAlpha = 1f;
                    }

                    render.StartDrawingSpriteBatch(camera.GetCamera());
                    render.MainMenuBackground(GraphicsDevice, mainMenu.Curtain, fadeAlpha);
                    render.FinishDrawingSpriteBatch();
                }

                if (timer >= fadeInTime)
                {
                    render.StartDrawingSpriteBatch(camera.GetCamera());
                    render.MainMenuBackground(GraphicsDevice, mainMenu.Curtain);
                    render.FinishDrawingSpriteBatch();

                    fadeAlpha = 1f;
                    timer = 0f;
                    fadingIn = false;
                    curtainUp = true;
                }
            }
        }

        /*
         * Hold the curtain in place.
         */
        public void ScreenCurtainHold(GameTime gameTime, GraphicsDevice GraphicsDevice)
        {
            if (curtainUp)
            {
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                float curtainTime = 0.5f;
                timer += deltaTime;

                render.StartDrawingSpriteBatch(camera.GetCamera());
                render.MainMenuBackground(GraphicsDevice, mainMenu.Curtain);
                render.FinishDrawingSpriteBatch();

                if (timer >= curtainTime)
                {
                    timer = 0f;
                    curtainUp = false;
                    fadingOut = true;
                }
            }
        }

        /*
         * Fade out the curtain.
         */
        public void ScreenFadeOut(GameTime gameTime, GraphicsDevice GraphicsDevice)
        {
            if (fadingOut)
            {
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                float fadeOutTime = 1f;
                timer += deltaTime;
                fadeAlpha -= deltaTime;

                if (timer < fadeOutTime)
                {
                    if (fadeAlpha < 0f)
                    {
                        fadeAlpha = 0f;
                    }
                    render.StartDrawingSpriteBatch(camera.GetCamera());
                    render.MainMenuBackground(GraphicsDevice, mainMenu.Curtain, fadeAlpha);
                    render.FinishDrawingSpriteBatch();
                }

                if (timer >= fadeOutTime)
                {
                    render.StartDrawingSpriteBatch(camera.GetCamera());
                    render.MainMenuBackground(GraphicsDevice, mainMenu.Curtain, 0f);
                    render.FinishDrawingSpriteBatch();

                    fadeAlpha = 0f;
                    timer = 0f;
                    fadingOut = false;
                }
            }
        }

        /*
         * Save or load game data according to player input.
         */
        public void SaveLoadInput(MainGame game, GameTime gameTime, GraphicsDevice GraphicsDevice)
        {
            if (CurrentInputScreen == "game")
            {
                switch (gamePadInput.CheckButtonInput())
                {
                    case "Up":
                        SaveGame();
                        break;
                    case "Down":
                        fadingIn = true;

                        ScreenFadeIn(game, gameTime, GraphicsDevice);
                        LoadGame(game, GraphicsDevice);

                        CurrentInputScreen = "game";
                        break;
                }

                switch (keyboardInput.CheckKeyInput())
                {
                    case "F8":
                        SaveGame();
                        break;
                    case "F9":
                        fadingIn = true;

                        ScreenFadeIn(game, gameTime, GraphicsDevice);
                        LoadGame(game, GraphicsDevice);

                        CurrentInputScreen = "game";
                        break;
                }
            }
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

                camera.FollowPlayer(player.Position);

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
         * Render game elements in order of y-axis position.
         */
        public void Render(MainGame game, GameTime gameTime, GraphicsDevice GraphicsDevice)
        {
            switch (currentMapEnum)
            {
                case CurrentMap.MainMenu:
                    GraphicsDevice.Clear(backgroundColour); // Clear the graphics buffer and set the window background colour
                    render.StartDrawingSpriteBatch(camera.GetCamera());
                    render.MainMenuBackground(GraphicsDevice, mainMenu.Curtain);
                    render.MainMenuText(mainMenu.MenuOptions[0], mainMenu.TitlePosition, font, Color.GhostWhite, 2.5f);
                    render.MainMenuText(mainMenu.MenuOptions[1], mainMenu.NewGamePosition, font, Color.Gray, 1);
                    render.MainMenuText(mainMenu.MenuOptions[2], mainMenu.ContinueGamePosition, font, Color.Gray, 1);
                    render.MainMenuText(mainMenu.MenuOptions[3], mainMenu.LoadGamePosition, font, Color.Gray, 1);
                    render.MainMenuText(mainMenu.MenuOptions[4], mainMenu.GameSettingsPosition, font, Color.Gray, 1);
                    render.FinishDrawingSpriteBatch();

                    switch (mainMenu.NavigateMenu(gameTime))
                    {
                        case 0:
                            render.StartDrawingSpriteBatch(camera.GetCamera());
                            render.MainMenuText(mainMenu.MenuOptions[1], mainMenu.NewGamePosition, font, Color.GhostWhite, 1);
                            render.FinishDrawingSpriteBatch();
                            if (gamePadInput.CheckButtonInput() == "A" || keyboardInput.CheckKeyInput() == "Enter")
                            {
                                Village(game, GraphicsDevice);
                            }
                            break;
                        case 1:
                            render.StartDrawingSpriteBatch(camera.GetCamera());
                            render.MainMenuText(mainMenu.MenuOptions[2], mainMenu.ContinueGamePosition, font, Color.GhostWhite, 1);
                            render.FinishDrawingSpriteBatch();
                            if (gamePadInput.CheckButtonInput() == "A" || keyboardInput.CheckKeyInput() == "Enter")
                            {
                                fadingIn = true;

                                ScreenFadeIn(game, gameTime, GraphicsDevice);
                                LoadGame(game, GraphicsDevice);

                                CurrentInputScreen = "game";
                            }
                            break;
                        case 2:
                            render.StartDrawingSpriteBatch(camera.GetCamera());
                            render.MainMenuText(mainMenu.MenuOptions[3], mainMenu.LoadGamePosition, font, Color.GhostWhite, 1);
                            render.FinishDrawingSpriteBatch();
                            if (gamePadInput.CheckButtonInput() == "A" || keyboardInput.CheckKeyInput() == "Enter")
                            {
                                fadingIn = true;

                                ScreenFadeIn(game, gameTime, GraphicsDevice);
                                LoadGame(game, GraphicsDevice);

                                CurrentInputScreen = "game";
                            }
                            break;
                        case 3:
                            render.StartDrawingSpriteBatch(camera.GetCamera());
                            render.MainMenuText(mainMenu.MenuOptions[4], mainMenu.GameSettingsPosition, font, Color.GhostWhite, 1);
                            render.FinishDrawingSpriteBatch();
                            if (gamePadInput.CheckButtonInput() == "A" || keyboardInput.CheckKeyInput() == "Enter")
                            {
                                // TBD
                            }
                            break;
                    }
                    break;

                default:
                    GraphicsDevice.Clear(backgroundColour); // Clear the graphics buffer and set the window background colour

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

                    ScreenFadeIn(game, gameTime, GraphicsDevice);
                    ScreenCurtainHold(gameTime, GraphicsDevice);
                    ScreenFadeOut(gameTime, GraphicsDevice);
                    break;

            }
        }
    }
}