using Logic.Game.GameMap;
using Logic.Game.GameMap.TiledScenery;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Graphics;
using SoR.Logic.Entities;
using Spine;

namespace Logic.Game.Graphics
{
    /*
     * Draw graphics to the screen.
     */
    internal class Render
    {
        private SpriteBatch spriteBatch;
        private SkeletonRenderer skeletonRenderer;
        private Vector2 position;

        public Render(GraphicsDevice GraphicsDevice)
        {
            // Initialise SpriteBatch
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Initialise skeleton renderer with premultiplied alpha
            skeletonRenderer = new SkeletonRenderer(GraphicsDevice);
            skeletonRenderer.PremultipliedAlpha = true;

            // The position the tile will be drawn at.
            position = Vector2.Zero;
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
            spriteBatch.Begin(transformMatrix: camera.GetViewMatrix(), samplerState: SamplerState.PointClamp);
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
         * Draw the map.
         */
        public void RenderMap(int rowLength, int columnLength, Texture2DRegion tileSet)
        {
            int rowNumber = 0;
            int columnNumber = 0;
            int previousColumn = -1;
            int previousRow = -1;

            for (int i = rowNumber; i < rowLength; i++)
            {
                for (int j = columnNumber; j < columnLength; j++)
                {
                    if (previousRow < 0 & previousColumn < 0)
                    {
                        position = new Vector2(0, 0);

                        spriteBatch.Draw(tileSet, position, Color.White);

                        position.Y += 64;
                    }

                    if (previousRow == i & previousColumn >= 0)
                    {

                    }
                    else if (previousRow - 1 < i & previousColumn == columnLength - 1)
                    {

                    }
                    previousColumn = j;
                    previousRow = i;

                    spriteBatch.Draw(tileSet, location, GetNextTile(xStart, yStart, tileWidth, tileHeight), Color.White);
                }
            }
        }

        /*
         * Draw SpriteBatch for the map.
         */
        public void DrawMapSpriteBatch(
            SpriteBatch spriteBatch,
            Texture2D tileSet,
            Vector2 location,
            Map drawTiles,
            int xStart,
            int yStart,
            int tileWidth,
            int tileHeight)
        {
            // Map
            spriteBatch.Draw(
                tileSet,
                location,
                drawTiles.GetNextTile(xStart, yStart, tileWidth, tileHeight),
                Color.White);
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