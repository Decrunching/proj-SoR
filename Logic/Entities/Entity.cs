using Microsoft.Xna.Framework;

namespace SoR.Logic.Entities
{
    internal interface Entity
    {
        /*
         * Get the Atlas path.
         */
        string GetAtlas();

        /*
         * Get the json path.
         */
        string GetJson();

        /*
         * Get the starting skin.
         */
        string GetSkin();

        /*
         * Get the starting animation.
         */
        string GetStartingAnim();

        /*
         * Get the current travel speed.
         */
        float GetSpeed();

        /*
         * Get the current x-axis position.
         */
        float GetPositionX();

        /*
         * Get the current y-axis position.
         */
        float GetPositionY();

        /*
         * Set the current x-axis position.
         */
        void SetPositionX(float positionX);

        /*
         * Set the current y-axis position.
         */
        void SetPositionY(float positionY);
    }
}