using Spine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace SoR.Logic.Input
{
    /*
     * This class handles player input and animation application.
     */
    internal class PlayerInput
    {
        private Vector2 position;
        private KeyboardState lastKeyState;
        private float speed;
        private int deadZone;

        public PlayerInput(GraphicsDeviceManager _graphics)
        {
            // Set the player's current position on the screen
            position = new Vector2(_graphics.PreferredBackBufferWidth / 2,
                _graphics.PreferredBackBufferHeight / 2);

            speed = 200f;    // Set the player's speed
            deadZone = 4096; // Set the joystick deadzone}
        }

        /*
         * Set the current running animation and move the player character across the screen according
         * to keyboard inputs. Set back to the idle animation if there are no current valid movement
         * inputs.
         */
        public void ProcessKeyboardInputs(GameTime gameTime, KeyboardState keyState, AnimationState animState)
        {
            //Anims: fdown, fdownidle, fside, fsideidle, fup, fupidle, mdown, mdownidle, mside, msideidle, mup, mupidle

            Keys[] keysPressed = keyState.GetPressedKeys(); // Collect a list of keys currently being pressed
            Keys[] lastKeysPressed = new Keys[0]; // Collect a list of previously pressed keys that have just been released

            // Dictionary to store the input keys and whether they are currently up or pressed.
            Dictionary<Keys, bool> keyIsUp =
                new Dictionary<Keys, bool>()
                {
                    { Keys.Up, keyState.IsKeyUp(Keys.Up) },
                    { Keys.Down, keyState.IsKeyUp(Keys.Down) },
                    { Keys.Left, keyState.IsKeyUp(Keys.Left) },
                    { Keys.Right, keyState.IsKeyUp(Keys.Right) }
                };

            /* Set player animation and position according to keyboard input.
             * 
             * TO DO?:
             * Adjust to retain current track number for incoming animations.
             */
            if (keyState.IsKeyDown(Keys.Up))
            {
                position.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (!lastKeyState.IsKeyDown(Keys.Up))
                {
                    animState.SetAnimation(0, "runup", true);
                }
            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                position.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (!lastKeyState.IsKeyDown(Keys.Down))
                {
                    animState.SetAnimation(0, "rundown", true);
                }
            }
            if (keyState.IsKeyDown(Keys.Left))
            {
                position.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (!lastKeyState.IsKeyDown(Keys.Left))
                {
                    animState.SetAnimation(0, "runleft", true);
                }
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (!lastKeyState.IsKeyDown(Keys.Right))
                {
                    animState.SetAnimation(0, "runright", true);
                }
            }

            /*
             * If a key has just been released, set the running animation back to the direction the character
             * is currently moving in. If two keys are being pressed simultaneously, set it to the direction
             * of the most recently pressed key.
             */
            foreach (Keys key in keyIsUp.Keys)
            {
                if (!keyState.IsKeyDown(key) & lastKeyState.IsKeyDown(key))
                {
                    if (keyState.IsKeyDown(Keys.Right) &
                        !keyState.IsKeyDown(Keys.Left))
                    {
                        animState.SetAnimation(0, "runright", true);
                    }
                    if (keyState.IsKeyDown(Keys.Left) &
                        !keyState.IsKeyDown(Keys.Right))
                    {
                        animState.SetAnimation(0, "runleft", true);
                    }
                    if (keyState.IsKeyDown(Keys.Down) &
                        !keyState.IsKeyDown(Keys.Up))
                    {
                        animState.SetAnimation(0, "rundown", true);
                    }
                    if (keyState.IsKeyDown(Keys.Up) &
                        !keyState.IsKeyDown(Keys.Down))
                    {
                        animState.SetAnimation(0, "runup", true);
                    }
                    else if (!keyIsUp.ContainsValue(false))
                    {
                        animState.SetAnimation(0, "idle", true);
                    }
                }
            }

            lastKeyState = keyState; // The previous keyboard state
            lastKeysPressed = keysPressed; // An array of keys that were previously being pressed
        }

        /*
         * Change the player's position on the screen according to joypad inputs
         */
        public void ProcessJoypadInputs(GameTime gameTime)
        {
            if (Joystick.LastConnectedIndex == 0)
            {
                JoystickState jstate = Joystick.GetState(0);

                float updatedcharSpeed = speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (jstate.Axes[1] < -deadZone)
                {
                    position.Y -= updatedcharSpeed;
                }
                else if (jstate.Axes[1] > deadZone)
                {
                    position.Y += updatedcharSpeed;
                }

                if (jstate.Axes[0] < -deadZone)
                {
                    position.X -= updatedcharSpeed;
                }
                else if (jstate.Axes[0] > deadZone)
                {
                    position.X += updatedcharSpeed;
                }
            }
        }

        /*
         * Get the current X position.
         */
        public float GetPositionX()
        {
            return position.X;
        }

        /*
         * Get the current Y position.
         */
        public float GetPositionY()
        {
            return position.Y;
        }
    }
}