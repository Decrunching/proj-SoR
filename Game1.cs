using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoR.Logic;
using SoR.Logic.Entities;

namespace SoR
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        public Chara chara;
        public Game1 game;
        private Animate animate;
        private Player player;
        private Input input;
        protected int screenWidth;
        protected int screenHeight;

        public Game1()
        {
            IsMouseVisible = true;
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = false;
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            screenWidth = _graphics.PreferredBackBufferWidth;
            screenHeight = _graphics.PreferredBackBufferHeight;

            game = this;

            Content.RootDirectory = "Content";
        }

        public GraphicsDeviceManager GetGraphics()
        {
            return game._graphics;
        }

        public GraphicsDevice GetGraphicsDevice()
        {
            return GraphicsDevice;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            input = new Input(this);
            animate = new Animate(this);

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
            input.UpdateInput();
            input.GetUserInput(gameTime, player);
            animate.UpdateAnimations(gameTime, animate);

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
