using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json;
using System.IO;
using Apos.Input;
using SoR;
using FontStashSharp;

namespace Logic.Game.Settings
{
    /*
     * INCOMPLETE: Manage graphics settings. Currently switches between borderless and windowed mode with F4, and
     * resets to windowed (default) with F1.
     * 
     * TO DO:
     * Figure out why windowed resolution can't be changed either here or in Settings or both without
     * messing up resolution when switching between borderless and windowed.
     */
    public class GraphicsSettings : Settings
    {
        private bool resolutionChange;
        private bool fixedTimeStep;
        private FontSystem fontSystem;

        public GraphicsSettings(MainGame game, GraphicsDeviceManager graphics, GameWindow Window, Settings settings)
        {
            game.IsMouseVisible = false;
            resolutionChange = false;

            settings = EnsureJson("Settings.json", SettingsContext.Default.Settings);

            RestoreWindow(graphics, Window, settings);
        }
        ICondition toggleBorderless = new KeyboardCondition(Keys.F4);
        ICondition resetSettings = new KeyboardCondition(Keys.F1);

        public static string GetPath(string name) => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name);

        public static T LoadJson<T>(string name, JsonTypeInfo<T> typeInfo) where T : new()
        {
            T json;
            string jsonPath = GetPath(name);

            if (File.Exists(jsonPath))
            {
                json = JsonSerializer.Deserialize(File.ReadAllText(jsonPath), typeInfo)!;
            }
            else
            {
                json = new T();
            }

            return json;
        }

        public static void SaveJson<T>(string name, T json, JsonTypeInfo<T> typeInfo)
        {
            string jsonPath = GetPath(name);
            Directory.CreateDirectory(Path.GetDirectoryName(jsonPath)!);
            string jsonString = JsonSerializer.Serialize(json, typeInfo);
            File.WriteAllText(jsonPath, jsonString);
        }

        public static T EnsureJson<T>(string name, JsonTypeInfo<T> typeInfo) where T : new()
        {
            T json;
            string jsonPath = GetPath(name);

            if (File.Exists(jsonPath))
            {
                json = JsonSerializer.Deserialize(File.ReadAllText(jsonPath), typeInfo)!;
            }
            else
            {
                json = new T();
                string jsonString = JsonSerializer.Serialize(json, typeInfo);
                Directory.CreateDirectory(Path.GetDirectoryName(jsonPath)!);
                File.WriteAllText(jsonPath, jsonString);
            }

            return json;
        }

        /*
         * Initialise the game settings.
         */
        public void InitialiseSettings(GraphicsDeviceManager graphics, Settings settings, GameWindow Window)
        {
            Window.AllowUserResizing = false;
            Window.AllowAltF4 = true;
            //settings.IsVSync = true;

            fixedTimeStep = settings.IsFixedTimeStep;
            graphics.SynchronizeWithVerticalRetrace = settings.IsVSync;

            settings.IsFullscreen = settings.IsFullscreen || settings.IsBorderless;

            RestoreWindow(graphics, Window, settings);
            if (settings.IsFullscreen)
            {
                ChangeFullscreen(graphics, Window, false, settings);
            }
        }

        /*
         * Setup InputHelper for Apos library and FontSystem.
         */
        public void LoadSettings(MainGame game, ContentManager Content)
        {
            InputHelper.Setup(game); // For Apos library
            fontSystem = new FontSystem();
            fontSystem.AddFont(TitleContainer.OpenStream($"{Content.RootDirectory}/Fonts/LiberationMono-Regular.ttf"));
        }

        /*
         * Unload settings content.
         */
        public void UnloadSettings(GameWindow Window, Settings settings)
        {
            if (!settings.IsFullscreen)
            {
                SaveWindow(Window, settings);
            }

            SaveJson("Settings.json", settings, SettingsContext.Default.Settings);
        }

