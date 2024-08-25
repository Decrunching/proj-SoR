using Spine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoR.Logic.Entities;

namespace SoR.Logic.Spine
{
    /*
     * Used to create and update Spine skeletons, skins and animations.
     */
    public class SpineSetUp
    {
        private Entity entity;

        /*
         * Constructor for instantiating entities and Spine animations.
         */
        public SpineSetUp(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice, Entity playerChar)
        {
            // Instantiate the entity
            entity = playerChar;
        }
    }
}