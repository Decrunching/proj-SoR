using Logic.Entities.Character;
using Logic.Entities.Character.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoR;

namespace Logic.Game.Screens
{
    /*
     * Placeholder class for game stages. Take components from GameLogic to set the initial state of the game, then manage progression.
     */
    public partial class Screens
    {
        private GameLogic gameLogic;
        private Location location;

        /*
         * Differentiate between environmental ojects.
         */
        enum Location
        {
            Village,
            Temple
        }

        public Screens(GraphicsDevice GraphicsDevice, GameWindow Window)
        {
            gameLogic = new GameLogic(GraphicsDevice, Window);
        }

        /*
         * Currently the same every time the game is started, but will be updated later when file saving implemented.
         */
        public void LoadGameState(MainGame game, GraphicsDevice GraphicsDevice)
        {
            gameLogic.Village(game, GraphicsDevice, true);

            switch (location)
            {
                case Location.Village:
                    foreach (var entity in gameLogic.Entities.Values)
                    {
                        if (gameLogic.Entities.TryGetValue("player", out Entity pheasantChar))
                        {
                            if (pheasantChar is Player pheasant)
                            {
                                if (pheasant.GetHitPoints() <= 0)
                                {
                                    gameLogic.Village(game, GraphicsDevice, false);
                                    gameLogic.Temple(game, GraphicsDevice, true);
                                }
                            }
                        }
                    }
                    break;
            }
        }
    }
}