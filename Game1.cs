using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoR.Logic;
using SoR.Logic.Scenes;

namespace SoR
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        public Chara screen;
        public Game1 game;
        private Animate animate;
        private Player chara;
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

        public Chara GetScreen()
        {
            return screen;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            chara = new Player(game);
            input = new Input(game);
            animate = new Animate(game);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            screen = new MainScreen(this);
            chara.CreateSkeleton(game);
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            input.GetUserInput(gameTime, game);
            animate.UpdateAnimations(gameTime, animate);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            ((BasicEffect)chara.GetSkeletonRenderer().Effect).Projection = Matrix.CreateOrthographicOffCenter(
                0,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height,
                0, 1, 0);

            // TODO: Add your drawing code here
            chara.DrawSkeletons();

            base.Draw(gameTime);
        }
    }
}
