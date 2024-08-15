using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoR.Logic.Entities;
using Spine;


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

            if (keyState.IsKeyDown(Keys.Up))
            {
                //keyPressed = true;
                position.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                player.SetAnimState(keyState, lastKeyState);
                //lastKey = "up";
            }

            if (keyState.IsKeyDown(Keys.Down))
            {
                //keyPressed = true;
                position.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                player.SetAnimState(keyState, lastKeyState);
                //lastKey = "down";
            }

            if (keyState.IsKeyDown(Keys.Left))
            {
                //keyPressed = true;
                position.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                player.SetAnimState(keyState, lastKeyState);
                //lastKey = "left";
            }

            if (keyState.IsKeyDown(Keys.Right))
            {
                //keyPressed = true;
                position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                player.SetAnimState(keyState, lastKeyState);
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

            lastKeyState = keyState;

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