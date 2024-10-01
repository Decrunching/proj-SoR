using System;
using System.IO;
using Newtonsoft.Json;
using Logic.Entities.Character;
using Microsoft.Xna.Framework;

namespace Logic.Game.Data
{
    /*
     * Save game data using JSON serialisation.
     */
    public class GameState
    {
        public Vector2 Position { get; set; }
        public int HitPoints { get; set; }
        public string Skin { get; set; }
        public string CurrentMap { get; set; }

        /*
         * Save the current game state to a JSON file.
         */
        public static void SaveFile(Entity player, string currentMap)
        {
            GameState save = new()
            {
                Position = player.Position,
                HitPoints = player.HitPoints,
                Skin = player.Skin,
                CurrentMap = currentMap
            };

            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto
            };

            string jsonData = JsonConvert.SerializeObject(save, Formatting.Indented, jsonSettings);

            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SoR"));

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SoR\\saveFile.json");

            File.WriteAllText(filePath, jsonData);
        }

        /*
         * Load saved game data from a JSON file.
         */
        public static GameState LoadFile()
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SoR\\saveFile.json");

            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);

                GameState loadedGameData = JsonConvert.DeserializeObject<GameState>(jsonData);

                return loadedGameData;
            }

            /*try
            {
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error encountered while attempting to load game data: {ex.Message}");
            }*/

            return null;
        }
    }
}