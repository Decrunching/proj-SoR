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
            player = new Player(_graphics, GraphicsDevice); // Create the player object
            player.CreateSkeletonRenderer(GraphicsDevice); // Create the skeleton renderer
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

            player.SetAnimRunning(gameTime, keyState); // Set the running animation according to current keyboard input
            player.UpdateSkeletalAnimations(gameTime); // Update the player skeleton to apply animations and movement

            base.Update(gameTime);
        }

        /*
         * Draw game components to the screen.
         */
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSeaGreen); // Clear the graphics buffer and set the window background colour to "dark sea green"

            // TODO: Add your drawing code here
            player.RenderSkeleton(GraphicsDevice); // Render the skeleton to the screen

            base.Draw(gameTime);
        }
    }
}