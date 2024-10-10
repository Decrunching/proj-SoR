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
    public abstract class Menu
    {
        protected GamePadInput gamePadInput;
        protected GamePadListener gamePadListener;
        protected KeyboardInput keyboardInput;
        protected KeyboardListener keyboardListener;
        protected int select;
        public Texture2D Curtain { get; set; }
        public List<string> MenuOptions { get; set; }

        /*
         * Initialise the generic menu fields.
         */
        public void InitialiseMenu(MainGame game)
        {
            select = 0;
            Curtain = game.Content.Load<Texture2D>(Globals.GetPath("Content\\SoR Resources\\Screens\\Screen Transitions\\curtain"));
        }

        /*
         * Initialise the input devices and listeners.
         */
        public void InitialiseInput(MainGame game)
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
            keyboardListener.KeyPressed += (sender, args) => { game.Window.Title = $"Key {args.Key} Pressed"; };
            keyboardListener.KeyPressed += OnKeyPressed;
            gamePadListener.ButtonRepeated += OnButtonPressed;
        }

        /*
         * Update keyboard input.
         */
        public void UpdateInput(GameTime gameTime)
        {
            keyboardListener.Update(gameTime);
            gamePadListener.Update(gameTime);
        }

        /*
         * On Keyboard key press, move up and down menu items with a slight delay whilst the key is being held down.
         */
        public void OnKeyPressed(object sender, KeyboardEventArgs e)
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
         * On GamePad button press, move up and down menu items with a slight delay whilst the button is being held down.
         */
        public void OnButtonPressed(object sender, GamePadEventArgs e)
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
            UpdateInput(gameTime);

            return select;
        }
    }
}