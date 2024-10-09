using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;

namespace SoR.Hardware.Input
{
    public class GamePadInput
    {
        private GamePadListener gamePadListener;
        private GamePadState gamePadState;
        private GamePadState lastGamePadState;
        private GamePadCapabilities gamePadCapabilities;
        public bool CurrentInputDevice { get; set; }

        public GamePadInput()
        {
            CurrentInputDevice = false;

            gamePadListener = new GamePadListener();
            gamePadCapabilities = GamePad.GetCapabilities(PlayerIndex.One);
        }

        /*
         * Check for and process gamepad input.
         */
        public int CheckXMoveInput()
        {
            int xAxisInput = 0;

            if (gamePadCapabilities.IsConnected) // If the gamepad is connected
            {
                gamePadState = GamePad.GetState(PlayerIndex.One); // Get the current gamepad state

                if (gamePadState.ThumbSticks.Left.X < -0.5f)
                {
                    CurrentInputDevice = true;
                    xAxisInput = 1;
                }
                else if (gamePadState.ThumbSticks.Left.X > 0.5f)
                {
                    CurrentInputDevice = true;
                    xAxisInput = 2;
                }

                if (gamePadState.ThumbSticks.Left.X !< -0.5f &&
                    gamePadState.ThumbSticks.Left.X !> 0.5f)
                {
                    xAxisInput = 0;
                }

                lastGamePadState = gamePadState;
            }

            return xAxisInput;
        }

        /*
         * Check for and process gamepad input.
         */
        public int CheckYMoveInput()
        {
            int yAxisInput = 0;

            if (gamePadCapabilities.IsConnected) // If the gamepad is connected
            {
                gamePadState = GamePad.GetState(PlayerIndex.One); // Get the current gamepad state

                if (gamePadState.ThumbSticks.Left.Y < -0.5f)
                {
                    CurrentInputDevice = true;
                    yAxisInput = 2;
                }
                else if (gamePadState.ThumbSticks.Left.Y > 0.5f)
                {
                    CurrentInputDevice = true;
                    yAxisInput = 1;
                }

                if (gamePadState.ThumbSticks.Left.Y !< -0.5f &&
                    gamePadState.ThumbSticks.Left.Y !> 0.5f)
                {
                    yAxisInput = 0;
                }

                lastGamePadState = gamePadState;
            }

            return yAxisInput;
        }

        /*
         * Check button input.
         * A = Load. B = Save. Start = fullscreen/windowed. Back = exit. DPad = navigation.
         */
        public string CheckButtonInput()
        {
            string button = "none";

            if (gamePadCapabilities.IsConnected) // If the gamepad is connected
            {
                gamePadState = GamePad.GetState(PlayerIndex.One); // Get the current gamepad state

                if (gamePadState.Buttons.B == ButtonState.Pressed && lastGamePadState.Buttons.B != ButtonState.Pressed)
                {
                    button = "B";
                }
                if (gamePadState.Buttons.A == ButtonState.Pressed && lastGamePadState.Buttons.A != ButtonState.Pressed)
                {
                    button = "A";
                }
                if (gamePadState.Buttons.Start == ButtonState.Pressed && lastGamePadState.Buttons.Start != ButtonState.Pressed)
                {
                    button = "Start";
                }
                if (gamePadState.DPad.Up == ButtonState.Pressed &&
                    lastGamePadState.DPad.Up != ButtonState.Pressed)
                {
                    button = "Up";
                }
                if (gamePadState.DPad.Down == ButtonState.Pressed &&
                    lastGamePadState.DPad.Down != ButtonState.Pressed)
                {
                    button = "Down";
                }

                lastGamePadState = gamePadState;
            }

            return button;
        }
    }
}
