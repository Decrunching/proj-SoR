using Microsoft.Xna.Framework.Input;
using Spine;

namespace SoR.Logic.Scenes
{
    public abstract class Screen
    {
        private Game1 game;
        private SkeletonRenderer skeletonRenderer;
        private KeyboardState keyState;
        private KeyboardState lastKeyState;
        private bool keyPressed;
        private bool enterPressed;
        private bool enterReleased;

        public Screen(Game1 game)
        {
            this.game = game;
            skeletonRenderer = new SkeletonRenderer(game.GraphicsDevice);
            skeletonRenderer.PremultipliedAlpha = true;
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
