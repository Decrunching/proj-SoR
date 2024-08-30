using System.Numerics;

namespace SoR.Logic.Input
{
    /*
     * Manage key functions.
     */
    internal class InputKeys
    {
        public bool Pressed { get; set; }
        public string Direction { get; set; }

        public InputKeys(bool pressed, string direction)
        {
            Pressed = pressed;
            Direction = direction;
        }
    }
}