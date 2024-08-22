namespace SoR.Logic.Entities
{
    internal interface Entity
    {
        /*
         * Get the entity Atlas path.
         */
        string GetAtlas();

        /*
         * Get the entity json path.
         */
        string GetJson();

        /*
         * Get the entity starting skin.
         */
        string GetSkin();

        /*
         * Get the entity starting animation.
         */
        string GetStartingAnim();
    }
}