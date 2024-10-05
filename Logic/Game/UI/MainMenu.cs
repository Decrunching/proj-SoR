using Logic.Game.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Logic.Game.UI
{
    /*
     * The main game menu.
     */
    public class MainMenu
    {
        private GamePadInput gamePadInput;
        private KeyboardInput keyboardInput;
        private int select;
        public List<string> MenuOptions { get; set; }
        public Vector2 TitlePosition { get; set; }
        public Vector2 NewGamePosition { get; set; }
        public Vector2 ContinueGamePosition { get; set; }
        public Vector2 LoadGamePosition { get; set; }
        public Vector2 GameSettingsPosition { get; set; }

        public MainMenu(GraphicsDeviceManager graphics)
        {
            gamePadInput = new GamePadInput();
            keyboardInput = new KeyboardInput();

            select = 0;

            MenuOptions = ["Game Title", "Start new game", "Continue", "Load game", "Settings"];
            TitlePosition = new Vector2((graphics.PreferredBackBufferWidth / 2) - 125, 100);
            NewGamePosition = new Vector2(100, 300);
            ContinueGamePosition = new Vector2(100, 320);
            LoadGamePosition = new Vector2(100, 340);
            GameSettingsPosition = new Vector2(100, 360);
        }

        /*
         * Navigate the menu.
         */
        public int NavigateMenu()
        {
            switch (gamePadInput.CheckButtonInput())
            {
                case "Up":
                    if (select > 0)
                    {
                        select--;
                    }
                    break;
                case "Down":
                    if (select < 3)
                    {
                        select++;
                    }
                    break;
            }

            switch (keyboardInput.CheckOtherInput())
            {
                case "Up":
                    if (select > 0)
                    {
                        select--;
                    }
                    break;
                case "Down":
                    if (select < 3)
                    {
                        select++;
                    }
                    break;
            }

            return select;
        }
    }
}