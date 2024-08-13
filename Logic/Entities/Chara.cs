using Microsoft.Xna.Framework;
using Spine;

namespace SoR.Logic.Entities
{
    public interface Chara
    {
        public SkeletonRenderer GetSkeletonRenderer();

        public abstract void Render(GameTime gameTime, Game1 game);
    }
}