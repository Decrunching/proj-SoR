using Logic.Game.GameMap;
using Logic.Game.GameMap.TiledScenery;
using Logic.Game.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoR.Logic.Entities;
using System.Collections.Generic;

namespace Logic.Game.Screens
{
    /*
     * Placeholder class for game stages. Should decide which map tiles and layout
     * to use, which mobs/NPCs to load and where, and should adjust according to
     * game progression within a single function for the area of the game being
     * loaded that takes input from elsewhere about the current state of the game.
     */
    public partial class GameLogic
    {
        private EntityType entityType;
        private SceneryType sceneryType;
        private Map map;
        private Render render;
        private Camera camera;
        private SpriteFont font;
        private Dictionary<string, Entity> entities;
        private Dictionary<string, Scenery> scenery;
        private Dictionary<string, Vector2> mapLowerWalls;
        private Dictionary<string, Vector2> mapUpperWalls;
        private Dictionary<string, Vector2> mapFloor;
        private List<Vector2> positions;
        private List<Rectangle> impassableArea;

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
            // Set up the camera
            camera = new Camera(Window, GraphicsDevice, 800, 600);

            // Create dictionaries for interactable game components
            entities = [];
            scenery = [];
        }

        /*
         * 
         */
        public void Village(GraphicsDevice GraphicsDevice)
        {
            // Get the map to be used
            map = new Map(1);

            // Create entities
            entityType = EntityType.Player;
            CreateEntity(GraphicsDevice, 250, 200);

            entityType = EntityType.Chara;
            CreateEntity(GraphicsDevice, 270, 200);

            entityType = EntityType.Pheasant;
            CreateEntity(GraphicsDevice, 250, 250);
        }

        /*
         * 
         */
        public void Temple(GraphicsDevice GraphicsDevice)
        {
            // Get the map to be used
            map = new Map(0);

            // Create entities
            entityType = EntityType.Player;
            CreateEntity(GraphicsDevice, 250, 130);

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