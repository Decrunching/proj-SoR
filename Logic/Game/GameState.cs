using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;
using Logic.Game.GameMap;
using Logic.Entities.Character;

namespace Logic.Game
{
    /*
     * Save game data using JSON serialisation.
     */
    public class GameState
    {
        public Entity Player { get; set; }
        public string PlayerName { get; set; }
        public string SceneryName { get; set; }
        public string CurrentMap { get; set; }

        /*
         * Save the current game state to a JSON file.
         */
        public static void SaveFile(Entity player, string playerName, string currentMap)
        {
            GameState save = new GameState
            {
                Player = player,
                PlayerName = playerName,
                CurrentMap = currentMap
            };

            var jsonSettings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            string jsonData = JsonConvert.SerializeObject(save, jsonSettings);

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "saveFile.json");

            File.WriteAllText(filePath, jsonData);
        }

        /*
         * Load saved game data from a JSON file.
         */
        public static GameState LoadFile()
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "saveFile.json");

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