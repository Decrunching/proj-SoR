using SoR.Hardware.Input;

namespace SoR.Logic.Character.Player
{
    internal partial class Player : Entity
    {
        protected GamePadInput gamePadInput;
        protected KeyboardInput keyboardInput;
        protected string lastAnimation;
        protected string movementAnimation;

        /*
         * Check whether the skin has changed.
         */
        public void SwitchSkin(string switchSkin)
        {
            if (switchSkin == "B" || switchSkin == "Space")
            {
                switch (Skin)
                {
                    case "solarknight-0":
                        skeleton.SetSkin(skeletonData.FindSkin("lunarknight-0"));
                        Skin = "lunarknight-0";
                        break;
                    case "lunarknight-0":
                        skeleton.SetSkin(skeletonData.FindSkin("knight-0"));
                        Skin = "knight-0";
                        break;
                    case "knight-0":
                        skeleton.SetSkin(skeletonData.FindSkin("solarknight-0"));
                        Skin = "solarknight-0";
                        break;
                }
            }
        }

        /*
         * Process keyboard and gamepad x-axis movement inputs.
         */
        public void ProcessXMovementInput(int x)
        {
            switch (x)
            {
                case 0:
                    if (CountDistance == 0)
                    {
                        direction.X = 0;
                    }
                    break;
                case 1:
                    MovementDirectionX(-1);
                    idle = false;
                    movementAnimation = "runleft";
                    break;
                case 2:
                    MovementDirectionX(1);
                    idle = false;
                    movementAnimation = "runright";
                    break;
                case 3:
                    direction.X = 0;
                    movementAnimation = lastAnimation;
                    break;
                case 4:
                    direction.X = 0;
                    movementAnimation = "idlebattle";
                    break;
            }
        }

        /*
         * Process keyboard and gamepad y-axis movement inputs.
         */
        public void ProcessYMovementInput(int y)
        {
            switch (y)
            {
                case 0:
                    if (CountDistance == 0)
                    {
                        direction.Y = 0;
                    }
                    break;
                case 1:
                    MovementDirectionY(-1);
                    movementAnimation = "runup";
                    idle = false;
                    break;
                case 2:
                    MovementDirectionY(1);
                    movementAnimation = "rundown";
                    idle = false;
                    break;
                case 3:
                    direction.Y = 0;
                    movementAnimation = lastAnimation;
                    break;
                case 4:
                    direction.Y = 0;
                    movementAnimation = "idlebattle";
                    break;
                case 5:
                    direction.Y = 0;
                    break;
            }
        }
    }
}