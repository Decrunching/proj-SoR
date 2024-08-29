using System.Numerics;

namespace SoR.Logic.Input
{
    /*
     * Manage key functions.
     */
    internal class InputKeys
    {
        public bool Pressed { get; set; }
        public string NextAnimation { get; set; }

        public InputKeys(bool pressed, string nextAnimation)
        {
            Pressed = pressed;
            NextAnimation = nextAnimation;
        }
    }
}