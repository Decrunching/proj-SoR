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
         * Create the main game menu.
         */
        public void GameMainMenu(MainGame game, GraphicsDevice GraphicsDevice, GraphicsDeviceManager graphics)
        {
            CurrentInputScreen = "menu";
            currentMapEnum = CurrentMap.MainMenu;
            mainMenu = new MainMenu(game, graphics);
            LoadGameContent(GraphicsDevice, game);
        }

        /*
         * Create the Village scene.
         */
        public void Village(MainGame game, GraphicsDevice GraphicsDevice)
        {
            CurrentInputScreen = "game";

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
            CurrentInputScreen = "game";

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
}