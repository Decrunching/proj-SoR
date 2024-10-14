using Microsoft.Xna.Framework;

namespace SoR.Logic.UI
{
    internal class MainMenu : Menu
    {
        /*
         * Set up the main menu.
         */
        public Vector2 TitlePosition { get; set; }
        public Vector2 NewGamePosition { get; set; }
        public Vector2 ContinueGamePosition { get; set; }
        public Vector2 LoadGamePosition { get; set; }
        public Vector2 GameSettingsPosition { get; set; }

        public MainMenu(MainGame game, GraphicsDeviceManager graphics)
        {
            InitialiseInput(game);
            InitialiseMenu(game);

            MenuOptions = ["Game Title", "Start new game", "Continue", "Load game", "Settings"];
            TitlePosition = new Vector2(graphics.PreferredBackBufferWidth / 2 - 125, 100);
            NewGamePosition = new Vector2(100, 300);
            ContinueGamePosition = new Vector2(100, 320);
            LoadGamePosition = new Vector2(100, 340);
            GameSettingsPosition = new Vector2(100, 360);
        }
    }
}