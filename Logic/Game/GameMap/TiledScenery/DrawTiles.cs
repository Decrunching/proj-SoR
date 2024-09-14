using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Logic.Game.GameMap.TiledScenery
{
    /*
     * 
     */
    internal class DrawTiles
    {
        private Vector2 position;
        private int tileCountWidth;
        private int tileCountHeight;
        private int currentTile;
        private int totalTiles;
        public Texture2D TileSet { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }


        public DrawTiles(Texture2D texture, int map, int rows, int columns)
        {
            TileSet = texture;
            Rows = rows;
            Columns = columns;
            position = Vector2.Zero;
            tileCountWidth = 2;
            tileCountHeight = 2;
            currentTile = 0;
            totalTiles = 0;
        }

        /*
         * 
         */
        public void LoadMap()
        {

        }

        /*
         * 
         */
        public void UpdateMap() { }

        /*
         * 
         */
        public void DrawMap(SpriteBatch spriteBatch, Vector2 location)
        {
            int width = TileSet.Width / Columns;
            int height = TileSet.Height / Rows;
            int row = currentTile / Columns;
            int column = currentTile % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            if (currentTile != totalTiles)
            {
                currentTile++;
            }
            spriteBatch.Begin();
            spriteBatch.Draw(TileSet, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
        }
    }
}