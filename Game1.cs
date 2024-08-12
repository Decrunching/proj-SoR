using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoR.Logic;
using SoR.Logic.Scenes;
using Spine;

namespace SoR
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        public Screen screen;
        public Game1 game;
        private Player player;
        private Input input;
        private Animate animate;
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

        public Screen GetScreen()
        {
            return screen;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player = new Player(game);
            input = new Input(game);
            animate = new Animate(game);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            screen = new MainScreen(this);
            player.SetupSkeleton();
            animate.SetupAnimation();
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            input.GetUserInput(gameTime);
            animate.UpdateAnimations(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            ((BasicEffect)player.GetSkeletonRenderer().Effect).Projection = Matrix.CreateOrthographicOffCenter(
                0,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height,
                0, 1, 0);

            // TODO: Add your drawing code here
            player.DrawSkeletons();

            base.Draw(gameTime);
        }
    }
}
