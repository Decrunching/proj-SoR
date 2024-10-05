namespace Hardware.Input
{
    /*
     * Manage key functions.
     */
    public class InputKeys
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