using Logic.Game.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;
using Spine;
using System.Collections.Generic;

namespace Logic.Game.UI
{
    /*
     * The main game menu.
     */
    public class MainMenu
    {
        private GamePadInput gamePadInput;
        private KeyboardState keyState;
        private KeyboardState lastKeyState;
        private GamePadState gamePadState;
        private GamePadState lastGamePadState;
        private GamePadCapabilities gamePadCapabilities;
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
            gamePadCapabilities = GamePad.GetCapabilities(PlayerIndex.One);

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
            keyState = Keyboard.GetState(); // Get the current keyboard state

            if (gamePadCapabilities.IsConnected) // If the gamepad is connected
            {
                gamePadState = GamePad.GetState(PlayerIndex.One); // Get the current gamepad state

                if (gamePadState.DPad.Down == ButtonState.Pressed &&
                    lastGamePadState.DPad.Down != ButtonState.Pressed &&
                    select < 3)
                {
                    select++;
                }

                if (gamePadState.DPad.Up == ButtonState.Pressed &&
                    lastGamePadState.DPad.Up != ButtonState.Pressed &&
                    select > 0)
                {
                    select--;
                }

                if (gamePadState.ThumbSticks.Left.Y < -0.5f && select < 3)
                {
                    select++;
                }
                else if (gamePadState.ThumbSticks.Left.Y > 0.5f && select > 0)
                {
                    select--;
                }

                lastGamePadState = gamePadState;
            }

            if (keyState.IsKeyDown(Keys.Down) && !lastKeyState.IsKeyDown(Keys.Down) && select < 3)
            {
                select++;
            }

            if (keyState.IsKeyDown(Keys.Up) && !lastKeyState.IsKeyDown(Keys.Up) && select > 0)
            {
                select--;
            }

            lastKeyState = keyState;

            return select;
        }
    }
}