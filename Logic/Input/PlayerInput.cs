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
    public class PlayerInput
    {
        private KeyboardState keyState;
        private KeyboardState lastKeyState;
        private Dictionary<Keys, InputKeys> inputKeys;
        private int deadZone;
        private float newPositionX;
        private float newPositionY;
        private float prevPositionX;
        private float prevPositionY;
        private bool switchSkin;
        private bool idle;
        private string lastPressedKey;

        public PlayerInput()
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
            AnimationState animState,
            float speed,
            float positionX,
            float positionY)
        {
            keyState = Keyboard.GetState(); // Get the current keyboard state

            float newPlayerSpeed = speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            newPositionX = positionX;
            newPositionY = positionY;
            prevPositionX = newPositionX;
            prevPositionY = newPositionY;

            CheckIdle(animState); // Set idle if the player is not moving

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
                        newPositionX -= newPlayerSpeed;
                    }
                    if (inputKeys[key].NextAnimation == "runright")
                    {
                        newPositionX += newPlayerSpeed;
                    }
                    if (inputKeys[key].NextAnimation == "runup")
                    {
                        newPositionY -= newPlayerSpeed;
                    }
                    if (inputKeys[key].NextAnimation == "rundown")
                    {
                        newPositionY += newPlayerSpeed;
                    }

                    idle = false; // Idle will no longer be playing

                    lastPressedKey = inputKeys[key].NextAnimation;
                }

                SetNextAnimation(animState, key, pressed, previouslyPressed);
            }

            lastKeyState = keyState; // Get the previous keyboard state

            // Get the previous x,y co-ordinates
            prevPositionX = newPositionX;
            prevPositionY = newPositionY;
        }

        /*
         * If a key was just pressed, set the new animation to play. If a key has just been released, set
         * the animation to the new direction of movement.
         */
        public void SetNextAnimation(AnimationState animState, Keys key, bool pressed, bool previouslyPressed)
        {
            if (pressed & !previouslyPressed)
            {
                animState.SetAnimation(0, inputKeys[key].NextAnimation, true);
            }

            if (!pressed & previouslyPressed)
            {
                animState.SetAnimation(0, lastPressedKey, true);
            }
        }

        /*
         * Set idle if the player is not moving.
         */
        public void CheckIdle(AnimationState animState)
        {
            if (inputKeys.Values.All(inputKeys => !inputKeys.Pressed)) // If no keys are being pressed
            {
                if (!idle) // If idle animation is not currently playing
                {
                    animState.SetAnimation(0, "idlebattle", true); // Set idle animation
                    idle = true; // Idle is now playing
                }
            }
        }

        public void CheckSkin()
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
        public void EnvironCollision(
            GraphicsDeviceManager graphics,
            GraphicsDevice GraphicsDevice,
            float positionX,
            float positionY)
        {
            newPositionX = positionX;
            newPositionY = positionY;

            if (newPositionX > graphics.PreferredBackBufferWidth - 5)
            {
                newPositionX = graphics.PreferredBackBufferWidth - 5;
            }
            else if (newPositionX < 5)
            {
                newPositionX = 5;
            }

            if (newPositionY > graphics.PreferredBackBufferHeight - 8)
            {
                newPositionY = graphics.PreferredBackBufferHeight - 8;
            }
            else if (newPositionY < 8)
            {
                newPositionY = 8;
            }
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
                    newPositionY -= newPlayerSpeed;
                }
                else if (jstate.Axes[1] > deadZone)
                {
                    newPositionY += newPlayerSpeed;
                }

                if (jstate.Axes[0] < -deadZone)
                {
                    newPositionX -= newPlayerSpeed;
                }
                else if (jstate.Axes[0] > deadZone)
                {
                    newPositionX += newPlayerSpeed;
                }
            }
        }

        /*
         * Get the new x-axis position.
         */
        public float UpdatePositionX()
        {
            return newPositionX;
        }

        /*
         * Get the new y-axis position.
         */
        public float UpdatePositionY()
        {
            return newPositionY;
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