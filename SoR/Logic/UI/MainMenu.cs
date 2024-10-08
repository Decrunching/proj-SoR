using Hardware.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoR;
using System.Collections.Generic;

namespace Logic.UI
{
    /*
     * The main game menu.
     */
    public class MainMenu
    {
        private GamePadInput gamePadInput;
        private KeyboardInput keyboardInput;
        private int select;
        public Texture2D Curtain { get; set; }
        public List<string> MenuOptions { get; set; }
        public Vector2 TitlePosition { get; set; }
        public Vector2 NewGamePosition { get; set; }
        public Vector2 ContinueGamePosition { get; set; }
        public Vector2 LoadGamePosition { get; set; }
        public Vector2 GameSettingsPosition { get; set; }

        public MainMenu(MainGame game, GraphicsDeviceManager graphics, GameWindow Window)
        {
            gamePadInput = new GamePadInput();
            keyboardInput = new KeyboardInput();
            keyboardInput.KeyboardInitialise(game, Window);

            select = 0;
            Curtain = game.Content.Load<Texture2D>(Globals.GetPath("Content\\SoR Resources\\Screens\\Screen Transitions\\curtain"));

            MenuOptions = ["Game Title", "Start new game", "Continue", "Load game", "Settings"];
            TitlePosition = new Vector2(graphics.PreferredBackBufferWidth / 2 - 125, 100);
            NewGamePosition = new Vector2(100, 300);
            ContinueGamePosition = new Vector2(100, 320);
            LoadGamePosition = new Vector2(100, 340);
            GameSettingsPosition = new Vector2(100, 360);
        }

        /*
         * Navigate the menu.
         */
        public int NavigateMenu(GameTime gameTime)
        {
            keyboardInput.KeyboardUpdate(gameTime);

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

            switch (keyboardInput.CheckKeyInput())
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