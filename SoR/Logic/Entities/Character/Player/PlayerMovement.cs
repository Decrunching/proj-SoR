using Hardware.Input;
using Microsoft.Xna.Framework;

namespace Logic.Entities.Character.Player
{
    internal partial class Player
    {
        protected GamePadInput gamePadInput;
        protected KeyboardInput keyboardInput;
        protected string lastAnimation;
        protected string movementAnimation;

        /*
         * Check whether player is idle.
         */
        public void CheckIdle()
        {
            if (keyboardInput.CheckXMoveInput() == 0 && keyboardInput.CheckYMoveInput() == 0 |
                gamePadInput.CheckXMoveInput() == 0 && gamePadInput.CheckYMoveInput() == 0)
            {
                if (!idle) // If idle animation is not currently playing
                {
                    idle = true; // Idle is now playing
                    movementAnimation = "idlebattle"; // Set idle animation
                }
                if (CountDistance == 0)
                {
                    direction = Vector2.Zero;
                }
            }
        }

        /*
         * Process keyboard and gamepad x-axis movement inputs.
         */
        public void ProcessXMovementInput(int x)
        {
            switch (x)
            {
                case 0:
                    if (CountDistance == 0)
                    {
                        direction.X = 0;
                    }
                    break;
                case 1:
                    MovementDirectionX(-1);
                    idle = false;
                    movementAnimation = "runleft";
                    break;
                case 2:
                    MovementDirectionX(1);
                    idle = false;
                    movementAnimation = "runright";
                    break;
                case 3:
                    direction.X = 0;
                    movementAnimation = lastAnimation;
                    break;
                case 4:
                    direction.X = 0;
                    movementAnimation = "idlebattle";
                    break;
            }
        }

        /*
         * Process keyboard and gamepad y-axis movement inputs.
         */
        public void ProcessYMovementInput(int y)
        {
            switch (y)
            {
                case 0:
                    if (CountDistance == 0)
                    {
                        direction.Y = 0;
                    }
                    break;
                case 1:
                    MovementDirectionY(-1);
                    movementAnimation = "runup";
                    idle = false;
                    break;
                case 2:
                    MovementDirectionY(1);
                    movementAnimation = "rundown";
                    idle = false;
                    break;
                case 3:
                    direction.Y = 0;
                    movementAnimation = lastAnimation;
                    break;
                case 4:
                    direction.Y = 0;
                    movementAnimation = "idlebattle";
                    break;
            }
        }

        /*
         * Change the player x-axis direction according to keyboard input.
         */
        public void MovementDirectionX(int changeDirection)
        {
            if (changeDirection != 0)
            {
                direction.X = changeDirection;
            }
        }

        /*
         * Change the player y-axis direction according to keyboard input.
         */
        public void MovementDirectionY(int changeDirection)
        {
            if (changeDirection != 0)
            {
                direction.Y = changeDirection;
            }
        }
    }
}
