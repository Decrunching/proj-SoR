using Microsoft.Xna.Framework;
using SoR.Logic.Entities;
using System;

namespace SoR.Logic
{
    /*
     * Placeholder class for handling game progression.
     */
    public class GameLogic
    {
        private Entity player;
        private Entity pheasant;
        private Entity chara;
        private Entity slime;
        private Entity campfire;
        private Entity entity;

        /*
         * Create player.
         */
        public Entity CreatePlayer(GraphicsDeviceManager _graphics)
        {
            player = new Player(_graphics);
            return player;
        }

        /*
         * Create pheasant.
         */
        public Entity CreatePheasant(GraphicsDeviceManager _graphics)
        {
            pheasant = new Pheasant(_graphics);
            return pheasant;
        }

        /*
         * Create chara.
         */
        public Entity CreateChara(GraphicsDeviceManager _graphics)
        {
            chara = new Chara(_graphics);
            return chara;
        }

        /*
         * Create slime.
         */
        public Entity CreateSlime(GraphicsDeviceManager _graphics)
        {
            slime = new Slime(_graphics);
            return slime;
        }

        /*
         * Create campfire.
         */
        public Entity CreateCampfire(GraphicsDeviceManager _graphics)
        {
            campfire = new Campfire(_graphics);
            return campfire;
        }

        /*
         * Placeholder game logic function
         */
        public Entity StartGame(GraphicsDeviceManager _graphics)
        {
            Random rand = new Random();
            switch (rand.Next(1, 5))
            {
                case 1:
                    entity = new Player(_graphics);
                    break;
                case 2:
                    entity = new Pheasant(_graphics);
                    break;
                case 3:
                    entity = new Chara(_graphics);
                    break;
                case 4:
                    entity = new Slime(_graphics);
                    break;
                case 5:
                    entity = new Campfire(_graphics);
                    break;
            }
            return entity;
        }
    }
}