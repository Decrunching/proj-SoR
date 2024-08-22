using SoR.Logic.Entities;

namespace SoR.Logic
{
    /*
     * Placeholder class for handling game progression.
     */
    internal class GameLogic
    {
        private Entity player;
        
        public Entity GetEntity()
        {
            player = new Player();
            return player;
        }
    }
}