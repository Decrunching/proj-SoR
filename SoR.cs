using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoR.Logic.Entities;

namespace SoR
{/*
 * The main game class, through which all other code runs. Graphics are currently handled here,
 * but everything else is called from and handled by other classes.
 */
    public class SoR : Game
    {
        private GraphicsDeviceManager _graphics;
        private KeyboardState keyState; // Gets the current keyboard state

        private Player player; // Creates a player instance

        /*
         * Constructor for the main game class. Initialises the graphics and mouse visibility.
         */
        public SoR()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 1400;
            _graphics.PreferredBackBufferHeight = 900;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            player = new Player(_graphics, GraphicsDevice);
            player.CreateSkeletonRenderer(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            keyState = Keyboard.GetState();

            player.SetAnimRunning(gameTime, keyState);
            player.UpdateSkeletalAnimations(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSeaGreen);

            // TODO: Add your drawing code here
            player.RenderSkeleton(GraphicsDevice);

            base.Draw(gameTime);
        }
    }
}