using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoR.Logic.Game;

namespace SoR
{
    /*
     * The main game class, through which all other code runs.
     */
    public class SoR : Game
    {
        private GraphicsDeviceManager graphics;
        private KeyboardState keyState;
        private KeyboardState lastKeyState;
        private GameLogic gameLogic;

        /*
         * Constructor for the main game class. Initialises the graphics and mouse visibility.
         */
        public SoR()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 1200;
        }

        /*
         * Initialise the game.
         */
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /*
         * Load game content.
         */
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here

            gameLogic = new GameLogic(graphics, GraphicsDevice);
        }

        /*
         * Update game components.
         */
        protected override void Update(GameTime gameTime)
        {
            // If the back button on the game pad or the escape key on the keyboard are pressed, exit the game
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            keyState = Keyboard.GetState(); // Get the current keyboard state

            // Update player input and animations
            gameLogic.UpdateEntities(gameTime, keyState, lastKeyState, graphics, GraphicsDevice);

            lastKeyState = keyState; // Get the previous keyboard state

            base.Update(gameTime);
        }

        /*
         * Draw game components to the screen.
         */
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSeaGreen); // Clear the graphics buffer and set the window background colour to "dark sea green"
            gameLogic.SpineRenderSkeleton(GraphicsDevice);

            base.Draw(gameTime);
        }
    }
}