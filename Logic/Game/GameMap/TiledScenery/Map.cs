using Microsoft.Xna.Framework.Graphics;
using Spine;
using SoR;
using MonoGame.Extended;
using Microsoft.Xna.Framework;

namespace Logic.Game.GameMap.TiledScenery
{
    /*
     * Create a map.
     */
    internal class Map
    {
        private Atlas atlas;
        private AtlasAttachmentLoader atlasAttachmentLoader;
        private SkeletonJson json;
        private SkeletonData skeletonData;
        private SkeletonRenderer skeletonRenderer;
        private Skeleton skeleton;
        private AnimationStateData animStateData;
        private AnimationState animState;
        private int width;
        private int height;
        public Vector2 Position { get; set; }
        public string Name { get; set; }
        public bool Render { get; set; }
        public int[] MapDimensions { get; private set; }
        public string SkinName { get; set; }

        public Map(GraphicsDevice GraphicsDevice, int row)
        {
            // Load texture atlas and attachment loader
            atlas = new Atlas(Globals.GetPath(UseTileset(row, 0)), new XnaTextureLoader(GraphicsDevice));
            //atlas = new Atlas("D:\\GitHub projects\\Proj-SoR\\Content\\Scenery\\Grass\\skeleton.atlas", new XnaTextureLoader(GraphicsDevice));
            atlasAttachmentLoader = new AtlasAttachmentLoader(atlas);
            json = new SkeletonJson(atlasAttachmentLoader);

            // Initialise skeleton json
            skeletonData = json.ReadSkeletonData(Globals.GetPath(UseTileset(row, 1)));
            //skeletonData = json.ReadSkeletonData("D:\\GitHub projects\\Proj-SoR\\Content\\Scenery\\Grass\\skeleton.json");
            skeleton = new Skeleton(skeletonData);

            // Setup animation
            animStateData = new AnimationStateData(skeleton.Data);
            animState = new AnimationState(animStateData);
            animState.Apply(skeleton);

            // Initialise skeleton renderer with premultiplied alpha
            skeletonRenderer = new SkeletonRenderer(GraphicsDevice);
            skeletonRenderer.PremultipliedAlpha = true;
        }

        /*
         * Get the required tileset.
         */
        public string UseTileset(int row, int column)
        {
            string[,] maps = new string[,]
                {
                    { "Content\\SoR Resources\\Locations\\TiledScenery\\Temple\\Spine\\Temple.atlas",
                        "Content\\SoR Resources\\Locations\\TiledScenery\\Temple\\Spine\\skeleton.json" } // 0: Temple
            };

            return maps[row, column];
        }

        /*
         * Set the tile dimensions for the current map. The first array element is the width, the second is the height.
         */
        public void SetTileDimensions(int mapNumber)
        {
            MapDimensions = [64, 64]; // 0: Temple
        }

        /*
         * Get the tile width.
         */
        public int GetWidth()
        {
            return MapDimensions[0];
        }

        /*
         * Get the tile width.
         */
        public int GetHeight()
        {
            return MapDimensions[1];
        }

        /*
         * Set the tile name.
         */
        public void SetName(string name)
        {
            Name = name;
        }

        /*
         * Set the skin for the current tile.
         */
        public void SetSkin(string skin)
        {
            skeleton.SetSkin(skeletonData.FindSkin(skin));
        }

        /*
         * Render the current skeleton to the screen.
         */
        public virtual void RenderFloorTile(GraphicsDevice GraphicsDevice, OrthographicCamera camera)
        {
            // Create the skeleton renderer projection matrix
            ((BasicEffect)skeletonRenderer.Effect).Projection = Matrix.CreateOrthographicOffCenter(
            0,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height,
                0, 1, 0);
            ((BasicEffect)skeletonRenderer.Effect).View = camera.GetViewMatrix();

            // Draw skeletons
            skeletonRenderer.Begin();

            // Update the animation state and apply animations to skeletons
            skeleton.X = Position.X;
            skeleton.Y = Position.Y;

            skeletonRenderer.Draw(skeleton);
            skeletonRenderer.End();
        }

        /*
         * Draw text to the screen.
         */
        public void DrawText(SpriteBatch spriteBatch, SpriteFont font, OrthographicCamera camera)
        {
            spriteBatch.Begin(transformMatrix: camera.GetViewMatrix());
            spriteBatch.DrawString(
                font,
                "",
                new Vector2(Position.X - 80, Position.Y + 100),
                Color.BlueViolet);
            spriteBatch.End();
        }

        /*
         * Get the skeleton.
         */
        public Skeleton GetSkeleton()
        {
            return skeleton;
        }
    }
}