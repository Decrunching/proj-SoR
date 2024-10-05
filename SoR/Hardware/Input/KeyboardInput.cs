using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

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
        public int XAxisInput { get; private set; }
        public int YAxisInput { get; private set; }
        public bool CurrentInputDevice { get; set; }

        public KeyboardInput()
        {
            CurrentInputDevice = false;
            XAxisInput = 0;
            YAxisInput = 0;

            // Dictionary to store the input keys, whether they are currently up or pressed, and which animation to apply
            InputCollection = new Dictionary<Keys, InputKeys>()
            {
            { Keys.Up, new InputKeys(keyState.IsKeyDown(Keys.Up), "runup") },
            { Keys.W, new InputKeys(keyState.IsKeyDown(Keys.W), "runup") },
            { Keys.Down, new InputKeys(keyState.IsKeyDown(Keys.Down), "rundown") },
            { Keys.S, new InputKeys(keyState.IsKeyDown(Keys.S), "rundown") },
            { Keys.Left, new InputKeys(keyState.IsKeyDown(Keys.Left), "runleft") },
            { Keys.A, new InputKeys(keyState.IsKeyDown(Keys.A), "runleft") },
            { Keys.Right, new InputKeys(keyState.IsKeyDown(Keys.Right), "runright") },
            { Keys.D, new InputKeys(keyState.IsKeyDown(Keys.D), "runright") }
            };
        }

        /*
         * Check for and process keyboard movement inputs.
         */
        public void CheckMoveInput()
        {
            keyState = Keyboard.GetState(); // Get the current keyboard state

            if (InputCollection.Values.All(inputKeys => !inputKeys.Pressed)) // If no keys are being pressed
            {
                XAxisInput = 0;
                YAxisInput = 0;
            }

            foreach (var key in InputCollection.Keys) // Check the state of the movement input keys
            {
                bool pressed = keyState.IsKeyDown(key);
                bool previouslyPressed = lastKeyState.IsKeyDown(key);
                InputCollection[key].Pressed = pressed;

                if (InputCollection[key].NextAnimation == "runleft" && pressed)
                {
                    CurrentInputDevice = true;
                    XAxisInput = 1;
                    if (!previouslyPressed)
                    {
                        XAxisInput = 3;
                    }
                }
                else if (InputCollection[key].NextAnimation == "runright" && pressed)
                {
                    CurrentInputDevice = true;
                    XAxisInput = 2;
                    if (!previouslyPressed)
                    {
                        XAxisInput = 4;
                    }
                }
                else
                {
                    XAxisInput = 0;
                }

                if (InputCollection[key].NextAnimation == "runup" && pressed)
                {
                    CurrentInputDevice = true;
                    YAxisInput = 1;
                    if (!previouslyPressed)
                    {
                        XAxisInput = 3;
                    }
                }
                else if (InputCollection[key].NextAnimation == "rundown" && pressed)
                {
                    CurrentInputDevice = true;
                    YAxisInput = 2;
                    if (!previouslyPressed)
                    {
                        XAxisInput = 4;
                    }
                }
                else
                {
                    YAxisInput = 0;
                }

                if (!pressed & previouslyPressed)
                {
                    XAxisInput = 5;

                    if (InputCollection[key].NextAnimation == "runleft" || InputCollection[key].NextAnimation == "runright")
                    {
                        XAxisInput = 0;
                    }
                }
            }

            lastKeyState = keyState; // Get the previous keyboard state
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