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
        private bool switchSkin;
        private bool idle;

        // DEBUGGING
        private string stringKeyState;
        private string stringLastKeyState;

        public PlayerInput()
        {
            deadZone = 4096; // Set the joystick deadzone
            idle = true; // Player is currently idle

            // Dictionary to store the input keys, whether they are currently up or pressed, and which animation to apply
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

            //DEBUGGING
            stringKeyState = "";
            stringLastKeyState = "";
        }

        /*
         * Set the current running animation and move the player character across the screen according
         * to keyboard inputs. Set back to the idle animation if there are no current valid movement
         * inputs.
         */
        public void ProcessKeyboardInputs(
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

            switchSkin = false; // Space has not been pressed yet, the skin will not be switched

            if (inputKeys.Values.All(inputKeys => !inputKeys.Pressed)) // If no keys are being pressed
            {
                if (!idle) // If idle animation is not currently playing
                {
                    animState.SetAnimation(0, "idlebattle", true); // Set idle animation
                    idle = true; // Idle is now playing
                }
            }

            /* Set player animation and position according to keyboard input.
             * 
             * TO DO?:
             * Adjust to retain current track number for incoming animations.
             * JSON files have exact times for frame starts if hardcoding.
             * AnimationState does return frame start times too, if puzzling out the API.
             */
            foreach (var key in inputKeys.Keys)
            {
                bool pressed = keyState.IsKeyDown(key);
                bool previouslyPressed = lastKeyState.IsKeyDown(key);
                inputKeys[key].Pressed = pressed;

                if (pressed)
                {
                    idle = false; // Idle will no longer be playing
                    if (inputKeys[key].NextAnimation == "runup")
                    {
                        newPositionY -= newPlayerSpeed;
                    }
                    if (inputKeys[key].NextAnimation == "rundown")
                    {
                        newPositionY += newPlayerSpeed;
                    }
                    if (inputKeys[key].NextAnimation == "runleft")
                    {
                        newPositionX -= newPlayerSpeed;
                    }
                    if (inputKeys[key].NextAnimation == "runright")
                    {
                        newPositionX += newPlayerSpeed;
                    }
                    if (!previouslyPressed)
                    {
                        animState.SetAnimation(0, inputKeys[key].NextAnimation, true); // Set new animation
                    }
                    stringKeyState = inputKeys[key].NextAnimation;
                }
                /*
                 * If a key has just been released, set the running animation back to the direction the character
                 * is currently moving in. If two keys are being pressed simultaneously, set it to the direction
                 * of the most recently pressed key.
                 * 
                 * TO DO: Fix this - doesn't quite work as intended and needs simplifying. Also need to switch to
                 * idle animation while two opposing direction keys are being held down with no other directional
                 * keys.
                 */
                else if (!pressed & previouslyPressed)
                {
                    if (stringKeyState == "runright")
                    {
                        animState.SetAnimation(0, "runright", true);
                    }
                    if (stringKeyState == "runleft")
                    {
                        animState.SetAnimation(0, "runleft", true);
                    }
                    if (stringKeyState == "rundown")
                    {
                        animState.SetAnimation(0, "rundown", true);
                    }
                    if (stringKeyState == "runup")
                    {
                        animState.SetAnimation(0, "runup", true);
                    }
                    stringLastKeyState = stringKeyState = inputKeys[key].NextAnimation;
                }
            }

            if (keyState.IsKeyDown(Keys.Space) & !lastKeyState.IsKeyDown(Keys.Space))
            {
                switchSkin = true; // Space was pressed, so switch skins
            }

            lastKeyState = keyState; // Get the previous keyboard state
        }

        public string GetStringKeyState()
        {
            return stringKeyState.ToString();
        }

        public string GetPrevStringKeyState()
        {
            return stringLastKeyState.ToString();
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

                float updatedcharSpeed = speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (jstate.Axes[1] < -deadZone)
                {
                    newPositionY -= updatedcharSpeed;
                }
                else if (jstate.Axes[1] > deadZone)
                {
                    newPositionY += updatedcharSpeed;
                }

                if (jstate.Axes[0] < -deadZone)
                {
                    newPositionX -= updatedcharSpeed;
                }
                else if (jstate.Axes[0] > deadZone)
                {
                    newPositionX += updatedcharSpeed;
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