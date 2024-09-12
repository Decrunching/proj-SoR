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
        protected Atlas atlas;
        protected AtlasAttachmentLoader atlasAttachmentLoader;
        protected SkeletonJson json;
        protected SkeletonData skeletonData;
        protected SkeletonRenderer skeletonRenderer;
        protected Skeleton skeleton;
        protected AnimationStateData animStateData;
        protected AnimationState animState;
        protected Vector2 position;
        protected string[,] maps;
        private int currentTile;
        private int totalTiles;
        public int Rows { get; set; }
        public int Columns { get; set; }
        public bool Render { get; set; }

        public Map(GraphicsDevice GraphicsDevice, int row, string skin)
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

            // Set the skin
            skeleton.SetSkin(skeletonData.FindSkin(skin));

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
            maps = new string[,]
                {
                { "Content\\SoR Resources\\Locations\\TiledScenery\\Temple\\Spine\\Temple.atlas",
                        "Content\\SoR Resources\\Locations\\TiledScenery\\Temple\\Spine\\skeleton.json" } // 0: Temple
            };

            return maps[row, column];
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
            skeleton.X = position.X;
            skeleton.Y = position.Y;

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
                new Vector2(position.X - 80, position.Y + 100),
                Color.BlueViolet);
            spriteBatch.End();
        }

        /*
         * Set entity position to the centre of the screen +/- any x,y axis adjustment.
         */
        public virtual void SetPosition(Vector2 tilePosition)
        {
            position = tilePosition;
        }

        /*
         * Get the skeleton.
         */
        public Skeleton GetSkeleton()
        {
            return skeleton;
        }

        public Vector2 GetPosition()
        {
            return position;
        }
    }
}