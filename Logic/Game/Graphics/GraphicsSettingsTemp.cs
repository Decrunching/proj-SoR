using Microsoft.Xna.Framework;
using SoR;

namespace Logic.Game.Graphics
{
    internal class GraphicsSettingsTemp
    {
         private bool isFullscreen;
         private bool isBorderless;
         private bool resolutionChange;
         private int screenWidth;
         private int screenHeight;
         private GameWindow Window;

         public GraphicsSettingsTemp(MainGame game, GraphicsDeviceManager graphics)
         {
             graphics.IsFullScreen = isFullscreen = false;
             isBorderless = false;
             Window.AllowAltF4 = true;
             Window.AllowUserResizing = false;
             game.IsMouseVisible = false;
             resolutionChange = false;
         }

         /*
          * The resolution has just been changed.
          */
         public bool ResolutionHasChanged()
         {
             return resolutionChange;
         }

         /*
          * Everything has been updated and the resolution change is complete.
          */
         public void ResolutionChangeFinished()
         {
             resolutionChange = false;
         }
    }
}
