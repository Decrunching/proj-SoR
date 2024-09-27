using Logic.Game.GameMap.TiledScenery;
using Microsoft.Xna.Framework.Graphics;
using SoR.Logic.Game;
using Microsoft.Xna.Framework;

namespace Logic.Game.Screens
{
    /*
     * Placeholder class for game stages. Should decide which map tiles and layout
     * to use, which mobs/NPCs to load and where, and should adjust according to
     * game progression within a single function for the area of the game being
     * loaded that takes input from elsewhere about the current state of the game.
     */
    internal class Stage
    {
        private Map map;
        private Interactions interactions;

        public Stage (GraphicsDevice GraphicsDevice, GameWindow Window)
        {
            interactions = new Interactions(GraphicsDevice, Window);
        }

        /*
         * 
         */
        public void Village(GraphicsDevice GraphicsDevice)
        {
            // Get the map to be used
            map = new Map(1);

            // Create entities
            entityType = EntityType.Player;
            interactions.CreateEntity(GraphicsDevice);

            entityType = EntityType.Chara;
            interactions.CreateEntity(GraphicsDevice);

            entityType = EntityType.Pheasant;
            interactions.CreateEntity(GraphicsDevice);
        }

        /*
         * 
         */
        public void Temple(GraphicsDevice GraphicsDevice)
        {
            // Get the map to be used
            map = new Map(0);

            // Create entities
            entityType = EntityType.Player;
            interactions.CreateEntity(GraphicsDevice);

            entityType = EntityType.Fishy;
            interactions.CreateEntity(GraphicsDevice);

            entityType = EntityType.Slime;
            interactions.CreateEntity(GraphicsDevice);

            // Create scenery
            sceneryType = SceneryType.Campfire;
            interactions.CreateObject(GraphicsDevice);
        }
    }
}