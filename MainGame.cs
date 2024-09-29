using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Logic.Game.Graphics;
using Logic.Game.Screens;

namespace SoR
{
    /*
     * The main game class, through which all other code runs. This project utilises Spine and Monogame.
     */

    /**************************************************************************************************************************
     * Copyright (c) 2024, Katherine Town
     * 
     * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the
     * following conditions are met:
     * 
     * 1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following
     * disclaimer.
     * 
     * 2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following
     * disclaimer in the documentation and/or other materials provided with the distribution.
     * 
     * 3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote products
     * derived from this software without specific prior written permission.
     * 
     * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS “AS IS” AND ANY EXPRESS OR IMPLIED WARRANTIES,
     * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
     * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
     * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
     * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
     * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
     * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
     * 
     * (The 3-Clause BSD License: https://opensource.org/license/BSD-3-Clause)
     **************************************************************************************************************************/

    public class MainGame : Game
    {
        private GraphicsDeviceManager graphics;
        private GameLogic stage;
        private MainGame game;
        private GraphicsSettings graphicsSettings;
        private int screenWidth;
        private int screenHeight;

        /*
         * Constructor for the main game class. Initialises the graphics and mouse visibility.
         */
        public MainGame()
        {
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);
        }

        /*
         * Initialise the game.
         */
        protected override void Initialize()
        {
            game = this;

            graphicsSettings = new GraphicsSettings(game, graphics, Window);

            stage = new GameLogic(GraphicsDevice, Window);

            base.Initialize();
        }

        /*
         * Load game content.
         */
        protected override void LoadContent()
        {
            stage.LoadGameContent(GraphicsDevice, game);
        }

        /*
         * Update game components.
         */
        protected override void Update(GameTime gameTime)
        {
            // If the back button on the game pad or the escape key on the keyboard are pressed, exit the game
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            UpdateResolution(graphicsSettings.CheckIfBorderlessToggled(graphics, Window));

            stage.UpdateWorld(gameTime, graphics);

            base.Update(gameTime);
        }

        /*
         * Get the updated screen resolution if it changes.
         */
        public void UpdateResolution(Vector2 resolution)
        {
            if (screenWidth != (int)resolution.X |
                screenHeight != (int)resolution.Y)
            {
                screenWidth = (int)resolution.X;
                screenHeight = (int)resolution.Y;

                stage.UpdateViewportGraphics(Window, screenWidth, screenHeight);

                graphics.ApplyChanges();
            }
        }

        /*
         * Draw game components to the screen.
         */
        protected override void Draw(GameTime gameTime)
        {
            stage.Render(GraphicsDevice);

            base.Draw(gameTime);
        }
    }
}