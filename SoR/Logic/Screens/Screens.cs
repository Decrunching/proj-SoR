using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoR.Logic.Character;

namespace SoR.Logic.Screens
{
    /*
     * Placeholder class for game stages. Take components from GameLogic to set the initial state of the game, then manage progression.
     */
    public partial class Screens
    {
        private GameLogic gameLogic;

        public Screens(MainGame game, GraphicsDevice GraphicsDevice, GraphicsDeviceManager graphics)
        {
            gameLogic = new GameLogic(game, GraphicsDevice);
        }

        /*
         * Update the state of the game.
         */
        public void UpdateGameState(GameTime gameTime, MainGame game, GraphicsDevice GraphicsDevice, GraphicsDeviceManager graphics)
        {
            switch (gameLogic.CurrentMapString)
            {
                case "mainMenu":
                    gameLogic.GameMainMenu(game, GraphicsDevice, graphics);
                    gameLogic.CurrentMapString = "none";
                    break;
                case "none":
                    if (gameLogic.CurrentInputScreen == "game")
                    {
                        gameLogic.UpdateWorld(gameTime, graphics);

                        foreach (var entity in gameLogic.Entities.Values)
                        {
                            if (gameLogic.Entities.TryGetValue("chara", out Entity chara))
                            {
                                if (chara.GetHitPoints() <= 98)
                                {
                                    gameLogic.Temple(game, GraphicsDevice);
                                }
                            }
                        }
                    }
                    break;
                case "village":
                    gameLogic.Village(game, GraphicsDevice);
                    gameLogic.CurrentMapString = "none";
                    break;
                case "temple":
                    gameLogic.Temple(game, GraphicsDevice);
                    gameLogic.CurrentMapString = "none";
                    break;
            }
            gameLogic.SaveLoadInput(game, gameTime, GraphicsDevice);
        }
    }
}