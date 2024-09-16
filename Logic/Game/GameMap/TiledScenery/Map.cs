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

        public Map(int map, string floorTileset, string wallTileset)
        {
            Position = Vector2.Zero;
            floorTiles = floorTileset;
            wallTiles = wallTileset;
        }

        /*
         * Load map content.
         */
        public void LoadMap(ContentManager Content, int map, int rows, int columns)
        {
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

            floorTexture = Content.Load<Texture2D>(floorTiles);
            wallTexture = Content.Load<Texture2D>(wallTiles);

            floorAtlas = Texture2DAtlas.Create("background", floorTexture, GetTileDimensions(0, 0), GetTileDimensions(0, 1));
            wallAtlas = Texture2DAtlas.Create("foreground", wallTexture, GetTileDimensions(0, 0), GetTileDimensions(0, 1));

            CreateAtlas(floorTileSet, floorAtlas, totalFloorTiles);
            CreateAtlas(wallTileSet, wallAtlas, totalWallTiles, totalFloorTiles);
            System.Diagnostics.Debug.Write("wall tile start = " + (totalFloorTiles));
            System.Diagnostics.Debug.Write("last wall tile = " + (totalWallTiles + totalFloorTiles));
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
         * Create and return the temple map layout.
         */
        public int[,] GetMapLayout()
        {
            int[,,] map = new int[,,]
            {
                {
                    { -1, 6, 7, 7, 7, 7, 7, 7, 7, 8 },
                    { 4, 0, 2, 2, 2, 3, 2, 2, 2, 9 },
                    { 5, 1, -1, -1, -1, -1, -1, -1, -1, 23 },
                    { 5, -1, -1, -1, -1, -1, -1, -1, -1, 22 },
                    { 5, -1, -1, 12, 11, 11, 13, -1, -1, 9 },
                    { 10, 11, 11, 14, 16, 16, 15, 11, 11, 21 },
                    { 17, 19, 20, 18, -1, -1, 17, 19, 20, 18 }// 0: Temple walls
                },
                {
                    { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
                    { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
                    { -1, 0, 2, 3, 0, 7, 0, 6, 5, -1 },
                    { -1, 0, 0, -1, 0, 0, 0, 0, 1, -1 },
                    { 4, 0, 0, 0, 0, 0, 0, 0, 0, -1 },
                    { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
                    { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 } // 1: Temple floors
                }
            };

            return map;
        }

        public void DebugMap(GraphicsDevice GraphicsDevice, int row = 0, int column = 1)
        {
            int rowNumber = 0;
            int columnNumber = 0;
            int previousColumn = -1;
            int previousRow = -1;

            for (int i = rowNumber; i < tileLocations.GetTempleLayout().GetLength(row); i++)
            {
                System.Diagnostics.Debug.Write(i + ": ");
                for (int j = columnNumber; j < tileLocations.GetTempleLayout().GetLength(column); j++)
                {
                    if (previousRow < 0 & previousColumn < 0)
                    {
                        System.Diagnostics.Debug.Write("First: ");
                    }
                    System.Diagnostics.Debug.Write(tileLocations.GetTempleLayout()[i, j] + ", ");

                    if (previousRow == i & previousColumn >= 0)
                    {
                        System.Diagnostics.Debug.Write("(" + tileLocations.GetTempleLayout()[i, j - 1] + "), ");
                    }
                    else if (previousRow - 1 < i & previousColumn == tileLocations.GetTempleLayout().GetLength(column) - 1)
                    {
                        System.Diagnostics.Debug.Write("(" + tileLocations.GetTempleLayout()[previousRow, previousColumn] + "), ");
                    }
                    previousColumn = j;
                    previousRow = i;
                }
                System.Diagnostics.Debug.Write("\n");
            }
        }
    }
}