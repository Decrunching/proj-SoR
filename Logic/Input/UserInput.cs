using Spine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace SoR.Logic.Input
{
    /*
     * This class handles player input and animation application.
     */
    public class UserInput
    {
        private KeyboardState keyState;
        private KeyboardState lastKeyState;
        private Vector2 newPosition;
        private Vector2 prevPosition;
        private Vector2 maxPosition;
        private Dictionary<Keys, InputKeys> inputKeys;
        private int deadZone;
        private bool switchSkin;
        private bool idle;
        private string lastPressedKey;
        private int turnAround;
        private string animation;

        public UserInput()
        {
            deadZone = 4096; // Set the joystick deadzone
            idle = true; // Player is currently idle
            lastPressedKey = ""; // Get the last key pressed

            // Dictionary to store the input keys, whether they are currently up or pressed, and which animation to apply
            // TO DO: Simplify to remove duplicated code
            inputKeys = new Dictionary<Keys, InputKeys>()
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
         * Move left or right, and adjust animation accordingly.
         * 
         * TO DO?:
         * Adjust to retain current track number for incoming animations.
         * JSON files have exact times for frame starts if hardcoding.
         * AnimationState does return frame start times too, if puzzling out the API.
         * Fix this - possibly switch to idle animation while two opposing direction keys are
         * being held down with no other directional keys, and make player face the direction
         * of travel if 3 buttons held down simultaneously.
         */
        public void CheckMovement(
            GameTime gameTime,
            GraphicsDeviceManager graphics,
            AnimationState animState,
            float speed,
            Vector2 position)
        {
            keyState = Keyboard.GetState(); // Get the current keyboard state

            float newPlayerSpeed = speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            prevPosition = newPosition = position;

            if (inputKeys.Values.All(inputKeys => !inputKeys.Pressed)) // If no keys are being pressed
            {
                if (!idle) // If idle animation is not currently playing
                {
                    idle = true; // Idle is now playing
                    animation = "idlebattle"; // Set idle animation
                }
            }

            // Set player animation and position according to keyboard input
            foreach (var key in inputKeys.Keys)
            {
                bool pressed = keyState.IsKeyDown(key);
                bool previouslyPressed = lastKeyState.IsKeyDown(key);
                inputKeys[key].Pressed = pressed;

                if (pressed)
                {
                    if (inputKeys[key].NextAnimation == "runleft")
                    {
                        newPosition.X -= newPlayerSpeed;
                    }
                    else if (inputKeys[key].NextAnimation == "runright")
                    {
                        newPosition.X += newPlayerSpeed;
                    }
                    
                    if (inputKeys[key].NextAnimation == "runup")
                    {
                        newPosition.Y -= newPlayerSpeed;
                    }
                    else if (inputKeys[key].NextAnimation == "rundown")
                    {
                        newPosition.Y += newPlayerSpeed;
                    }

                    idle = false; // Idle will no longer be playing

                    lastPressedKey = inputKeys[key].NextAnimation;
                }

                if (pressed & !previouslyPressed)
                {
                    animation = inputKeys[key].NextAnimation;
                }

                if (!pressed & previouslyPressed)
                {
                    animation = lastPressedKey;
                }
            }

            ChangeSkin(graphics); // Check all non-directional user input

            lastKeyState = keyState; // Get the previous keyboard state

            // Get the previous x,y co-ordinates
            prevPosition = newPosition;
        }

        /*
         * Return the animation to play according to user input.
         */
        public string AnimateMovement()
        {
            return animation;
        }

        /*
         * All non-directional user input.
         */
        public void ChangeSkin(GraphicsDeviceManager graphics)
        {
            keyState = Keyboard.GetState(); // Get the current keyboard state

            switchSkin = false; // Space has not been pressed yet, the skin will not be switched

            if (keyState.IsKeyDown(Keys.Space) & !lastKeyState.IsKeyDown(Keys.Space))
            {
                switchSkin = true; // Space was pressed, so switch skins
            }
        }

        /*
         * Handle environmental collision. Currently just the edge of the game window.
         */
        public bool EnvironCollision(
            GraphicsDeviceManager graphics,
            GraphicsDevice GraphicsDevice,
            SkeletonBounds hitbox,
            Vector2 position,
            Vector2 maxPosition)
        {
            newPosition = position;
            this.maxPosition = maxPosition;

            if (newPosition.X > graphics.PreferredBackBufferWidth - hitbox.Width)
            {
                newPosition.X = graphics.PreferredBackBufferWidth - hitbox.Width;
                turnAround = 1; // Left
                return true;
            }
            else if (newPosition.X < hitbox.Width)
            {
                newPosition.X = hitbox.Width;
                turnAround = 2; // Right
                return true;
            }

            if (newPosition.Y > graphics.PreferredBackBufferHeight - hitbox.Height)
            {
                newPosition.Y = graphics.PreferredBackBufferHeight - hitbox.Height;
                turnAround = 3; // Up
                return true;
            }
            else if (newPosition.Y < hitbox.Height * 4)
            {
                newPosition.Y = hitbox.Height * 4;
                turnAround = 4; // Down
                return true;
            }
            return false;
        }

        /*
         * Return the current direction to face. 1 = left, 2 = right, 3 = up, 4 = down.
         */
        public int TurnAround()
        {
            return turnAround;
        }

        /*
         * Change the player's position on the screen according to joypad inputs
         */
        public void ProcessJoypadInputs(GameTime gameTime, float speed)
        {
            if (Joystick.LastConnectedIndex == 0)
            {
                JoystickState jstate = Joystick.GetState(0);

                float newPlayerSpeed = speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (jstate.Axes[1] < -deadZone)
                {
                    newPosition.Y -= newPlayerSpeed;
                }
                else if (jstate.Axes[1] > deadZone)
                {
                    newPosition.Y += newPlayerSpeed;
                }

                if (jstate.Axes[0] < -deadZone)
                {
                    newPosition.X -= newPlayerSpeed;
                }
                else if (jstate.Axes[0] > deadZone)
                {
                    newPosition.X += newPlayerSpeed;
                }
            }
        }

        /*
         * Get the new x-axis position.
         */
        public Vector2 UpdatePosition()
        {
            return newPosition;
        }

        /*
         * Check whether space was pressed and the skin should change.
         */
        public bool SkinHasChanged()
        {
            return switchSkin;
        }
    }
}