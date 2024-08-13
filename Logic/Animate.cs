using Microsoft.Xna.Framework;
using Spine;

namespace SoR.Logic
{
    internal class Animate : Input
    {
        public Animate(Game1 game) : base(game)
        {
        }

        public void UpdateAnimations(GameTime gameTime, Animate animate)
        {
            //Anims: fdown, fdownidle, fside, fsideidle, fup, fupidle, mdown, mdownidle, mside, msideidle, mup, mupidle
            if (!keyPressed)
            {
                switch (lastKey)
                {
                    case "up":
                        animate.animState.SetAnimation(0, "fupidle", true);
                        break;
                    case "down":
                        animate.animState.SetAnimation(0, "fdownidle", true);
                        break;
                    case "left":
                        skeleton.ScaleX = -1;
                        animate.animState.SetAnimation(0, "fsideidle", true);
                        break;
                    case "right":
                        skeleton.ScaleX = 1;
                        animate.animState.SetAnimation(0, "fsideidle", true);
                        break;
                }
            }
            else if (keyPressed)
            {
                switch (lastKey)
                {
                    case "up":
                        animate.animState.AddAnimation(0, "fup", true, 0);
                        break;
                    case "down":
                        animate.animState.AddAnimation(0, "fdown", true, 0);
                        break;
                    case "left":
                        animate.animState.AddAnimation(0, "fside", true, 0);
                        skeleton.ScaleX = -1;
                        break;
                    case "right":
                        animate.animState.AddAnimation(0, "fside", true, 0);
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

            // Update animation state and apply animations to the skeleton
            skeleton.X = position.X;
            skeleton.Y = position.Y;
            animState.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            skeleton.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            animState.Apply(skeleton);

            // Update skeletal transformations
            skeleton.UpdateWorldTransform(Skeleton.Physics.Update);
        }
    }
}
