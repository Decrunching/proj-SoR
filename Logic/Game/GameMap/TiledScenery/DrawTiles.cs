using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;

namespace Logic.Game.GameMap.TiledScenery
{
    /*
     * Create a map using a specified tileset and layout.
     */
    internal class DrawTiles
    {
        private Texture2D floorTexture;
        private Texture2D wallTexture;
        private Rectangle targetRectangle;
        private string floorTiles;
        private string wallTiles;
        private int mapWidth;
        private int mapHeight;
        public Vector2 Position { get; set; }

        public DrawTiles(int map, string floorTileset, string wallTileset)
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
            mapWidth = 64 * rows;
            mapHeight = 64 * columns;

            targetRectangle = new Rectangle(0, 0, mapWidth, mapHeight);

            floorTexture = Content.Load<Texture2D>(floorTiles);
            wallTexture = Content.Load<Texture2D>(wallTiles);
        }

        /*
         * 
         */
        public void DebugMap(GraphicsDevice GraphicsDevice, int row = 0, int column = 1)
        {
            int rowNumber = 0;
            int columnNumber = 0;
            int previousColumn = -1;
            int previousRow = -1;

            for (int i = rowNumber; i < tileLocations.GetTempleLayout().GetLength(row); i++)
            {
                for (int j = columnNumber; j < tileLocations.GetTempleLayout().GetLength(column); j++)
                {
                    if (previousRow < 0 & previousColumn < 0)
                    {

                    }

                    if (previousRow == i & previousColumn >= 0)
                    {

                    }
                    else if (previousRow - 1 < i & previousColumn == tileLocations.GetTempleLayout().GetLength(column) - 1)
                    {

                    }
                    previousColumn = j;
                    previousRow = i;
                }
            }
        }

        /*
         * Draw the map.
         */
        public void DrawMap(SpriteBatch spriteBatch, Vector2 location, int tileWidth, int tileHeight)
        {
            Rectangle sourceRectangle = new Rectangle(tileWidth * column, tileHeight * row, tileWidth, tileHeight);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, tileWidth, tileHeight);

            int width = TileSet.Width / Columns;
            int height = TileSet.Height / Rows;
            int row = currentTile / Columns;
            int column = currentTile % Columns;

            if (currentTile != totalTiles)
            {
                currentTile++;
            }




            spriteBatch.Begin();


            spriteBatch.Draw(Map, location, targetRectangle, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);


            spriteBatch.End();
        }
    }
}