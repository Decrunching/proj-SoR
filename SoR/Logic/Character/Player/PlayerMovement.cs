using Microsoft.Xna.Framework;

namespace SoR.Logic.Character.Player
{
    internal partial class Player : Entity
    {
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

        /*
         * Update entity position.
         */
        public override void UpdatePosition(GameTime gameTime, GraphicsDeviceManager graphics)
        {
            FrozenTimer(gameTime);

            if (!Frozen)
            {
                CheckIdle();

                if (gamePadInput.CurrentInputDevice)
                {
                    keyboardInput.CurrentInputDevice = false;
                    ProcessXMovementInput(gamePadInput.CheckXMoveInput());
                    ProcessYMovementInput(gamePadInput.CheckYMoveInput());
                }

                if (keyboardInput.CurrentInputDevice)
                {
                    gamePadInput.CurrentInputDevice = false;
                    ProcessXMovementInput(keyboardInput.CheckXMoveInput());
                    ProcessYMovementInput(keyboardInput.CheckYMoveInput());
                }

                BeMoved(gameTime);

                AdjustPosition(gameTime, ImpassableArea);

                lastAnimation = movementAnimation;
            }
        }
    }
}