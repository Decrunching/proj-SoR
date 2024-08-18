using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SoR.Logic.Entities;

namespace SoR.Logic.Input
{
    public interface KeyboardInput
    {
        public void GetKeyInput(KeyboardState keyState, KeyboardState lastKeyState)
        {
            if (keyState.IsKeyDown(Keys.Up))
            {
                //keyPressed = true;
                position.Y -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                SetRunAnim(keyState, lastKeyState);
                //lastKey = "up";
            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                //keyPressed = true;
                position.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                SetRunAnim(keyState, lastKeyState);
                //lastKey = "down";
            }
            if (keyState.IsKeyDown(Keys.Left))
            {
                //keyPressed = true;
                position.X -= speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                SetRunAnim(keyState, lastKeyState);
                //lastKey = "left";
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                //keyPressed = true;
                position.X += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                SetRunAnim(keyState, lastKeyState);
                //lastKey = "right";
            }
            if (!keyState.IsKeyDown(Keys.Up) &&
                !keyState.IsKeyDown(Keys.Down) &&
                !keyState.IsKeyDown(Keys.Left) &&
                !keyState.IsKeyDown(Keys.Right))
            {
                SetIdle();
            }
        }
    }
}
