using Microsoft.Xna.Framework;

namespace SoR.Logic.UI
{
    internal class MainMenu : Menu
    {
        /*
         * Set up the main menu.
         */

        public MainMenu(MainGame game, GraphicsDeviceManager graphics)
        {
            InitialiseInput(game);
            InitialiseMenu(game);

            MenuOptions = ["Game Title", "Start new game", "Continue", "Load game", "Settings", "Exit to desktop"];
        }
    }
}