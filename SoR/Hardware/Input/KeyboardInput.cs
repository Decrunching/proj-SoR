using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using MonoGame.Extended.Input.InputListeners;
using SoR;

namespace Hardware.Input
{
    /*
     * Handle keyboard input.
     */
    public class KeyboardInput
    {
        private KeyboardStateExtended keyState;
        private KeyboardStateExtended lastKeyState;
        private readonly KeyboardListener keyboardListener;
        private bool up;
        private bool down;
        private bool left;
        private bool right;
        public bool CurrentInputDevice { get; set; }

        public KeyboardInput()
        {
            keyboardListener = new KeyboardListener();

            CurrentInputDevice = false;
            up = false;
            down = false;
            left = false;
            right = false;
        }

        /*
         * 
         */
        public void KeyboardInitialise(MainGame game, GameWindow Window)
        {
            keyboardListener.KeyPressed += (sender, args) => { Window.Title = $"Key {args.Key} Pressed"; };

            game.Components.Add(new InputListenerComponent(game, keyboardListener));
        }

        /*
         * Update keyboard input.
         */
        public void KeyboardUpdate(GameTime gameTime)
        {
            keyboardListener.Update(gameTime);
        }

        /*
         * Check for and process keyboard x-axis movement inputs.
         */
        public int CheckXMoveInput()
        {
            keyState = KeyboardExtended.GetState(); // Get the current keyboard state

            int xAxisInput = 0;

            if (keyState.IsKeyDown(Keys.Left) ||
                keyState.IsKeyDown(Keys.A))
            {
                CurrentInputDevice = true;
                left = true;

                if (!lastKeyState.IsKeyDown(Keys.Left) ||
                    !lastKeyState.IsKeyDown(Keys.A))
                {
                    xAxisInput = 1;
                }
            }
            else if (keyState.IsKeyDown(Keys.Right) ||
                keyState.IsKeyDown(Keys.D))
            {
                CurrentInputDevice = true;
                right = true;

                if (!lastKeyState.IsKeyDown(Keys.Right) ||
                !lastKeyState.IsKeyDown(Keys.D))
                {
                    xAxisInput = 2;
                }
            }

            if ((keyState.IsKeyDown(Keys.Left) ||
            keyState.IsKeyDown(Keys.A)) &&
            (keyState.IsKeyDown(Keys.Right) ||
            keyState.IsKeyDown(Keys.D)))
            {
                xAxisInput = 4;
                left = true;
                right = true;
            }

            bool unpressedLeft =
                keyState.WasKeyReleased(Keys.Left) ||
                keyState.WasKeyReleased(Keys.A);

            bool unpressedRight =
                keyState.WasKeyReleased(Keys.Right) ||
                keyState.WasKeyReleased(Keys.D);

            if (unpressedLeft || unpressedRight)
            {
                xAxisInput = 3;
            }
            if (unpressedLeft)
            {
                left = false;
            }
            if (unpressedRight)
            {
                right = false;
            }

            lastKeyState = keyState; // Get the previous keyboard state

            return xAxisInput;
        }

        /*
         * Check for and process keyboard y-axis movement inputs.
         */
        public int CheckYMoveInput()
        {
            keyState = KeyboardExtended.GetState(); // Get the current keyboard state

            int yAxisInput = 0;

            if (keyState.IsKeyDown(Keys.Up) ||
                keyState.IsKeyDown(Keys.W))
            {
                CurrentInputDevice = true;
                up = true;

                if (!lastKeyState.IsKeyDown(Keys.Up) ||
                !lastKeyState.IsKeyDown(Keys.W))
                {
                    yAxisInput = 1;
                }
            }
            else if (keyState.IsKeyDown(Keys.Down) ||
                keyState.IsKeyDown(Keys.S))
            {
                CurrentInputDevice = true;
                down = true;

                if (!lastKeyState.IsKeyDown(Keys.Down) ||
                !lastKeyState.IsKeyDown(Keys.S))
                {
                    yAxisInput = 2;
                }
            }

            if ((keyState.IsKeyDown(Keys.Up) ||
            keyState.IsKeyDown(Keys.W)) &&
            (keyState.IsKeyDown(Keys.Down) ||
            keyState.IsKeyDown(Keys.S)))
            {
                up = true;
                down = true;

                if (left || right)
                {
                    yAxisInput = 5;
                }
                else
                {
                    yAxisInput = 4;
                }
            }

            bool unpressedUp =
                keyState.WasKeyReleased(Keys.Up) ||
                keyState.WasKeyReleased(Keys.W);

            bool unpressedDown =
                keyState.WasKeyReleased(Keys.Down) ||
                keyState.WasKeyReleased(Keys.S);

            if (unpressedUp || unpressedDown)
            {
                yAxisInput = 3;
            }
            if (unpressedUp)
            {
                up = false;
            }
            if (unpressedDown)
            {
                down = false;
            }

            lastKeyState = keyState; // Get the previous keyboard state

            return yAxisInput;
        }

        /*
         * Check keyboard input.
         * F4 = toggle fullscreen. F8 = save. F9 = load. Esc = exit.
         */
        public string CheckKeyInput()
        {
            string key = "none";

            keyState = KeyboardExtended.GetState(); // Get the current keyboard state

            if (keyState.IsKeyDown(Keys.F4) && !lastKeyState.IsKeyDown(Keys.F4))
            {
                key = "F4";
            }
            if (keyState.IsKeyDown(Keys.F8) && !lastKeyState.IsKeyDown(Keys.F8))
            {
                key = "F8";
            }
            if (keyState.IsKeyDown(Keys.F9) && !lastKeyState.IsKeyDown(Keys.F9))
            {
                key = "F9";
            }
            if (keyState.IsKeyDown(Keys.Down) && !lastKeyState.IsKeyDown(Keys.Down) ||
                keyState.IsKeyDown(Keys.S) && !lastKeyState.IsKeyDown(Keys.S))
            {
                key = "Down";
            }
            if (keyState.IsKeyDown(Keys.Up) && !lastKeyState.IsKeyDown(Keys.Up) ||
                keyState.IsKeyDown(Keys.W) && !lastKeyState.IsKeyDown(Keys.W))
            {
                key = "Up";
            }
            if (keyState.IsKeyDown(Keys.Enter) && !lastKeyState.IsKeyDown(Keys.Enter))
            {
                key = "Enter";
            }

            lastKeyState = keyState; // Get the previous keyboard state

            return key;
        }
    }
}