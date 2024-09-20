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
        public string FloorSpriteSheet { get; set; }
        public string WallSpriteSheet { get; set; }
        public int[,] MapFloor { get; set; }
        public int[,] MapWalls { get; set; }
        public int Width {  get; set; }
        public int Height { get; set; }

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

            int totalFloorTiles = floorTexture.Width / Width * floorTexture.Height / Height;
            int totalWallTiles = wallTexture.Width / Width * wallTexture.Height / Height;
            int floorRows = floorTexture.Width / Width;
            int floorColumns = floorTexture.Height / Height;
            int wallRows = wallTexture.Width / Width;
            int wallColumns = wallTexture.Height / Height;

            floorAtlas = Texture2DAtlas.Create("background", floorTexture, Width, Height);
            wallAtlas = Texture2DAtlas.Create("foreground", wallTexture, Width, Height);
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
                        { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
                        { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
                        { -1,  0,  2,  3,  0,  7,  0,  6,  5, -1 },
                        { -1,  0,  0,  0,  0,  0,  0,  0,  1, -1 },
                        { -1,  0,  0,  0,  0,  0,  0,  0,  0, -1 },
                        { -1,  4,  0, -1, -1, -1, -1,  0,  0, -1 },
                        { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 }
                    };

                    Width = 64;
                    Height = 64;

                    FloorSpriteSheet = "SoR Resources/Locations/TiledScenery/Temple/floorSpritesheet";
                    WallSpriteSheet = "SoR Resources/Locations/TiledScenery/Temple/wallSpritesheet";

                    break;
            }
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