using Logic.Game.GameMap;
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
        public void RenderMap(Texture2DAtlas atlas, int row = 1, int column = 2)
        {
            Vector2 position = new Vector2(0, 0);
            int rowNumber = 0;
            int columnNumber = 0;
            int previousColumn = -1;
            int previousRow = -1;

            for (int i = 0; i < 1; i++) // Which map tileset to iterate through
            {
                for (int j = rowNumber; j < GetTempleLayout().GetLength(row); j++) // Which row
                {
                    for (int k = columnNumber; k < GetTempleLayout().GetLength(column); k++) // Which column
                    {
                        int tile = GetTempleLayout()[i, j, k];

                        if (previousRow < 0 & previousColumn < 0) // On the first tile placed from each tileset
                        {
                            if (tile > 0)
                            {
                                spriteBatch.Draw(atlas[tile], position, Color.White);
                            }
                            position.Y += 64;
                        }

                        if (previousRow == j & previousColumn >= 0)
                        {
                            spriteBatch.Draw(atlas[tile], position, Color.White);
                            position.Y += 64;
                        }
                        else if (previousRow - 1 < j & previousColumn == GetTempleLayout().GetLength(column) - 1)
                        {
                            position.X += 64;
                            position.Y = 0;
                            spriteBatch.Draw(atlas[tile], position, Color.White);
                        }
                        previousColumn = k;
                        previousRow = j;
                    }
                }
                position = new Vector2(0, 0);
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