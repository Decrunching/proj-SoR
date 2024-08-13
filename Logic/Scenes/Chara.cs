using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Spine;

namespace SoR.Logic.Scenes
{
    public abstract class Chara
    {
        private Game1 game;
        private SkeletonRenderer skeletonRenderer;
        private KeyboardState keyState;
        private KeyboardState lastKeyState;
        protected Vector2 position;
        private bool keyPressed;
        private bool enterPressed;
        private bool enterReleased;

        public Chara(Game1 game)
        {
            this.game = game;
            position = new Vector2(game.GetGraphics().PreferredBackBufferWidth / 2,
                game.GetGraphics().PreferredBackBufferHeight / 2);

            // Initialise skeleton renderer with premultiplied alpha
            skeletonRenderer = new SkeletonRenderer(game.GetGraphicsDevice());
            skeletonRenderer.PremultipliedAlpha = true;
        }

        public SkeletonRenderer GetSkeletonRenderer()
        {
            return skeletonRenderer;
        }

        public KeyboardState GetKeyState()
        {
            return keyState;
        }

        public void UpdateInput()
        {
            enterPressed = keyState.IsKeyDown(Keys.Enter);
            enterReleased = keyState.IsKeyUp(Keys.Enter);

            keyPressed = lastKeyState.Equals(keyState.IsKeyDown(Keys.Enter)) == enterPressed
                && keyState.Equals(keyState.IsKeyDown(Keys.Enter)) == enterReleased;

            lastKeyState = keyState;
        }
    }
}
