using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Hardware.Input
{
    /*
     * Handle keyboard input.
     */
    public class KeyboardInput
    {
        private KeyboardState keyState;
        private KeyboardState lastKeyState;
        public Dictionary<Keys, InputKeys> InputCollection { get; set; }
        public bool CurrentInputDevice { get; set; }

        public KeyboardInput()
        {
            CurrentInputDevice = false;
        }

        /*
         * Check for and process keyboard movement inputs.
         */
        public int CheckXMoveInput()
        {
            keyState = Keyboard.GetState(); // Get the current keyboard state

            int xAxisInput = 0;

            if (keyState.IsKeyDown(Keys.Left) ||
                keyState.IsKeyDown(Keys.A))
            {
                CurrentInputDevice = true;

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
            }

            bool unpressedLeft =
                (!keyState.IsKeyDown(Keys.Left) &&
                lastKeyState.IsKeyDown(Keys.Left)) ||
                (!keyState.IsKeyDown(Keys.A) &&
                lastKeyState.IsKeyDown(Keys.A));

            bool unpressedRight =
                (!keyState.IsKeyDown(Keys.Right) &&
                lastKeyState.IsKeyDown(Keys.Right)) ||
                (!keyState.IsKeyDown(Keys.D) &&
                lastKeyState.IsKeyDown(Keys.D));

            if (unpressedLeft || unpressedRight)
            {
                xAxisInput = 3;
            }

            lastKeyState = keyState; // Get the previous keyboard state

            return xAxisInput;
        }

        /*
         * Check for and process keyboard movement inputs.
         */
        public int CheckYMoveInput()
        {
            keyState = Keyboard.GetState(); // Get the current keyboard state

            int yAxisInput = 0;

            if (keyState.IsKeyDown(Keys.Up) ||
                keyState.IsKeyDown(Keys.W))
            {
                CurrentInputDevice = true;

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
                yAxisInput = 4;
            }

            bool unpressedUp =
                (!keyState.IsKeyDown(Keys.Up) &&
                lastKeyState.IsKeyDown(Keys.Up)) ||
                (!keyState.IsKeyDown(Keys.W) &&
                lastKeyState.IsKeyDown(Keys.W));

            bool unpressedDown =
                (!keyState.IsKeyDown(Keys.Down) &&
                lastKeyState.IsKeyDown(Keys.Down)) ||
                (!keyState.IsKeyDown(Keys.S) &&
                lastKeyState.IsKeyDown(Keys.S));

            if (unpressedUp || unpressedDown)
            {
                yAxisInput = 3;
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

            keyState = Keyboard.GetState(); // Get the current keyboard state

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
            if (keyState.IsKeyDown(Keys.Down) && !lastKeyState.IsKeyDown(Keys.Down))
            {
                key = "Down";
            }
            if (keyState.IsKeyDown(Keys.Up) && !lastKeyState.IsKeyDown(Keys.Up))
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