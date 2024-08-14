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
        public Chara chara;
        protected int screenWidth;
        protected int screenHeight;

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

        public KeyboardState GetKeyState()
        {
            return keyState;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            keyState = Keyboard.GetState();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            player = new Player(this);
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            player.GetUserInput(gameTime);
            player.UpdateAnimations(gameTime, game);

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
