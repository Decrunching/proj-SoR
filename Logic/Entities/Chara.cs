using Microsoft.Xna.Framework;
using Spine;

namespace SoR.Logic.Entities
{
    public abstract class Chara : Game1
    {
        private SkeletonRenderer skeletonRenderer;
        protected Vector2 position;

        public Chara(Game1 game)
        {
            position = new Vector2(game.GetGraphics().PreferredBackBufferWidth / 2,
                game.GetGraphics().PreferredBackBufferHeight / 2);

            // Initialise skeleton renderer with premultiplied alpha
            skeletonRenderer = new SkeletonRenderer(game.GetGraphicsDevice());
            skeletonRenderer.PremultipliedAlpha = true;
        }

        public SkeletonRenderer GetSkeletonRenderer()
        {
            return skeletonRenderer;
        }

        public abstract void Render(GameTime gameTime, Game1 game);
    }
}