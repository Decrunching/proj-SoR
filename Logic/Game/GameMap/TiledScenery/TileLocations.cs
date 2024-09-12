namespace Logic.Game.GameMap.TiledScenery
{/*
  * Tiles are mapped to multidimnesional arrays, with each element corresponding to a tile skin.
  */
    public class TileLocations
    {
        private string[,] temple;

        public TileLocations()
        {
            temple = new string[,]
            {
                { "", "11", "12", "12", "12", "12", "12", "12", "12", "13" },
                { "9", "2", "4", "4", "4", "5", "4", "4", "4", "14" },
                { "10", "3", "31", "32", "1", "33", "1", "34", "27", "28" },
                { "10", "1", "1", "", "1", "1", "1", "1", "29", "30" },
                { "10", "35", "1", "17", "16", "16", "18", "1", "1", "14" },
                { "15", "16", "16", "19", "21", "21", "20", "16", "16", "26" },
                { "22", "24", "25", "23", "", "", "22", "24", "25", "23" }
            };
        }

        /*
         * Get the temple map layout.
         */
        public string[,] GetTempleLayout()
        {
            return temple;
        }
    }
}