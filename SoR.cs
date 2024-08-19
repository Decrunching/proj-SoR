using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoR.Logic.Entities;
using Spine;
using System.Collections.Generic;
using System.Linq;


namespace SoR
{
    public class SoR : Game
    {
        private GraphicsDeviceManager _graphics;
        private KeyboardState keyState;
        private KeyboardState lastKeyState;
        private Vector2 position;
        private float speed;
        private int deadZone;
        private int screenWidth;
        private int screenHeight;

        protected SkeletonRenderer skeletonRenderer;

        private SoR game;
        private Player player;

        public SoR()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            screenWidth = _graphics.PreferredBackBufferWidth;
            screenHeight = _graphics.PreferredBackBufferHeight;
            speed = 300f;
            deadZone = 4096;
            game = this;
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
            // Initialise skeleton renderer with premultiplied alpha
            skeletonRenderer = new SkeletonRenderer(GraphicsDevice);
            skeletonRenderer.PremultipliedAlpha = true;

            player = new Player(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here//Anims: fdown, fdownidle, fside, fsideidle, fup, fupidle, mdown, mdownidle, mside, msideidle, mup, mupidle

            keyState = Keyboard.GetState();

            Keys[] keysPressed = keyState.GetPressedKeys();
            Keys[] lastKeysPressed = new Keys[0];

            bool up = true;

            Dictionary<Keys, bool> keyIsUp =
                new Dictionary<Keys, bool>()
                {
                    { Keys.Up, up },
                    { Keys.Down, up },
                    { Keys.Left, up },
                    { Keys.Right, up }
                };
            
            foreach (Keys pressed in keysPressed)
            {
                foreach (Keys lastPressed in lastKeysPressed)
                {
                    if (pressed == lastPressed)
                    {
                        up = false;
                        keyIsUp[pressed] = up;
                    }
                }
            }

            foreach (Keys key in keyIsUp.Keys)
            {
                foreach (Keys pressed in keysPressed)
                {
                    player.SetAnimRunning(keyState, lastKeyState);

                    if (key == Keys.Up & key == pressed)
                    {
                        position.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    if (key == Keys.Down & key == pressed)
                    {
                        position.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    if (key == Keys.Left & key == pressed)
                    {
                        position.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    if (key == Keys.Right & key == pressed)
                    {
                        position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }

                    if (key != pressed & lastKeyState.IsKeyDown(key))
                    {
                        player.ChangeRunDirection(keyState);
                    }
                }
            }

            if (!keyState.IsKeyDown(Keys.Up) &&
                !keyState.IsKeyDown(Keys.Down) &&
                !keyState.IsKeyDown(Keys.Left) &&
                !keyState.IsKeyDown(Keys.Right))
            {
                player.SetIdle();
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

            lastKeyState = keyState;
            lastKeysPressed = keysPressed;

            player.UpdateSkeletalAnimations(gameTime, position);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSeaGreen);

            // TODO: Add your drawing code here
            ((BasicEffect)skeletonRenderer.Effect).Projection = Matrix.CreateOrthographicOffCenter(
            0,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height,
                0, 1, 0);

            // Draw skeletons
            skeletonRenderer.Begin();
            skeletonRenderer.Draw(player.GetSkeleton());
            skeletonRenderer.End();

            base.Draw(gameTime);
        }
    }
}