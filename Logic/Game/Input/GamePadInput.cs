using MonoGame.Extended.Input.InputListeners;

namespace Logic.Game.Input
{
    public class GamePadInput
    {
        private GamePadListener gamePadListener;

        public GamePadInput()
        {
            gamePadListener = new GamePadListener();

        }
    }
}
