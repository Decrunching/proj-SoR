using Hardware.Input;
using Logic.GameMap;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Logic.Entities.Character.EntityMovement
{
    /*
     * This class handles player input and animation application.
     */
    public class Movement
    {
        private Random random;
        private GamePadInput gamePadInput;
        private KeyboardInput keyboardInput;
        private Vector2 newPosition;
        private Vector2 prevPosition;
        private Vector2 direction;
        private bool idle;
        private string lastAnimation;
        private string animation;
        private float sinceLastChange;
        private float newDirectionTime;
        public bool Traversable { get; private set; }
        public bool DirectionReversed { get; set; }
        public int CountDistance { get; set; }
        public bool BeenPushed { get; set; }

        public Movement()
        {
            random = new Random();
            gamePadInput = new GamePadInput();
            keyboardInput = new KeyboardInput();

            idle = true; // Player is currently idle
            lastAnimation = ""; // Get the last key pressed

            Traversable = true; // Whether the entity is on walkable terrain

            CountDistance = 0; // Count how far to automatically move the entity
            direction = new Vector2(0, 0); // The direction of movement
            sinceLastChange = 0; // Time since last NPC direction change
            newDirectionTime = (float)random.NextDouble() * 1f + 0.25f; // After 0.25-1 seconds, NPC chooses a new movement direction
            DirectionReversed = false;
            BeenPushed = false;

        }

        /*
         * Check whether player is idle.
         */
        public void CheckIdle()
        {
            if (keyboardInput.CheckXMoveInput() == 0 && keyboardInput.CheckYMoveInput() == 0 |
                gamePadInput.CheckXMoveInput() == 0 && gamePadInput.CheckYMoveInput() == 0)
            {
                SetIdle();
                if (CountDistance == 0)
                {
                    direction = Vector2.Zero;
                }
            }
        }

        /*
         * Set idle animation if player character idle.
         */
        public void SetIdle()
        {
            if (!idle) // If idle animation is not currently playing
            {
                idle = true; // Idle is now playing
                animation = "idlebattle"; // Set idle animation
            }
        }

        /* 
         * Move left or right, and adjust animation accordingly.
         */
        public void CheckMovement(Vector2 position)
        {
            newPosition = prevPosition = position;

            CheckIdle();

            gamePadInput.CheckXMoveInput();
            gamePadInput.CheckYMoveInput();

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

            lastAnimation = animation;
        }

        /*
         * Process keyboard and gamepad x-axis movement inputs.
         */
        public void ProcessXMovementInput( int x)
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
                    animation = "runleft";
                    break;
                case 2:
                    MovementDirectionX(1);
                    idle = false;
                    animation = "runright";
                    break;
                case 3:
                    direction.X = 0;
                    animation = lastAnimation;
                    break;
                case 4:
                    direction.X = 0;
                    animation = "idlebattle";
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
                    animation = "runup";
                    idle = false;
                    break;
                case 2:
                    MovementDirectionY(1);
                    animation = "rundown";
                    idle = false;
                    break;
                case 3:
                    direction.Y = 0;
                    animation = lastAnimation;
                    break;
                case 4:
                    direction.Y = 0;
                    animation = "idlebattle";
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

        /*
         * Calculate the direction to be repelled in. Positive to move right or down, negative to move up or left.
         */
        public float RepelDirection(float direction, bool positive)
        {
            if (positive)
            {
                direction += 1;
                if (direction > 1)
                {
                    direction = 1;
                }
            }
            else
            {
                direction -= 1;
                if (direction < -1)
                {
                    direction = -1;
                }
            }

            return direction;
        }

        /*
         * Be repelled away from an entity.
         */
        public void RepelledFromEntity(Vector2 location, int distance, Entity entity)
        {
            CountDistance = distance;

            direction = Vector2.Zero;

            if (entity.GetPosition().X > location.X) // Push right
            {
                direction.X = RepelDirection(direction.X, false);
            }
            else if (entity.GetPosition().X < location.X) // Push left
            {
                direction.X = RepelDirection(direction.X, true);
            }
            if (entity.GetPosition().Y > location.Y) // Push down
            {
                direction.Y = RepelDirection(direction.Y, false);
            }
            else if (entity.GetPosition().Y < location.Y) // Push up
            {
                direction.Y = RepelDirection(direction.Y, true);
            }
        }

        /*
         * Be repelled away from scenery.
         */
        public void RepelledFromScenery(Vector2 location, int distance, Scenery scenery)
        {
            CountDistance = distance;

            direction = Vector2.Zero;

            if (scenery.GetPosition().X > location.X) // Push right
            {
                direction.X = RepelDirection(direction.X, false);
            }
            else if (scenery.GetPosition().X < location.X) // Push left
            {
                direction.X = RepelDirection(direction.X, true);
            }
            if (scenery.GetPosition().Y > location.Y) // Push down
            {
                direction.Y = RepelDirection(direction.Y, false);
            }
            else if (scenery.GetPosition().Y < location.Y) // Push up
            {
                direction.Y = RepelDirection(direction.Y, true);
            }
        }

        /*
         * Change direction to move away from something.
         */
        public void RedirectNPC(Vector2 prevPosition, Entity entity)
        {
            if (newPosition.X > prevPosition.X)
            {
                NewDirection(entity, 1); // Redirect left
            }
            else if (newPosition.X < prevPosition.X)
            {
                NewDirection(entity, 2); // Redirect right
            }

            if (newPosition.Y > prevPosition.Y)
            {
                NewDirection(entity, 3); // Redirect up
            }
            else if (newPosition.Y < prevPosition.Y)
            {
                NewDirection(entity, 4); // Redirect down
            }
        }

        /*
         * Choose a new direction to face.
         */
        public void NewDirection(Entity entity, int newDirection)
        {
            switch (newDirection)
            {
                case 1:
                    direction = new Vector2(-1, 0); // Left
                    RedirectAnimation(1, entity);
                    break;
                case 2:
                    direction = new Vector2(1, 0); // Right
                    RedirectAnimation(2, entity);
                    break;
                case 3:
                    direction = new Vector2(0, -1); // Up
                    break;
                case 4:
                    direction = new Vector2(0, 1); // Down
                    break;
            }
        }

        /*
         * Animate NPC redirection.
         */
        public void RedirectAnimation(int newDirection, Entity entity)
        {
            switch (newDirection)
            {
                case 1:
                    entity.ChangeAnimation("turnleft");
                    entity.GetSkeleton().ScaleX = 1;
                    break;
                case 2:
                    entity.ChangeAnimation("turnright");
                    entity.GetSkeleton().ScaleX = -1;
                    break;
            }
        }

        /*
         * Move the NPC in the direction they're facing, and periodically pick a random new direction.
         */
        public void NonPlayerMovement(GameTime gameTime, Entity entity)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            int newDirection = 0;
            sinceLastChange += deltaTime;
            newPosition = entity.GetPosition();

            if (entity.IsMoving())
            {
                if (sinceLastChange >= newDirectionTime || BeenPushed)
                {
                    newDirection = random.Next(4);
                    NewDirection(entity, newDirection);
                    newDirectionTime = (float)random.NextDouble() * 3f + 0.33f;
                    sinceLastChange = 0;
                    BeenPushed = false;
                }
            }

            prevPosition = entity.GetPosition();
        }

        /*
         * Set the new position after moving, and halve the speed if moving diagonally.
         */
        public void AdjustPosition(GameTime gameTime, Entity entity, List<Rectangle> impassableArea)
        {
            float newSpeed = (float)(entity.Speed * 1.5) * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (direction.X > 0 | direction.X < 0 && direction.Y > 0 | direction.Y < 0) // If moving diagonally
            {
                newSpeed /= 1.5f; // Reduce the speed by 25%
            }

            newPosition += direction * newSpeed;

            foreach (Rectangle area in impassableArea)
            {
                if (area.Contains(newPosition) && !area.Contains(prevPosition))
                {
                    direction = Vector2.Zero;

                    if (!entity.Player) // If entity is not the player
                    {
                        if (!DirectionReversed) // If the direction has not already been reversed
                        {
                            RedirectNPC(prevPosition, entity); // Move in the opposite direction
                            DirectionReversed = true;
                        }
                    }

                    Traversable = false;
                    newPosition = prevPosition;

                    break;
                }
                if (area.Contains(newPosition) && area.Contains(prevPosition)) // If entity is stuck inside the wall
                {
                    bool left = prevPosition.X < area.Center.X;
                    bool right = prevPosition.X > area.Center.X;
                    bool top = prevPosition.Y < area.Center.Y;
                    bool bottom = prevPosition.Y > area.Center.Y;

                    if (left) // If it is in the left half of the wall
                    {
                        newPosition.X -= newSpeed; // Move the entity left
                    }
                    else if (right)
                    {
                        newPosition.X += newSpeed;
                    }

                    if (top)
                    {
                        newPosition.Y -= newSpeed;
                    }
                    else if (bottom)
                    {
                        newPosition.Y += newSpeed;
                    }
                }
                else
                {
                    Traversable = true;
                }

            }
        }

        /*
         * Reverse the direction of travel.
         */
        public int ReverseDirection(float direction)
        {
            if (direction > 0)
            {
                return -1;
            }
            else if (direction < 0)
            {
                return 1;
            }
            else return 0;
        }

        /*
         * Set the direction to be moved in.
         */
        public void SetDirection(float x, float y)
        {
            direction.X = x;
            direction.Y = y;
        }

        /*
         * Return the animation to play according to user input.
         */
        public string AnimateMovement()
        {
            return animation;
        }

        /*
         * Get the current movement direction.
         */
        public Vector2 GetDirection()
        {
            return direction;
        }

        /*
         * Get the new x-axis position.
         */
        public Vector2 UpdatePosition()
        {
            return newPosition;
        }
    }
}