using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;

namespace SoR.Hardware.Input
{
    /*
     * Handle keyboard input.
     */
    public class KeyboardInput
    {
        private KeyboardStateExtended keyState;
        private KeyboardStateExtended lastKeyState;
        private bool up;
        private bool down;
        private bool left;
        private bool right;
        public bool CurrentInputDevice { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string Key { get; set; }

        public KeyboardInput()
        {
            CurrentInputDevice = false;
            up = false;
            down = false;
            left = false;
            right = false;
        }

        /*
         * Get input.
         */
        public void GetInput()
        {
            if (keyState.WasAnyKeyJustDown())
            {
                CurrentInputDevice = true;
            }
            else
            {
                CurrentInputDevice = false;
            }

            Key = CheckKeyInput();
            X = CheckXMoveInput();
            Y = CheckYMoveInput();
        }

        /*
         * Check keyboard input.
         * F4 = toggle fullscreen. F8 = save. F9 = load. Esc = exit. Enter = select menu item.
         * Space = change skin. Escape = open start menu.
         */
            public string CheckKeyInput()
        {
            Key = "none";

            keyState = KeyboardExtended.GetState(); // Get the current keyboard state

            if (keyState.IsKeyDown(Keys.F4) && !lastKeyState.IsKeyDown(Keys.F4))
            {
                Key = "F4";
            }
            if (keyState.IsKeyDown(Keys.F8) && !lastKeyState.IsKeyDown(Keys.F8))
            {
                Key = "F8";
            }
            if (keyState.IsKeyDown(Keys.F9) && !lastKeyState.IsKeyDown(Keys.F9))
            {
                Key = "F9";
            }
            if (keyState.IsKeyDown(Keys.Enter) && !lastKeyState.IsKeyDown(Keys.Enter))
            {
                Key = "Enter";
            }
            if (keyState.IsKeyDown(Keys.Space) & !lastKeyState.IsKeyDown(Keys.Space))
            {
                Key = "Space";
            }
            if (keyState.IsKeyDown(Keys.Escape) & !lastKeyState.IsKeyDown(Keys.Escape))
            {
                Key = "Escape";
            }

            lastKeyState = keyState; // Get the previous keyboard state

            return Key;
        }

        /*
         * Check for and process keyboard x-axis movement inputs.
         */
        public int CheckXMoveInput()
        {
            keyState = KeyboardExtended.GetState(); // Get the current keyboard state

            X = 0;

            if (keyState.IsKeyDown(Keys.Left) ||
                keyState.IsKeyDown(Keys.A))
            {
                left = true;

                if (!lastKeyState.IsKeyDown(Keys.Left) ||
                    !lastKeyState.IsKeyDown(Keys.A))
                {
                    X = 1;
                }
            }
            else if (keyState.IsKeyDown(Keys.Right) ||
                keyState.IsKeyDown(Keys.D))
            {
                right = true;

                if (!lastKeyState.IsKeyDown(Keys.Right) ||
                !lastKeyState.IsKeyDown(Keys.D))
                {
                    X = 2;
                }
            }

            if ((keyState.IsKeyDown(Keys.Left) ||
            keyState.IsKeyDown(Keys.A)) &&
            (keyState.IsKeyDown(Keys.Right) ||
            keyState.IsKeyDown(Keys.D)))
            {
                X = 4;
                left = true;
                right = true;
            }

            bool unpressedLeft =
                keyState.WasKeyReleased(Keys.Left) ||
                keyState.WasKeyReleased(Keys.A);

            bool unpressedRight =
                keyState.WasKeyReleased(Keys.Right) ||
                keyState.WasKeyReleased(Keys.D);

            if (unpressedLeft)
            {
                left = false;
            }
            if (unpressedRight)
            {
                right = false;
            }
            if (unpressedLeft || unpressedRight)
            {
                X = 3;
            }

            lastKeyState = keyState; // Get the previous keyboard state

            return X;
        }

        /*
         * Check for and process keyboard y-axis movement inputs.
         */
        public int CheckYMoveInput()
        {
            keyState = KeyboardExtended.GetState(); // Get the current keyboard state

            Y = 0;

            if (keyState.IsKeyDown(Keys.Up) ||
                keyState.IsKeyDown(Keys.W))
            {
                up = true;

                if (!lastKeyState.IsKeyDown(Keys.Up) ||
                !lastKeyState.IsKeyDown(Keys.W))
                {
                    Y = 1;
                }
            }
            else if (keyState.IsKeyDown(Keys.Down) ||
                keyState.IsKeyDown(Keys.S))
            {
                down = true;

                if (!lastKeyState.IsKeyDown(Keys.Down) ||
                !lastKeyState.IsKeyDown(Keys.S))
                {
                    Y = 2;
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
                    Y = 5;
                }
                else
                {
                    Y = 4;
                }
            }

            bool unpressedUp =
                keyState.WasKeyReleased(Keys.Up) ||
                keyState.WasKeyReleased(Keys.W);

            bool unpressedDown =
                keyState.WasKeyReleased(Keys.Down) ||
                keyState.WasKeyReleased(Keys.S);

            if (unpressedUp)
            {
                up = false;
            }
            if (unpressedDown)
            {
                down = false;
            }
            if (unpressedUp || unpressedDown)
            {
                Y = 3;
            }
            if (!up && !down && !left && !right)
            {
                Y = 4;
            }

            lastKeyState = keyState; // Get the previous keyboard state

            return Y;
        }
    }
}