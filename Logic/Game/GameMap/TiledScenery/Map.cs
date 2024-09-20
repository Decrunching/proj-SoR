using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;

namespace Logic.Game.GameMap.TiledScenery
{
    /*
     * Create a map using a specified tileset and layout.
     */
    public class Map
    {
        private Texture2DAtlas floorAtlas;
        private Texture2DAtlas wallAtlas;
        private Texture2D floorTexture;
        private Texture2D wallTexture;
        private string floorTiles;
        private string wallTiles;
        private int mapNumber;
        public int[,] MapFloor { get; set; }
        public int[,] MapWalls { get; set; }
        public Rectangle BoundingArea { get; set; }

        public Map(int mapNumber)
        {
            this.mapNumber = mapNumber;
            GetMapLayout();
        }

        /*
         * Load map content.
         */
        public void LoadMap(ContentManager Content, string floorTileset, string wallTileset)
        {
            floorTiles = floorTileset;
            wallTiles = wallTileset;

            floorTexture = Content.Load<Texture2D>(floorTiles);
            wallTexture = Content.Load<Texture2D>(wallTiles);

            int totalFloorTiles = floorTexture.Width / GetTileDimensions(0, 0) * floorTexture.Height / GetTileDimensions(0, 1);
            int totalWallTiles = wallTexture.Width / GetTileDimensions(0, 0) * wallTexture.Height / GetTileDimensions(0, 1);
            int floorRows = floorTexture.Width / GetTileDimensions(0, 0);
            int floorColumns = floorTexture.Height / GetTileDimensions(0, 1);
            int wallRows = wallTexture.Width / GetTileDimensions(0, 0);
            int wallColumns = wallTexture.Height / GetTileDimensions(0, 1);

            floorAtlas = Texture2DAtlas.Create("background", floorTexture, GetTileDimensions(0, 0), GetTileDimensions(0, 1));
            wallAtlas = Texture2DAtlas.Create("foreground", wallTexture, GetTileDimensions(0, 0), GetTileDimensions(0, 1));
        }

        /*
         * Create and return the temple wall layout.
         */
        public void GetMapLayout()
        {
            switch (mapNumber)
            {
                case 0:
                    MapWalls = new int[,]
                    {
                        { -1,  6,  7,  7,  7,  7,  7,  7,  7,  8  },
                        {  4,  0,  2,  2,  2,  3,  2,  2,  2,  9  },
                        {  5,  1, -1, -1, -1, -1, -1, -1, -1,  23 },
                        {  5, -1, -1, -1, -1, -1, -1, -1, -1,  22 },
                        {  5, -1, -1,  12, 11, 11, 13,-1, -1,  9  },
                        {  10, 11, 11, 14, 16, 16, 15, 11, 11, 21 },
                        {  17, 19, 20, 18,-1, -1,  17, 19, 20, 18 }
                    };
                    MapFloor = new int[,]
                    {
                        { -1, -1, -1, -1, -1,  0, -1, -1, -1, -1 },
                        { -1, -1, -1, -1, -1,  0, -1, -1, -1, -1 },
                        { -1,  0,  2,  3,  0,  7,  0,  6,  5, -1 },
                        { -1,  0,  0,  0,  0,  0,  0,  0,  1, -1 },
                        { -1,  0,  0,  0,  0,  0,  0,  0,  0, -1 },
                        { -1,  4,  0, -1, -1, -1, -1,  0,  0, -1 },
                        { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 }
                    };

                    int width = GetTileDimensions(0, 0);
                    int height = GetTileDimensions(0, 1);

                    BoundingArea = new Rectangle(
                        (width * 2) - (int)(width * 0.5),
                        height - (int)(height * 1.25),
                        width * 8,
                        (int)(height * 0.5));

                    break;
            }
        }

        /*
         * if MapFloor > -1
         * walkable = > tile.X, < tile.X + width, > tile.Y, < tile.Y + height
         * 
         * walkableList = [ x,y, +w,+h ]
         */

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

        /*
         * Get the floor atlas.
         */
        public Texture2DAtlas GetFloorAtlas()
        {
            return floorAtlas;
        }

        /*
         * Get the wall atlas.
         */
        public Texture2DAtlas GetWallAtlas()
        {
            return wallAtlas;
        }
    }
}