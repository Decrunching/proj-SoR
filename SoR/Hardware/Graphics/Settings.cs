﻿/*using System.Text.Json.Serialization;

namespace Logic.Game.Graphics
{
    public class Settings
    {
        public int X { get; set; } = 200; // 320 200
        public int Y { get; set; } = 150; // 180 150
        public int Width { get; set; } = 800; //1280 800
        public int Height { get; set; } = 600; // 720 600
        public bool IsFixedTimeStep { get; set; } = true;
        public bool IsVSync { get; set; } = false;
        public bool IsFullscreen { get; set; } = false;
        public bool IsBorderless { get; set; } = false;
    }

    [JsonSourceGenerationOptions(
        PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
        WriteIndented = true)]
    [JsonSerializable(typeof(GraphicsSettings))]
    internal partial class SettingsContext : JsonSerializerContext { }
}