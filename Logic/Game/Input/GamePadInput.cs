using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;

namespace Logic.Game.Input
{
    public class GamePadInput
    {
        private GamePadListener gamePadListener;
        private GamePadState gamePadState;
        private GamePadState lastGamePadState;
        private GamePadCapabilities gamePadCapabilities;
        private Vector2 direction;
        public bool Idle { get; set; }

        public GamePadInput()
        {
            gamePadListener = new GamePadListener();
            gamePadCapabilities = GamePad.GetCapabilities(PlayerIndex.One);

            Idle = true;
        }

        /*
         * Check for and process gamepad input.
         */
        public string CheckThumbstickInput()
        {
            string animation = "none";

            if (gamePadCapabilities.IsConnected) // If the gamepad is connected
            {
                gamePadState = GamePad.GetState(PlayerIndex.One); // Get the current gamepad state

                if (gamePadState.ThumbSticks.Left.X < -0.5f)
                {
                    ThumbstickDirection(-1);
                    animation = "runleft";
                }
                else if (gamePadState.ThumbSticks.Left.X > 0.5f)
                {
                    ThumbstickDirection(1);
                    animation = "runright";
                }
                else if (gamePadState.ThumbSticks.Left.X > -0.5f &&
                    gamePadState.ThumbSticks.Left.X < 0.5f &&
                    lastGamePadState.ThumbSticks.Left.X < -0.5f ||
                    lastGamePadState.ThumbSticks.Left.X > 0.5f)
                {
                    direction.X = 0;
                }

                if (gamePadState.ThumbSticks.Left.Y < -0.5f)
                {
                    ThumbstickDirection(0, 1);
                    animation = "rundown";
                }
                else if (gamePadState.ThumbSticks.Left.Y > 0.5f)
                {
                    ThumbstickDirection(0, -1);
                    animation = "runup";
                }
                else if (gamePadState.ThumbSticks.Left.Y > -0.5f &&
                    gamePadState.ThumbSticks.Left.Y < 0.5f &&
                    lastGamePadState.ThumbSticks.Left.Y < -0.5f ||
                    lastGamePadState.ThumbSticks.Left.Y > 0.5f)
                {
                    direction.Y = 0;
                }

                lastGamePadState = gamePadState;
            }

            return animation;
        }

        /*
         * Change the player direction according to gamepad input.
         */
        public void ThumbstickDirection(float changeDirectionX = 0, float changeDirectionY = 0)
        {
            if (changeDirectionX != 0)
            {
                direction.X = changeDirectionX;
                Idle = false;
            }

            if (changeDirectionY != 0)
            {
                direction.Y = changeDirectionY;
                Idle = false;
            }
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

        /*
         * Check whether the player is now idle.
         */
        public bool CheckIdle()
        {
            return Idle;
        }
    }
}
