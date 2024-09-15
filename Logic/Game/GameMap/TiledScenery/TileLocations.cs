using System.Drawing;

namespace Logic.Game.GameMap.TiledScenery
{
    /*
     * Tiles are mapped to multidimnesional arrays, with each element corresponding to a tile skin.
     */
    public class TileLocations
    {
        private string[,] temple;
        private Rectangle[,] templeRectangle;

        public TileLocations() { }

        /*
         * Create and return the temple map layout.
         */
        public string[,] GetTempleLayout()
        {
            temple = new string[,]
            {   // 0     1     2     3     4     5     6     7     8     9
                { "0", "11", "12", "12", "12", "12", "12", "12", "12", "13" },  // 0
                { "9", "2", "4", "4", "4", "5", "4", "4", "4", "14" },          // 1
                { "10", "3", "31", "32", "1", "33", "1", "34", "27", "28" },    // 2
                { "10", "1", "1", "0", "1", "1", "1", "1", "29", "30" },        // 3
                { "10", "35", "1", "17", "16", "16", "18", "1", "1", "14" },    // 4
                { "15", "16", "16", "19", "21", "21", "20", "16", "16", "26" }, // 5
                { "22", "24", "25", "23", "0", "0", "22", "24", "25", "23" }    // 6 // 0: Temple
            };

            return temple;
        }

        /*
         * 
         */
        public Rectangle[,] GetTemple()
        {
            templeRectangle = new Rectangle[,]
            {
                { new Rectangle(0, 0, 64, 64), new Rectangle(64, 0, 64, 64), new Rectangle(0, 0, 64, 64) }
            };
        }

        /*
         * Get the required tileset.
         */
        public string UseTileset(int row, int column)
        {
            string[,] maps = new string[,]
            {
                { "SoR Resources/Locations/TiledScenery/Temple/floorSpritesheet",
                    "SoR Resources/Locations/TiledScenery/Temple/wallSpritesheet" } // 0: Temple
            };

            return maps[row, column];
        }

        /*
         * Get the width and height of each tile in this set.
         */
        public int GetTileDimensions(int row, int column)
        {
            int[,] dimensions = new int[,]
            {
                { 64, 64 } // 0: Temple
            };

            return dimensions[row, column];
        }
    }
}