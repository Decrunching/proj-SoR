using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SoR.Logic.UI;
using SoR.Logic.GameMap.TiledScenery;

namespace SoR.Logic
{
    /*
     * Create and position the elements of each map area, and remove them on scene transition.
     */
    public partial class GameLogic
    {
        /*
         * The game map currently in use.
         */
        enum CurrentMap
        {
            MainMenu,
            Village,
            Temple
        }

        /*
         * Fade in the curtain.
         */
        public void ScreenFadeIn(GameTime gameTime, MainGame game, GraphicsDevice GraphicsDevice)
        {
            if (FadingIn)
            {
                float deltaTime = GetTime(gameTime);
                float fadeInTime = 0.3f;
                curtainTimer += deltaTime;
                fadeAlpha += deltaTime * 3.33f;

                if (curtainTimer < fadeInTime)
                {
                    curtainTimer += deltaTime;

                    if (fadeAlpha > 1f)
                    {
                        fadeAlpha = 1f;
                    }

                    DrawCurtain(GraphicsDevice, mainMenu.Curtain, fadeAlpha);
                }

                if (curtainTimer >= fadeInTime)
                {
                    DrawCurtain(GraphicsDevice, mainMenu.Curtain);

                    if (newGame && FadingIn)
                    {
                        Village(game, GraphicsDevice);
                    }
                    if (loadingGame && FadingIn)
                    {
                        LoadGame(game, gameTime, GraphicsDevice);
                    }

                    fadeAlpha = 1f;
                    curtainTimer = 0f;
                    FadingIn = false;
                    CurtainUp = true;
                }
            }
        }

        /*
         * Hold the curtain in place.
         */
        public void ScreenCurtainHold(GameTime gameTime, GraphicsDevice GraphicsDevice)
        {
            if (CurtainUp)
            {
                float deltaTime = GetTime(gameTime);
                float curtainTime = 0.5f;
                curtainTimer += deltaTime;

                DrawCurtain(GraphicsDevice, mainMenu.Curtain);

                if (curtainTimer >= curtainTime)
                {
                    curtainTimer = 0f;
                    CurtainUp = false;
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
                float deltaTime = GetTime(gameTime);
                float fadeOutTime = 1f;
                curtainTimer += deltaTime;
                fadeAlpha -= deltaTime;

                if (curtainTimer < fadeOutTime)
                {
                    if (fadeAlpha < 0f)
                    {
                        fadeAlpha = 0f;
                    }

                    DrawCurtain(GraphicsDevice, mainMenu.Curtain, fadeAlpha);
                }

                if (curtainTimer >= fadeOutTime)
                {
                    DrawCurtain(GraphicsDevice, mainMenu.Curtain, 0f);

                    fadeAlpha = 0f;
                    curtainTimer = 0f;
                    fadingOut = false;
                }
            }
        }

        /*
         * Set up the main game menu.
         */
        public void GameMainMenu(MainGame game, GraphicsDevice GraphicsDevice, GraphicsDeviceManager graphics)
        {
            menu = true;
            InGameScreen = "none";
            mainMenu = new MainMenu(game, graphics);
            currentMapEnum = CurrentMap.MainMenu;
            LoadGameContent(GraphicsDevice, game);
        }

        /*
         * Set up the main game menu.
         */
        public void GameStartMenu(MainGame game, GraphicsDevice GraphicsDevice, GraphicsDeviceManager graphics)
        {
            menu = true;
            InGameScreen = "game";
            startMenu = new StartMenu(game, graphics);
            LoadGameContent(GraphicsDevice, game);
        }

        /*
         * Create the Village scene.
         */
        public void Village(MainGame game, GraphicsDevice GraphicsDevice)
        {
            menu = false;
            newGame = false;
            InGameScreen = "game";

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

        /*
         * Create the Temple scene.
         */
        public void Temple(MainGame game, GraphicsDevice GraphicsDevice)
        {
            menu = false;
            InGameScreen = "game";

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
            CreateScenery(GraphicsDevice, 224, 160);
        }
    }
}