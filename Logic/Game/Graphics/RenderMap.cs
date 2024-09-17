namespace Logic.Game.Graphics
{
    internal partial class Render
    {
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
         * Create and return the temple map layout.
         */
        public int[,] GetTempleWalls()
        {
            int[,] map = new int[,]
            {
                { -1, 6, 7, 7, 7, 7, 7, 7, 7, 8 },
                { 4, 0, 2, 2, 2, 3, 2, 2, 2, 9 },
                { 5, 1, -1, -1, -1, -1, -1, -1, -1, 23 },
                { 5, -1, -1, -1, -1, -1, -1, -1, -1, 22 },
                { 5, -1, -1, 12, 11, 11, 13, -1, -1, 9 },
                { 10, 11, 11, 14, 16, 16, 15, 11, 11, 21 },
                { 17, 19, 20, 18, -1, -1, 17, 19, 20, 18 }
            };

            return map;
        }

        /*
         * Get the Temple floors.
         */
        public int[,] GetTempleFloor()
        {
            int[,] map = new int[,]
            {
                { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
                { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
                { -1, 0, 2, 3, 0, 7, 0, 6, 5, -1 },
                { -1, 0, 0, -1, 0, 0, 0, 0, 1, -1 },
                { 4, 0, 0, 0, 0, 0, 0, 0, 0, -1 },
                { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
                { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 }
            };

            return map;
        }
    }
}
