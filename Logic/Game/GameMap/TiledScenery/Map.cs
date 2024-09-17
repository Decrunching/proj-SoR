using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;
using System.Collections.Generic;

namespace Logic.Game.GameMap.TiledScenery
{
    /*
     * Create a map using a specified tileset and layout.
     */
    internal class Map
    {
        private Texture2DAtlas floorAtlas;
        private Texture2DAtlas wallAtlas;
        private Dictionary<int, Texture2DRegion> floorTileSet;
        private Dictionary<int, Texture2DRegion> wallTileSet;
        private Texture2D floorTexture;
        private Texture2D wallTexture;
        private string floorTiles;
        private string wallTiles;

        public Vector2 Position { get; set; }

        public Map(string floorTileset, string wallTileset)
        {
            Position = Vector2.Zero;
            floorTiles = floorTileset;
            wallTiles = wallTileset;
        }

        /*
         * Load map content.
         */
        public void LoadMap(ContentManager Content, int rows, int columns)
        {
            floorTexture = Content.Load<Texture2D>(floorTiles);
            wallTexture = Content.Load<Texture2D>(wallTiles);

            int totalFloorTiles = floorTexture.Width / GetTileDimensions(0, 0) * floorTexture.Height / GetTileDimensions(0, 1);
            System.Diagnostics.Debug.Write("total floor tiles = " + totalFloorTiles);

            int totalWallTiles = wallTexture.Width / GetTileDimensions(0, 0) * wallTexture.Height / GetTileDimensions(0, 1);
            System.Diagnostics.Debug.Write("total wall tiles = " + totalWallTiles);

            int floorRows = floorTexture.Width / GetTileDimensions(0, 0);
            System.Diagnostics.Debug.Write("floor tiles per row = " + floorRows);

            int floorColumns = floorTexture.Height / GetTileDimensions(0, 1);
            System.Diagnostics.Debug.Write("floor tiles per column = " + floorColumns);

            int wallRows = wallTexture.Width / GetTileDimensions(0, 0);
            System.Diagnostics.Debug.Write("wall tiles per row = " + wallRows);

            int wallColumns = wallTexture.Height / GetTileDimensions(0, 1);
            System.Diagnostics.Debug.Write("wall tiles per column = " + wallColumns);

            floorAtlas = Texture2DAtlas.Create("background", floorTexture, GetTileDimensions(0, 0), GetTileDimensions(0, 1));
            wallAtlas = Texture2DAtlas.Create("foreground", wallTexture, GetTileDimensions(0, 0), GetTileDimensions(0, 1));

            CreateAtlas(floorTileSet, floorAtlas, totalFloorTiles);
            CreateAtlas(wallTileSet, wallAtlas, totalWallTiles, totalFloorTiles);
            System.Diagnostics.Debug.Write("wall tile start = " + (totalFloorTiles));
            System.Diagnostics.Debug.Write("last wall tile = " + (totalWallTiles + totalFloorTiles));
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
         * Create an atlas from a spritesheet.
         */
        public void CreateAtlas(Dictionary<int, Texture2DRegion> tileSet, Texture2DAtlas atlas, int totalTiles, int offset = 0)
        {
            for (int i = 0; i < totalTiles + offset; i++)
            {
                tileSet.Add(i, atlas[i]);
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