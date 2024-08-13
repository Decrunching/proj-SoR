using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoR.Logic.Entities;

namespace SoR
{
    public class Game1 : Game
    {
        public Game1 game;
        private Gfx gfx;
        private Player player;
        public Chara chara;

        public Game1()
        {
            IsMouseVisible = true;
            game = this;
            gfx = new Gfx(game);
            Content.RootDirectory = "Content";
        }

        public GraphicsDeviceManager GetGraphicsDeviceManager()
        {
            return gfx.GetGraphicsDeviceManagerGfx();
        }

        public GraphicsDevice GetGraphicsDevice()
        {
            return gfx.GetGraphicsDeviceGfx();
        }

        public int GetScreenWidth()
        {
            return gfx.GetScreenWidthGfx();
        }

        public int GetScreenHeight()
        {
            return gfx.GetScreenHeightGfx();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

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
