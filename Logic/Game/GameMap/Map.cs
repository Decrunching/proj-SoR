using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Logic.Game.GameMap
{/*
  * Tiles are mapped to jagged arrays, with each array element corresponding to a map tile skin.
  */
    public class Map
    {
        private string[][] temple;

        public Map()
        {
            temple =
                [
                ["", "11", "12", "12", "12", "12", "12", "12", "12", "13"],
                ["9", "2", "4", "4", "4", "5", "4", "4", "4", "14"],
                ["10", "3", "31", "32", "1", "33", "1", "34", "27", "28"],
                ["10", "1", "", "1", "1", "1", "1", "29", "30"],
                ["10", "35", "1", "17", "16", "16", "18", "1", "1", "14", ],
                ["15", "16", "16", "19", "21", "21", "20", "16", "16", "26", ],
                ["22", "24", "25", "23", "", "", "22", "24", "25", "23", ]
                ];
        }

        /*
         * Get the temple map.
         */
        public string[][] GetTemple()
        {
            return temple;
        }
    }
}