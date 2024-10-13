using Microsoft.Xna.Framework.Graphics;

namespace SoR.Logic.UI
{
    /*
     * Set up the start menu.
     */
    internal class StartMenu : Menu
    {
        public StartMenu(MainGame game, GraphicsDevice GraphicsDevice)
        {
            InitialiseInput(game);
            InitialiseMenu(game);

            MenuOptions = ["Inventory", "Settings", "Load game", "Exit game"];
        }
    }
}