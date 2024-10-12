using Microsoft.Xna.Framework;

namespace SoR.Logic.UI
{
    internal class StartMenu : Menu
    {
        public Vector2 InventoryPosition { get; set; }
        public Vector2 GameSettingsPosition { get; set; }
        public Vector2 LoadGamePosition { get; set; }
        public Vector2 ExitGamePosition { get; set; }
        public bool StartMenuScreen { get; set; }

        public StartMenu(MainGame game, GraphicsDeviceManager graphics)
        {
            InitialiseInput(game);
            InitialiseMenu(game);

            MenuOptions = ["Inventory", "Settings", "Load game", "Exit game"];
            InventoryPosition = new Vector2(graphics.PreferredBackBufferWidth / 2 - 125, 100);
            GameSettingsPosition = new Vector2(100, 300);
            LoadGamePosition = new Vector2(100, 340);
            ExitGamePosition = new Vector2(100, 360);

            StartMenuScreen = true;
        }
    }
}