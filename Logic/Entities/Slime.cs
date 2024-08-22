namespace SoR.Logic.Entities
{
    internal class Slime
    {
        /*
         * Get the entity Atlas path.
         */
        public string GetAtlas()
        {
            return "F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Slime\\Slime.atlas";
        }

        /*
         * Get the entity json path.
         */
        public string GetJson()
        {
            return "F:\\MonoGame\\SoR\\SoR\\Content\\Entities\\Slime\\skeleton.json";
        }

        /*
         * Get the entity starting skin.
         */
        public string GetSkin()
        {
            return "default";
        }

        /*
         * Get the entity starting animation.
         */
        public string GetStartingAnim()
        {
            return "idle";
        }
    }
}
