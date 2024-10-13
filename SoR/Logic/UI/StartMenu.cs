using Microsoft.Xna.Framework;

namespace SoR.Logic.UI
{
    internal class StartMenu : Menu
    {
        public Vector2 InventoryPosition { get; set; }
        public Vector2 GameSettingsPosition { get; set; }
        public Vector2 LoadGamePosition { get; set; }
        public Vector2 ExitGamePosition { get; set; }

        public StartMenu(MainGame game, GraphicsDeviceManager graphics)
        {
            InitialiseInput(game);
            InitialiseMenu(game);

            int screenWidth = graphics.PreferredBackBufferWidth;
            int screenHeight = graphics.PreferredBackBufferHeight;

            MenuOptions = ["Inventory", "Settings", "Load game", "Exit game"];
            InventoryPosition = new Vector2(screenWidth * 0.1f, screenHeight * 0.3f);
            GameSettingsPosition = new Vector2(screenWidth * 0.1f, screenHeight * 0.4f);
            LoadGamePosition = new Vector2(screenWidth * 0.1f, screenHeight * 0.5f);
            ExitGamePosition = new Vector2(screenWidth * 0.1f, screenHeight * 0.6f);
        }
    }
}