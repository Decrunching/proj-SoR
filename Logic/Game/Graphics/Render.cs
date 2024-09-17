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
         * 
         * Try:
         * Use once to create dictionary with position mapped to Atlas region for each tile to be placed. Combine
         * with entity & scenery dictionaries to render everything in order of y-axis.
         */
        public void DrawMap(Texture2DAtlas atlas, Map map, int[,] tileLocations, int row = 0, int column = 1)
        {
            Vector2 position = new Vector2(0, 0);

            for (int x = 0; x < tileLocations.GetLength(row); x++) // For each row in the 2D array
            {
                for (int y = 0; y < tileLocations.GetLength(column); y++) // For each column
                {
                    int tile = tileLocations[x, y]; // Get the value of the current tile

                    if (tile > -1)
                    {
                        spriteBatch.Draw(atlas[tile], position, Color.White); // Draw the tile if not blank
                    }

                    position.X += map.GetTileDimensions(0, 0); // Step right by one tile space
                }
                position.X = 0;
                position.Y += map.GetTileDimensions(0, 1); // Reset the x-axis and step down by one tile space
            }
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