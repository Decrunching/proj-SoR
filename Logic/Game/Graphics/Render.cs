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
         * Currently destroys framerate - Too many spritebatch calls? Repeated multidimensional array iterations
         * too memory intensive?
         * 
         * Not currently aligning tiles correctly.
         */
        public void DrawMap(Texture2DAtlas atlas, int[,] map, int row = 0, int column = 1)
        {
            Vector2 position = new Vector2(0, 0);
            int rowNumber = 0;
            int columnNumber = 0;
            int previousColumn = -1;
            int previousRow = -1;

            for (int i = rowNumber; i < map.GetLength(row); i++)
            {
                System.Diagnostics.Debug.Write(i + ": ");
                for (int j = columnNumber; j < map.GetLength(column); j++)
                {
                    int tile = map[i, j];
                    if (previousRow < 0 & previousColumn < 0)
                    {
                        System.Diagnostics.Debug.Write("First: ");
                        if (tile > -1)
                        {
                            spriteBatch.Draw(atlas[tile], position, Color.White);
                        }
                        position.X += 64;
                    }
                    System.Diagnostics.Debug.Write(map[i, j] + ", ");

                    if (tile > -1 & previousRow == i & previousColumn >= 0)
                    {
                        spriteBatch.Draw(atlas[tile], position, Color.White);
                        position.X += 64;
                        System.Diagnostics.Debug.Write("(" + map[i, j - 1] + "), ");
                    }
                    else if (tile > -1 & previousRow - 1 < i & previousColumn == map.GetLength(column) - 1)
                    {
                        position.Y += 64;
                        position.X = 0;
                        spriteBatch.Draw(atlas[tile], position, Color.White);
                        System.Diagnostics.Debug.Write("(" + map[previousRow, previousColumn] + "), ");
                    }
                    previousColumn = j;
                    previousRow = i;
                }
                System.Diagnostics.Debug.Write("\n");
            }
            position = new Vector2(0, 0);
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