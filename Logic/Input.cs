using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace SoR.Logic
{
    internal class Input : Player
    {
        private float speed;
        private int deadZone;
        protected string lastKey;
        protected bool keyPressed;

        public Input(Game1 game) : base(game)
        {
            lastKey = "down";
            keyPressed = false;
            speed = 400f;
            deadZone = 4096;
        }

        public void GetUserInput(GameTime gameTime, Game1 game)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Anims: fdown, fdownidle, fside, fsideidle, fup, fupidle, mdown, mdownidle, mside, msideidle, mup, mupidle

            if (game.screen.GetKeyState().IsKeyDown(Keys.Up))
            {
                keyPressed = true;
                position.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                lastKey = "up";
            }

            if (game.screen.GetKeyState().IsKeyDown(Keys.Down))
            {
                keyPressed = true;
                position.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                lastKey = "down";
            }

            if (game.screen.GetKeyState().IsKeyDown(Keys.Left))
            {
                keyPressed = true;
                position.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                lastKey = "left";
            }

            if (game.screen.GetKeyState().IsKeyDown(Keys.Right))
            {
                keyPressed = true;
                position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                lastKey = "right";
            }

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
    }
}
