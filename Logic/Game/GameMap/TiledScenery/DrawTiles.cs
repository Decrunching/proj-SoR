using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Logic.Game.GameMap.TiledScenery
{
    /*
     * 
     */
    internal class DrawTiles
    {
        public Texture2D Texture {  get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }

        private int currentTile;
        private int totalTiles;

        public DrawTiles(Texture2D texture, int rows, int columns)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
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
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = currentTile / Columns;
            int column = currentTile % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            if (currentTile != totalTiles)
            {
                currentTile++;
            }
            spriteBatch.Begin();
            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
        }
    }
}