using Spine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace SoR.Logic.Input
{
    /*
     * This class handles player input and animation application.
     */
    public class PlayerInput
    {
        public Direction EntityDirection { get; private set; }
        private KeyboardState lastKeyState;
        private int deadZone;
        private float speed;
        private float newPositionX;
        private float newPositionY;

        public enum Direction
        {
            RunUp,
            RunDown,
            RunLeft,
            RunRight,
            IdleUp,
            IdleDown,
            IdleLeft,
            IdleRight,
            IdleBattle
        }

        public PlayerInput()
        {
            // Set the joystick deadzone
            deadZone = 4096;
        }

        /*
         * Set the current running animation and move the player character across the screen according
         * to keyboard inputs. Set back to the idle animation if there are no current valid movement
         * inputs.
         */
        public void ProcessKeyboardInputs(
            GameTime gameTime,
            KeyboardState keyState,
            AnimationState animState,
            float speed,
            float positionX,
            float positionY)
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

            float newPlayerSpeed = speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.speed = speed;
            newPositionX = positionX;
            newPositionY = positionY;

            /* Set player animation and position according to keyboard input.
             * 
             * TO DO?:
             * Adjust to retain current track number for incoming animations.
             */
            if (keyState.IsKeyDown(Keys.Up))
            {
                newPositionY -= newPlayerSpeed;
                if (!lastKeyState.IsKeyDown(Keys.Up))
                {
                    EntityDirection = Direction.RunUp;
                }
            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                newPositionY += newPlayerSpeed;
                if (!lastKeyState.IsKeyDown(Keys.Down))
                {
                    EntityDirection = Direction.RunDown;
                }
            }
            if (keyState.IsKeyDown(Keys.Left))
            {
                newPositionX -= newPlayerSpeed;
                if (!lastKeyState.IsKeyDown(Keys.Left))
                {
                    EntityDirection = Direction.RunLeft;
                }
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                newPositionX += newPlayerSpeed;
                if (!lastKeyState.IsKeyDown(Keys.Right))
                {
                    EntityDirection = Direction.RunRight;
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
                        EntityDirection = Direction.RunRight;
                    }
                    if (keyState.IsKeyDown(Keys.Left) &
                        !keyState.IsKeyDown(Keys.Right))
                    {
                        EntityDirection = Direction.RunLeft;
                    }
                    if (keyState.IsKeyDown(Keys.Down) &
                        !keyState.IsKeyDown(Keys.Up))
                    {
                        EntityDirection = Direction.RunDown;
                    }
                    if (keyState.IsKeyDown(Keys.Up) &
                        !keyState.IsKeyDown(Keys.Down))
                    {
                        EntityDirection = Direction.RunUp;
                    }
                    else if (!keyIsUp.ContainsValue(false))
                    {
                        EntityDirection = Direction.IdleBattle;
                    }
                }
            }

            lastKeyState = keyState; // The previous keyboard state
            lastKeysPressed = keysPressed; // An array of keys that were previously being pressed
        }

        /*
         * Prevent the player from leaving the visible screen area.
         */
        public void CheckScreenEdges(GraphicsDeviceManager _graphics,
            GraphicsDevice GraphicsDevice,
            float positionX,
            float positionY)
        {
            newPositionX = positionX;
            newPositionY = positionY;

            if (newPositionX > _graphics.PreferredBackBufferWidth - 5)
            {
                newPositionX = _graphics.PreferredBackBufferWidth - 5;
            }
            else if (newPositionX < 5)
            {
                newPositionX = 5;
            }

            if (newPositionY > _graphics.PreferredBackBufferHeight - 8)
            {
                newPositionY = _graphics.PreferredBackBufferHeight - 8;
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
            this.speed = speed;

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
    }
}