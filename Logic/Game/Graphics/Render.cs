using Logic.Game.GameMap;
using Logic.Game.GameMap.TiledScenery;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;
using SoR.Logic.Entities;
using Spine;
using System;
using System.Collections.Generic;

namespace Logic.Game.Graphics
{
    /*
     * Draw graphics to the screen.
     */
    internal partial class Render
    {
        private SpriteBatch spriteBatch;
        private SkeletonRenderer skeletonRenderer;

        public Render(GraphicsDevice GraphicsDevice)
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            skeletonRenderer = new SkeletonRenderer(GraphicsDevice);
            skeletonRenderer.PremultipliedAlpha = true;
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
         * Draws the map to the screen.
         */
        public Dictionary<string, Vector2> CreateMap(Texture2DAtlas atlas, Map map, int[,] tileLocations, int row = 0, int column = 1)
        {
            Dictionary<string, Vector2> sortByYAxis = new Dictionary<string, Vector2>();
            Vector2 position = new Vector2(0, 0);
            int tileID = 1000;

            for (int x = 0; x < tileLocations.GetLength(row); x++) // For each row in the 2D array
            {
                for (int y = 0; y < tileLocations.GetLength(column); y++) // For each column
                {
                    int tile = tileLocations[x, y]; // Get the value of the current tile

                    if (tile > -1)
                    {
                        string tileName = string.Concat(tileID + tile.ToString());
                        sortByYAxis.Add(tileName, position);
                        tileID++;
                    }

                    position.X += map.GetTileDimensions(0, 0); // Step right by one tile space
                }
                position.X = 0;
                position.Y += map.GetTileDimensions(0, 1); // Reset the x-axis and step down by one tile space
            }

            return sortByYAxis;
        }

        /*
         * Draw the map walls to the screen.
         */
        public void DrawMap(Texture2DAtlas atlas, Map map, string tileName, Vector2 position, SpriteFont font)
        {
            string tile = tileName.Remove(0, 4);

            int tileNumber = Convert.ToInt32(tile);

            // Offset drawing position by tile height to draw in front of the components that use a different positioning reference
            position.Y -= (float)(map.GetTileDimensions(0, 1) * 1.25);

            spriteBatch.Draw(atlas[tileNumber], position, Color.White);

            // Entity text
            spriteBatch.DrawString(
            font,
                "Rect X:" + map.BoundingArea.X + ", Rect Y: " + map.BoundingArea.Y +
            "\nRect width: " + map.BoundingArea.Width + ", Rect height: " + map.BoundingArea.Height,
            new Vector2(map.BoundingArea.X, map.BoundingArea.Y),
                Color.BlueViolet);
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