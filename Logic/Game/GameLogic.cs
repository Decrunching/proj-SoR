using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoR.Logic.Entities;
using SoR.Logic.Input;
using SoR.Logic.Spine;
using Spine;

namespace SoR.Logic.Game
{
    /*
     * Placeholder class for handling game progression.
     */
    public class GameLogic
    {
        private Entity playerChar;
        private Entity pheasant;
        private Entity chara;
        private Entity slime;
        private Entity campfire;
        private Entity entity;
        private SpineSetUp spineSetUp;
        private PlayerInput playerInput;
        private EntityType entityType;
        private bool gameStart;

        /*
         * Temporary Enums for differentiating between entities
         */
        enum EntityType
        {
            Player,
            NPCPheasant,
            NPCChara,
            EnemySlime,
            EnvironmentFire
        }

        /*
         * Constructor for initial game setup.
         */
        public GameLogic(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice)
        {
            gameStart = true;
            CreateEntity(graphics);
            spineSetUp = new SpineSetUp(graphics, GraphicsDevice, GetEntity()); // Instantiate Spine entities
            spineSetUp.CreateSkeletonRenderer(GraphicsDevice);  // Create the skeleton renderer
            playerInput = new PlayerInput(); // Instantiate the keyboard input
        }

        /*
         * Placeholder function for choosing which entity to create. Only use for permanent
         * entities - transient entities are fine being transient.
         */
        public void CreateEntity(GraphicsDeviceManager graphics)
        {
            EntityType entityType = EntityType.Player;

            switch (entityType)
            {
                case EntityType.Player:
                    playerChar = new PlayerChar(graphics);
                    break;
                case EntityType.NPCPheasant:
                    pheasant = new Pheasant(graphics);
                    break;
                case EntityType.NPCChara:
                    chara = new Chara(graphics);
                    break;
                case EntityType.EnemySlime:
                    slime = new Slime(graphics);
                    break;
                case EntityType.EnvironmentFire:
                    campfire = new Campfire(graphics);
                    break;
            }
        }

        /*
         * Get the appropriate entity.
         */
        public Entity GetEntity()
        {
            return playerChar;
        }

        /*
         * Update entity position according to player input.
         */
        public void UpdateEntityPosition(
            GameTime gameTime,
            KeyboardState keyState,
            GraphicsDeviceManager graphics,
            GraphicsDevice GraphicsDevice,
            AnimationState animState,
            Skeleton skeleton)
        {
            // Pass the speed, position and animation state to PlayerInput for keyboard input processing
            playerInput.ProcessKeyboardInputs(gameTime,
                keyState,
                animState,
                playerChar.GetSpeed(),
                playerChar.GetPositionX(),
                playerChar.GetPositionY());

            // Pass the speed to PlayerInput for joypad input processing
            playerInput.ProcessJoypadInputs(gameTime, playerChar.GetSpeed());

            // Set the new position according to player input
            playerChar.SetPositionX(playerInput.UpdatePositionX());
            playerChar.SetPositionY(playerInput.UpdatePositionY());

            // Prevent the user from leaving the visible screen area
            playerInput.CheckScreenEdges(graphics,
                GraphicsDevice,
                playerChar.GetPositionX(),
                playerChar.GetPositionY());

            // Set the new position according to player input
            playerChar.SetPositionX(playerInput.UpdatePositionX());
            playerChar.SetPositionY(playerInput.UpdatePositionY());
        }

        /*
         * Set up Spine animations and skeletons.
         */
        public void UpdatePlayerInput(
            GameTime gameTime,
            KeyboardState keyState,
            GraphicsDeviceManager graphics,
            GraphicsDevice GraphicsDevice)
        {
            // Update position according to user input
            UpdateEntityPosition(
                gameTime,
                keyState, graphics,
                GraphicsDevice,
                spineSetUp.GetAnimState(),
                spineSetUp.GetSkeleton());

            // Update animations
            spineSetUp.UpdateEntityAnimations(gameTime);
        }

        /*
         * Render Spine skeletons.
         */
        public void SpineRenderSkeleton(GraphicsDevice GraphicsDevice)
        {
            spineSetUp.RenderSkeleton(GraphicsDevice); // Render the skeleton to the screen
        }
    }
}