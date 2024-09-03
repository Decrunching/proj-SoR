using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Net.Quic;
using System.Text.Json.Serialization.Metadata;
using System.Text.Json;
using System.IO;

namespace Logic.Game
{
    /*
     * Manage graphics settings.
     * 
     * https://learn-monogame.github.io/tutorial/game-settings/ <- using tutorial from Learn Monogame
     */
    public class GraphicsSettings : Settings
    {
        public bool Fullscreen { get; set; }
        public bool Borderless { get; set; }
        public bool ToggleFullscreen { get; set; }
        public bool ToggleBorderless { get; set; }
        public int screenWidth { get; set; }
        public int screenHeight { get; set; }
        private float sinceLastChange;
        private float screenChangeTime;
        private KeyboardState keyState;

        public GraphicsSettings(GraphicsDeviceManager graphics, GameWindow Window, Settings settings)
        {
            Window.AllowUserResizing = true;
            Window.AllowAltF4 = true;

            IsFixedTimeStep = settings.IsFixedTimeStep;
            graphics.SynchronizeWithVerticalRetrace = settings.IsVSync;

            settings.IsFullscreen = settings.IsFullscreen || settings.IsBorderless;

            RestoreWindow(graphics, Window, settings);
        }

        /*
         * 
         */
        public void UpdateSettings(GraphicsDeviceManager graphics, GameWindow Window, Settings settings)
        {
            if (QuicStreamType.Pressed())
                Exit();

            if (ToggleBorderlessMode(graphics, Window, settings).Pressed())
            {
                ToggleBorderlessMode(graphics, Window, settings);
            }

            if (resetSettings.Pressed())
            {
                bool fullscreenLast = settings.IsFullscreen;
                settings = new Settings();
                SaveJson("Settings.json", settings, SettingsContext.Default.Settings);

                ChangeFullscreen(graphics, Window, fullscreenLast, settings);
            }
        }

        /*
         * Toggle between borderless / windowed when F4 is pressed.
         * 
         * Has fullscreen option set up too, but I think fullscreen mode is annoying and pointless. If I 
         * ever release this as a full, playable game and people start asking for it, adding it back in
         * is as easy as calling ToggleFullscreen(graphics, Window).
         */
        public void ChooseScreenMode(GameTime gameTime, GraphicsDeviceManager graphics, GameWindow Window, Settings settings)
        {
            keyState = Keyboard.GetState(); // Get the current keyboard state

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            sinceLastChange += deltaTime;
            screenChangeTime = 0.3f;

            if (sinceLastChange >= screenChangeTime)
            {
                sinceLastChange = 0;

                if (keyState.IsKeyDown(Keys.F4))
                {
                    ToggleBorderlessMode(graphics, Window, settings);
                }
            }
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
         * 
         */
        public void SaveWindow(GameWindow Window, Settings settings)
        {
            settings.X = Window.ClientBounds.X;
            settings.Y = Window.ClientBounds.Y;
            settings.Width = Window.ClientBounds.Width;
            settings.Height = Window.ClientBounds.Height;
        }

        /*
         * 
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