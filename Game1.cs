using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoR.Logic.Entities;

namespace SoR
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private GraphicsDevice graphicsDevice;
        public Game1 game;
        private Player player;
        private KeyboardState keyState;
        private KeyboardState lastKeyState;
        private Vector2 position;
        public Chara chara;
        protected int screenWidth;
        protected int screenHeight;
        protected string lastKey;
        protected bool keyPressed;
        private float speed;
        private int deadZone;

        public Game1()
        {
            IsMouseVisible = true;
            game = this;
            _graphics = new GraphicsDeviceManager(game);
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            screenWidth = _graphics.PreferredBackBufferWidth;
            screenHeight = _graphics.PreferredBackBufferHeight;
            Content.RootDirectory = "Content";
            lastKey = "down";
            keyPressed = false;
            speed = 100f;
            deadZone = 4096;
        }

        public GraphicsDeviceManager GetGraphicsDeviceManager(Game1 game)
        {
            return game._graphics;
        }

        public GraphicsDevice GetGraphicsDevice(Game1 game)
        {
            graphicsDevice = game.GraphicsDevice;
            return game.graphicsDevice;
        }

        public void SetPositionY(float positionY)
        {
            position.Y += positionY;
        }

        public float GetPositionY()
        {
            return position.Y;
        }

        public float GetPositionX()
        {
            return position.X;
        }

        public KeyboardState GetKeyState()
        {
            return game.keyState;
        }

        public KeyboardState GetLastKeyState()
        {
            return game.lastKeyState;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            position = new Vector2(screenWidth / 2,
                screenHeight / 2);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            player = new Player(game);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            //Anims: fdown, fdownidle, fside, fsideidle, fup, fupidle, mdown, mdownidle, mside, msideidle, mup, mupidle

            keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Up))
            {
                //keyPressed = true;
                position.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                //lastKey = "up";
            }

            if (keyState.IsKeyDown(Keys.Down))
            {
                //keyPressed = true;
                position.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                //lastKey = "down";
            }

            if (keyState.IsKeyDown(Keys.Left))
            {
                //keyPressed = true;
                position.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                //lastKey = "left";
            }

            if (keyState.IsKeyDown(Keys.Right))
            {
                //keyPressed = true;
                position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                //lastKey = "right";
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

            player.UpdateAnimations();

            lastKeyState = keyState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here
            player.Render(gameTime, game);

            base.Draw(gameTime);
        }
    }
}