        /*
         * Update the game settings.
         */
        public void UpdateSettings(GraphicsDeviceManager graphics, GameWindow Window, Settings settings)
        {
            InputHelper.UpdateSetup();

            if (toggleBorderless.Pressed())
            {
                ToggleBorderlessMode(graphics, Window, settings);
            }

            if (resetSettings.Pressed())
            {
                bool fullscreenLast = settings.IsFullscreen;
                settings = new Settings();
                SaveJson("Settings.json", settings, SettingsContext.Default.Settings);

                ChangeFullscreen(graphics, Window, fullscreenLast, settings);

                InputHelper.UpdateCleanup();
            }
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


        /*
         * Render settings choices.
         */
        public void DrawSettings(Settings settings)
        {
            var font = fontSystem.GetFont(24);
            string mode = settings.IsBorderless ? "Borderless" : settings.IsFullscreen ? "Fullscreen" : "Window";
            Vector2 modeCenter = font.MeasureString(mode) / 2f;
        }

        /*
         * Turn off borderless mode or toggle fullscreen state.
         */
        public void ToggleFullscreenMode(GraphicsDeviceManager graphics, GameWindow Window, Settings settings)
        {
            bool fullscreenLast = settings.IsFullscreen;

            if (settings.IsBorderless)
            {
                settings.IsBorderless = false;
            }
            else
            {
                settings.IsFullscreen = !settings.IsFullscreen;
            }

            ChangeFullscreen(graphics, Window, fullscreenLast, settings);
        }

        /*
         * Toggle borderless state and set fullscreen state to the same.
         */
        public void ToggleBorderlessMode(GraphicsDeviceManager graphics, GameWindow Window, Settings settings)
        {
            bool fullscreenLast = settings.IsFullscreen;

            settings.IsBorderless = !settings.IsBorderless;
            settings.IsFullscreen = settings.IsBorderless;

            ChangeFullscreen(graphics, Window, fullscreenLast, settings);
        }

        /*
         * Set or remove fullscreen mode.
         */
        public void ChangeFullscreen(GraphicsDeviceManager graphics, GameWindow Window, bool fullscreenLast, Settings settings)
        {
            resolutionChange = true;

            if (settings.IsFullscreen)
            {
                if (fullscreenLast)
                {
                    ApplyHardwareMode(graphics, settings);
                }
                else
                {
                    SetFullscreen(graphics, Window, settings);
                }
            }
            else
            {
                RemoveFullscreen(graphics, Window, settings);
            }
        }

        /*
         * Called when already in fullscreen. When hardware mode switch set to false, will be borderless.
         */
        public void ApplyHardwareMode(GraphicsDeviceManager graphics, Settings settings)
        {
            graphics.HardwareModeSwitch = !settings.IsBorderless;
            graphics.ApplyChanges();
        }

        /*
         * Set screen width and height to the size of the monitor, then switch to fullscreen mode.
         */
        public void SetFullscreen(GraphicsDeviceManager graphics, GameWindow Window, Settings settings)
        {
            SaveWindow(Window, settings);

            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.HardwareModeSwitch = !settings.IsBorderless;

            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
        }

        /*
         * Restore window dimensions to their previous state, and remove fullscreen mode.
         */
        public void RemoveFullscreen(GraphicsDeviceManager graphics, GameWindow Window, Settings settings)
        {
            graphics.IsFullScreen = false;
            RestoreWindow(graphics, Window, settings);
        }

        /*
         * Save window settings.
         */
        public void SaveWindow(GameWindow Window, Settings settings)
        {
            settings.X = Window.ClientBounds.X;
            settings.Y = Window.ClientBounds.Y;
            settings.Width = Window.ClientBounds.Width;
            settings.Height = Window.ClientBounds.Height;
        }

        /*
         * Restore the window to defaults.
         */
        public void RestoreWindow(GraphicsDeviceManager graphics, GameWindow Window, Settings settings)
        {
            Window.Position = new Point(settings.X, settings.Y);
            graphics.PreferredBackBufferWidth = settings.Width;
            graphics.PreferredBackBufferHeight = settings.Height;
            graphics.ApplyChanges();
        }
    }
}