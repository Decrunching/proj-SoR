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
        public Dictionary<Keys, InputKeys> InputCollection { get; set; }

        public KeyboardInput()
        {
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
         * 
         */
        public string CheckKeyInput()
        {
            string animation = "none";

            keyState = Keyboard.GetState(); // Get the current keyboard state

            if (InputCollection.Values.All(inputKeys => !inputKeys.Pressed)) // If no keys are being pressed
            {
                animation = "idlebattle";
            }

            lastKeyState = keyState; // Get the previous keyboard state

            return animation;
        }
    }
}