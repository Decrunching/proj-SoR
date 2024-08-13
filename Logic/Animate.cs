using Microsoft.Xna.Framework;
using Spine;

namespace SoR.Logic
{
    internal class Animate : Input
    {
        public Animate(Game1 game) : base(game) { }

        public void UpdateAnimations(GameTime gameTime, Animate animate)
        {
            //Anims: fdown, fdownidle, fside, fsideidle, fup, fupidle, mdown, mdownidle, mside, msideidle, mup, mupidle
            if (!animate.keyPressed)
            {
                switch (lastKey)
                {
                    case "up":
                        animState.SetAnimation(0, "fupidle", true);
                        break;
                    case "down":
                        animState.SetAnimation(0, "fdownidle", true);
                        break;
                    case "left":
                        skeleton.ScaleX = -1;
                        animState.SetAnimation(0, "fsideidle", true);
                        break;
                    case "right":
                        skeleton.ScaleX = 1;
                        animState.SetAnimation(0, "fsideidle", true);
                        break;
                }
            }
            else if (animate.keyPressed)
            {
                switch (lastKey)
                {
                    case "up":
                        animState.AddAnimation(0, "fup", true, 0);
                        break;
                    case "down":
                        animState.AddAnimation(0, "fdown", true, 0);
                        break;
                    case "left":
                        animState.AddAnimation(0, "fside", true, 0);
                        skeleton.ScaleX = -1;
                        break;
                    case "right":
                        animState.AddAnimation(0, "fside", true, 0);
                        skeleton.ScaleX = 1;
                        break;
                }
            }

            if (position.X > screenWidth - skeletonData.Width)
            {
                position.X = screenWidth - skeletonData.Width;
            }
            else if (position.X < skeletonData.Width)
            {
                position.X = skeletonData.Width;
            }

            if (position.Y > screenHeight + skeletonData.Height / 3)
            {
                position.Y = screenHeight + skeletonData.Height / 3;
            }
            else if (position.Y < skeletonData.Height * 3)
            {
                position.Y = skeletonData.Height * 3;
            }
        }
    }
}
