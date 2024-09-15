﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;

namespace Logic.Game.GameMap.TiledScenery
{
    /*
     * Create a map using a specified tileset and layout.
     */
    internal class DrawTiles
    {
        private Texture2D floorTexture;
        private Texture2D wallTexture;
        private Texture2DRegion textureRegion;
        private Rectangle targetRectangle;
        private string floorTiles;
        private string wallTiles;
        private int mapWidth;
        private int mapHeight;
        private int totalTiles;
        private int rowLength;
        private int columnLength;
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
            totalTiles = rows * columns;
            rowLength = rows;
            columnLength = columns;

            targetRectangle = new Rectangle(-mapWidth / 2, -mapHeight / 2, mapWidth, mapHeight);

            floorTexture = Content.Load<Texture2D>(floorTiles);
            wallTexture = Content.Load<Texture2D>(wallTiles);
        }

        /*
         * Get the next tile from the spritesheet. Try using instead of a new rectangle for each spritesheet region.
         */
        public Rectangle GetNextTile(int xStart, int yStart, int tileWidth, int tileHeight)
        {
            Rectangle rectangle = new Rectangle(xStart, yStart, tileWidth, tileHeight);

            return rectangle;
        }

        /*
         * Draw the map.
         */
        public void DrawMap(Texture2D tileSet, SpriteBatch spriteBatch, Vector2 location, int xStart, int yStart, int tileWidth, int tileHeight)
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

        }
    }
}