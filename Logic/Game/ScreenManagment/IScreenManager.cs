using Microsoft.Xna.Framework.Graphics;

namespace Logic.Game.ScreenManagment
{
    /*
     * Placeholder class for game stages.
     */
    public interface IScreenManager
    {
        /*
         * 
         */
        void Update(float delta);

        void Draw(SpriteBatch spriteBatch);
    }
}