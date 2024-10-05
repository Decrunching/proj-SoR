using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Logic.Game.Input
{
    /*
     * Handle keyboard input.
     */
    public class KeyboardInput
    {
        private KeyboardState keyState;
        private KeyboardState lastKeyState;
        private Vector2 direction;
        private string lastPressedKey;
        public Dictionary<Keys, InputKeys> InputCollection { get; set; }
        public bool Idle { get; set; }

        public KeyboardInput()
        {
            lastPressedKey = "none";
            Idle = true;

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
        public string CheckMoveInput()
        {
            string animation = "none";

            keyState = Keyboard.GetState(); // Get the current keyboard state

            if (InputCollection.Values.All(inputKeys => !inputKeys.Pressed)) // If no keys are being pressed
            {
                animation = "idlebattle";
            }
            foreach (var key in InputCollection.Keys) // Check the state of the movement input keys
            {
                bool pressed = keyState.IsKeyDown(key);
                bool previouslyPressed = lastKeyState.IsKeyDown(key);
                InputCollection[key].Pressed = pressed;

                if (InputCollection[key].NextAnimation == "runleft")
                {
                    MovementDirection(key, pressed, previouslyPressed, -1);
                }
                else if (InputCollection[key].NextAnimation == "runright")
                {
                    MovementDirection(key, pressed, previouslyPressed, 1);
                }

                if (InputCollection[key].NextAnimation == "runup")
                {
                    MovementDirection(key, pressed, previouslyPressed, 0, -1);
                }
                else if (InputCollection[key].NextAnimation == "rundown")
                {
                    MovementDirection(key, pressed, previouslyPressed, 0, 1);
                }

                if (pressed & !previouslyPressed)
                {
                    animation = InputCollection[key].NextAnimation;
                }

                if (!pressed & previouslyPressed)
                {
                    animation = lastPressedKey;

                    // ??? Remove once joypad fixed
                    if (InputCollection[key].NextAnimation == "runleft" || InputCollection[key].NextAnimation == "runright")
                    {
                        direction.X = 0;
                    }
                }
            }

            lastKeyState = keyState; // Get the previous keyboard state

            return animation;
        }

        /*
         * Change the player direction according to keyboard input.
         */
        public void MovementDirection(Keys key, bool pressed, bool previouslyPressed, int changeDirectionX = 0, int changeDirectionY = 0)
        {
            if (changeDirectionX != 0)
            {
                if (pressed)
                {
                    direction.X = changeDirectionX;
                    lastPressedKey = InputCollection[key].NextAnimation;
                    Idle = false;
                }
                if (!pressed & previouslyPressed)
                {
                    direction.X = 0;
                }
            }

            if (changeDirectionY != 0)
            {
                if (pressed)
                {
                    direction.Y = changeDirectionY;
                    lastPressedKey = InputCollection[key].NextAnimation;
                    Idle = false;
                }
                if (!pressed & previouslyPressed)
                {
                    direction.Y = 0;
                }
            }
        }

        /*
         * Check keyboard input.
         * F4 = toggle fullscreen. F8 = save. F9 = load. Esc = exit.
         */
        public string CheckOtherInput()
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

            lastKeyState = keyState; // Get the previous keyboard state

            return key;
        }

        /*
         * Get the current direction of travel.
         */
        public Vector2 GetDirection()
        {
            return direction;
        }
    }
}