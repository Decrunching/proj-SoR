using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace SoR.Logic
{
    public abstract class Input : Game1
    {
        private KeyboardState lastKeyState;
        protected Vector2 position;
        protected bool keyPressed;
        private bool enterPressed;
        private bool enterReleased;
        private float speed;
        private int deadZone;
        protected string lastKey;

        public Input(Game1 game)
        {
            enterPressed = game.GetKeyState().IsKeyDown(Keys.Enter);
            enterReleased = game.GetKeyState().IsKeyUp(Keys.Enter);
            lastKey = "down";
            keyPressed = false;
            speed = 400f;
            deadZone = 4096;
            position = new Vector2(GetGraphicsDeviceManager(game).PreferredBackBufferWidth / 2,
                GetGraphicsDeviceManager(game).PreferredBackBufferHeight / 2);
        }

        public void GetUserInput(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Anims: fdown, fdownidle, fside, fsideidle, fup, fupidle, mdown, mdownidle, mside, msideidle, mup, mupidle

            if (game.GetKeyState().IsKeyDown(Keys.Up))
            {
                keyPressed = true;
                position.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                lastKey = "up";
            }

            if (game.GetKeyState().IsKeyDown(Keys.Down))
            {
                keyPressed = true;
                position.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                lastKey = "down";
            }

            if (game.GetKeyState().IsKeyDown(Keys.Left))
            {
                keyPressed = true;
                position.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                lastKey = "left";
            }

            if (game.GetKeyState().IsKeyDown(Keys.Right))
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

            lastKeyState = game.GetKeyState();
        }

        public void UpdateInput()
        {
            keyPressed = lastKeyState.Equals(game.GetKeyState().IsKeyDown(Keys.Enter)) == enterPressed
                && game.GetKeyState().Equals(game.GetKeyState().IsKeyDown(Keys.Enter)) == enterReleased;

            lastKeyState = game.GetKeyState();
        }
    }
}