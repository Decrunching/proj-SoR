using SoR.Hardware.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;
using System.Collections.Generic;

namespace SoR.Logic.UI
{
    /*
     * The main game menu.
     */
    public class MainMenu
    {
        private GamePadInput gamePadInput;
        private GamePadListener gamePadListener;
        private KeyboardInput keyboardInput;
        private KeyboardListener keyboardListener;
        private int select;
        public Texture2D Curtain { get; set; }
        public List<string> MenuOptions { get; set; }
        public Vector2 TitlePosition { get; set; }
        public Vector2 NewGamePosition { get; set; }
        public Vector2 ContinueGamePosition { get; set; }
        public Vector2 LoadGamePosition { get; set; }
        public Vector2 GameSettingsPosition { get; set; }
        public bool MainMenuScreen { get; set; }

        public MainMenu(MainGame game, GraphicsDeviceManager graphics)
        {
            gamePadInput = new GamePadInput();
            keyboardInput = new KeyboardInput();

            var keyboardListenerSettings = new KeyboardListenerSettings
            {
                RepeatPress = true,
                InitialDelayMilliseconds = 250,
                RepeatDelayMilliseconds = 100
            };

            var gamePadListenerSettings = new GamePadListenerSettings
            {
                RepeatInitialDelay = 250,
                RepeatDelay = 100
            };

            keyboardListener = new KeyboardListener(keyboardListenerSettings);
            gamePadListener = new GamePadListener(gamePadListenerSettings);

            // Debugging - shows the pressed key is being registered
            keyboardListener.KeyPressed += (sender, args) => {game.Window.Title = $"Key {args.Key} Pressed"; };
            keyboardListener.KeyPressed += OnKeyPressed;
            gamePadListener.ButtonRepeated += OnButtonPressed;

            select = 0;
            Curtain = game.Content.Load<Texture2D>(Globals.GetPath("Content\\SoR Resources\\Screens\\Screen Transitions\\curtain"));

            MenuOptions = ["Game Title", "Start new game", "Continue", "Load game", "Settings"];
            TitlePosition = new Vector2(graphics.PreferredBackBufferWidth / 2 - 125, 100);
            NewGamePosition = new Vector2(100, 300);
            ContinueGamePosition = new Vector2(100, 320);
            LoadGamePosition = new Vector2(100, 340);
            GameSettingsPosition = new Vector2(100, 360);

            MainMenuScreen = true;
        }

        /*
         * Update keyboard input.
         */
        public void InputUpdate(GameTime gameTime)
        {
            keyboardListener.Update(gameTime);
            gamePadListener.Update(gameTime);
        }

        /*
         * On key press, move up and down menu items with a slight delay whilst the key is being held down.
         */
        private void OnKeyPressed(object sender, KeyboardEventArgs e)
        {
            if (e.Key == Keys.Down ||
                e.Key == Keys.S)
            {
                if (select < 3)
                {
                    select++;
                }
            }
            else if (e.Key == Keys.Up ||
                e.Key == Keys.W)
            {
                if (select > 0)
                {
                    select--;
                }
            }
        }

        /*
         * 
         */
        private void OnButtonPressed(object sender, GamePadEventArgs e)
        {
            if (e.Button == Buttons.DPadDown)
            {
                if (select < 3)
                {
                    select++;
                }
            }
            else if (e.Button == Buttons.DPadUp)
            {
                if (select > 0)
                {
                    select--;
                }
            }
        }

        /*
         * Navigate the menu.
         */
        public int NavigateMenu(GameTime gameTime)
        {
            InputUpdate(gameTime);

            return select;
        }
    }
}