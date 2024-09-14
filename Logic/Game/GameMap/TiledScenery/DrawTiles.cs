using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Logic.Game.GameMap.TiledScenery
{
    /*
     * Create a map using a specified tileset and layout.
     */
    internal class DrawTiles
    {
        private Rectangle targetRectangle;
        private int tileCountWidth;
        private int tileCountHeight;
        private int currentTile;
        private int totalTiles;
        public Texture2D TileSet { get; set; }
        public Vector2 Position { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }


        public DrawTiles(int map, int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Position = Vector2.Zero;
            tileCountWidth = 2;
            tileCountHeight = 2;
            currentTile = 0;
            totalTiles = 0;
        }

        /*
         * 
         */
        public void LoadMap(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            TileSet = Content.Load<Texture2D>("SoR Resources/Locations/TiledScenery/Temple/spritesheet");
            targetRectangle = new Rectangle(0, 0, TileSet.Width * tileCountWidth, TileSet.Height * tileCountHeight);
            Viewport viewport = GraphicsDevice.Viewport;
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
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.Opaque, SamplerState.LinearWrap,
                DepthStencilState.Default, RasterizerState.CullNone);
            spriteBatch.Draw(TileSet, Vector2.Zero, targetRectangle, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            spriteBatch.End();
        }
    }
}