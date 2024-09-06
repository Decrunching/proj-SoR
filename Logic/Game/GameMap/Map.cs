using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;
using SoR;

namespace Logic.Game.GameMap
{
    internal class Map
    {
        protected Point TileSize { get; private set; }
        protected Point MapSize { get; private set; }
        private readonly Point mapTileSize = new(4, 3);
        private readonly Sprite[,] tiles;

        public Map()
        {



            /*tiles = new Sprite[mapTileSize.X, mapTileSize.Y];

            List<Texture2D> textures = new(5);
            for (int i = 1; i < 6; i++) textures.Add(Globals.Content.Load<Texture2D>($"tile{i}"));

            TileSize = new(textures[0].Width, textures[0].Height);
            MapSize = new(TileSize.X * mapTileSize.X, TileSize.Y * mapTileSize.Y);

            Random random = new();

            for (int y = 0 ; y < mapTileSize.Y; y++)
            {
                for (int x = 0; x < mapTileSize.X; x++)
                {
                    int r = random.Next(0, textures.Count);
                    //tiles[x, y] = new(textures[r], new(x * TileSize.X, y * TileSize.Y));
                }
            }*/
        }

        public void DrawMap()
        {
            for (int y = 0; y < mapTileSize.Y; y++)
            {
                //for (int x = 0; x < mapTileSize.X; x++) tiles[x, y].Draw();
            }
        }
    }
}
