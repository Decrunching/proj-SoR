using Logic.Entities.Character;
using Logic.Game.GameMap;
using Logic.Game.GameMap.TiledScenery;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;
using Spine;
using System;
using System.Collections.Generic;

namespace Logic.Game.Graphics
{
    /*
     * Draw graphics to the screen, collect the impassable sections of the map, and convert map arrays into atlas positions for drawing tiles.
     */
    internal partial class Render
    {
        private SpriteBatch spriteBatch;
        private SkeletonRenderer skeletonRenderer;
        public List<Rectangle> ImpassableTiles { get; private set; }

        /*
         * Initialise the SpriteBatch, SkeletonRenderer and ImpassableTiles collection.
         */
        public Render(GraphicsDevice GraphicsDevice)
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            skeletonRenderer = new SkeletonRenderer(GraphicsDevice)
            {
                PremultipliedAlpha = true
            };

            ImpassableTiles = [];
        }

        /*
         * Start drawing skeletons.
         */
        public void StartDrawingSkeleton(GraphicsDevice GraphicsDevice, Camera camera)
        {
            ((BasicEffect)skeletonRenderer.Effect).Projection = Matrix.CreateOrthographicOffCenter(
                    0,
                        GraphicsDevice.Viewport.Width,
                        GraphicsDevice.Viewport.Height,
                        0, 1, 0);
            ((BasicEffect)skeletonRenderer.Effect).View = camera.GetCamera().GetViewMatrix();

            skeletonRenderer.Begin();
        }

        /*
         * Start drawing SpriteBatch.
         */
        public void StartDrawingSpriteBatch(OrthographicCamera camera)
        {
            spriteBatch.Begin(transformMatrix: camera.GetViewMatrix(), samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
        }

        /*
         * Draw entity skeletons.
         */
        public void DrawEntitySkeleton(Entity entity)
        {
            // Draw skeletons
            skeletonRenderer.Draw(entity.GetSkeleton());
        }

        /*
         * Draw scenery skeletons.
         */
        public void DrawScenerySkeleton(Scenery scenery)
        {
            // Draw skeletons
            skeletonRenderer.Draw(scenery.GetSkeleton());
        }

        /*
         * Draw SpriteBatch for entities.
         */
        public void DrawEntitySpriteBatch(Entity entity, SpriteFont font)
        {
            // Entity text
            spriteBatch.DrawString(
                font,
                "HP: " + entity.GetHitPoints(),
                new Vector2(entity.GetPosition().X - 30, entity.GetPosition().Y + 30),
                Color.BlueViolet);
        }

        /*
         * Draw the text for the main menu.
         */
        public void MainMenuText(string menuItem, Vector2 position, SpriteFont font, Color colour, float scale)
        {
            // Entity text
            spriteBatch.DrawString(
            font,
                menuItem,
                new Vector2(position.X, position.Y),
                colour,
                0,
                new Vector2(0, 0),
                scale,
                SpriteEffects.None,
                0);
        }

        /*
         * Draw SpriteBatch for scenery.
         */
        public void DrawScenerySpriteBatch( Scenery scenery, SpriteFont font)
        {
            // Scenery text
            spriteBatch.DrawString(
                font,
                "",
                new Vector2(scenery.GetPosition().X - 80, scenery.GetPosition().Y + 100),
                Color.BlueViolet);
        }

        /*
         * Pair the atlas position of each tile with its world position.
         */
        public Dictionary<string, Vector2> CreateMap(Map map, int[,] tileLocations, bool impassable = false)
        {
            Dictionary<string, Vector2> sortByYAxis = [];
            Vector2 position = new(0, 0);
            int tileID = 1000;
            int row = 0;
            int column = 1;

            for (int x = 0; x < tileLocations.GetLength(row); x++) // For each row in the 2D array
            {
                for (int y = 0; y < tileLocations.GetLength(column); y++) // For each column
                {
                    int tile = tileLocations[x, y]; // Get the value of the current tile

                    if (tile > -1) // If it's a renderable tile
                    {
                        string tileName = string.Concat(tileID + tile.ToString()); // Give it a unique ID
                        sortByYAxis.Add(tileName, position); // And add it to the collection
                        tileID++; // Iterate the ID by 1

                        if (impassable) // If it's an impassable tile
                        {
                            Rectangle tileArea = new Rectangle((int)position.X, (int)position.Y, map.Width, map.Height); // Get the area of the map it occupies
                            ImpassableTiles.Add(tileArea); // And add it to the collection of impassable tile spaces
                        }
                    }

                    position.X += map.Width; // Step right by one tile space
                }
                position.X = 0;
                position.Y += map.Height; // Reset the x-axis and step down by one tile space
            }

            return sortByYAxis;
        }

        /*
         * Group impassable tile spaces into a collection of rows containing the impassable map areas.
         */
        public void ImpassableMapArea()
        {
            Rectangle block = ImpassableTiles[0]; // Take the first impassable tile space in the collection as the first block to compare
            List<Rectangle> impassableArea = []; // Ready a temporary empty collection to store the grouped impassable map areas

            foreach (Rectangle area in ImpassableTiles) // For each impassable tile space in the collection created on map generation
            {
                if (block.Bottom != area.Bottom) // If the block being compared against is not in the same row as, or is taller than, this tile space
                {
                    AddBlock(block, impassableArea); // Add it to the new collection
                    block = area; // Take the current impassable tile space as the next block to compare
                }
                if (block.Bottom == area.Bottom && block.Y == area.Y) // If this block is in the same row as this space and is the same height
                {
                    if (block.Right == area.Left) // If its x-axis end point is the same as this area's start point
                    {
                        block.Width += area.Width; // Combine their widths so the block encompasses both spaces
                    }
                    else if (block != area) // Otherwise, provided they do not represent the same tile space
                    {
                        AddBlock(block, impassableArea); // Add this block to the collection of impassable map spaces
                        block = area; // Take the current impassable tile space as the next block to compare
                    }
                }
            }
            AddBlock(block, impassableArea); // Add the final block to the collection of impassable map areas once there are no more tile spaces to compare against

            ImpassableTiles = impassableArea; // Update the original collection with the new collection
        }

        /*
         * Add the current block to the list of walkable areas.
         */
        public void AddBlock(Rectangle block, List<Rectangle> impassableArea)
        {
            block.Y -= block.Height;
            impassableArea.Add(block);
        }

        /*
         * Draw the map to the screen.
         */
        public void DrawMap(Texture2DAtlas atlas, Map map, string tileName, Vector2 position)
        {
            string tile = tileName.Remove(0, 4); // Remove the unique ID to get the atlas position
            int tileNumber = Convert.ToInt32(tile);

            // Offset drawing position by tile height to draw in front of any components using a different positioning reference
            position.Y -= (float)(map.Height * 1.25);

            spriteBatch.Draw(atlas[tileNumber], position, Color.White); // Draw the tile to the screen
        }

        /*
         * Finish drawing Skeleton.
         */
        public void FinishDrawingSkeleton()
        {
            skeletonRenderer.End();
        }

        /*
         * Finish drawing SpriteBatch.
         */
        public void FinishDrawingSpriteBatch()
        {
            spriteBatch.End();
        }
    }
}