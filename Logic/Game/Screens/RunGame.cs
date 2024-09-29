using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Logic.Game.Screens
{
    public partial class Screens
    {
        /*
         * Update the state of the game.
         */
        public void UpdateGameState(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            gameLogic.UpdateWorld(gameTime, graphics);
        }

        /*
         * Update the resolution after a screen size change.
         */
        public void UpdateResolution(GameWindow Window, int screenWidth, int screenHeight)
        {
            gameLogic.UpdateViewportGraphics(Window, screenWidth, screenHeight);
        }

        /*
         * Draw the current state of the game to the screen.
         */
        public void DrawGameState(GraphicsDevice GraphicsDevice)
        {
            gameLogic.Render(GraphicsDevice);
        }
    }
}
