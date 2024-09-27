using Logic.Game.GameMap.TiledScenery;
using Logic.Game.GameMap;
using System.Collections.Generic;

namespace Logic.Game.Screens
{
    internal class Stage
    {
        /*
         * Placeholder class for game stages. Should decide which map tiles and layout
         * to use, which mobs/NPCs to load and where, and should adjust according to
         * game progression within a single function for the area of the game being
         * loaded that takes input from elsewhere about the current state of the game.
         */

        private EntityType entityType;
        private SceneryType sceneryType;
        private Map map;
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

        public Stage ()
        {
            // Create dictionaries for game components
            entities = new Dictionary<string, Entity>();
            scenery = new Dictionary<string, Scenery>();
        }
    }
}