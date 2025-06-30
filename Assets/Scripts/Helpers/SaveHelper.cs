
using System;
using System.IO;
using System.Linq;
using Models;
using Models.Scriptables;
using UnityEngine;
using Newtonsoft.Json;

namespace Helpers
{
    public static class SaveHelper
    {
        private static SaveFile currentSaveFile;
        public static SaveFile CurrentSaveFile
        {
            get
            {
                if (currentSaveFile == null)
                {
                    var lastUsedSave = PlayerPrefs.GetString(Constants.lastUsedSaveKey, string.Empty);
                    currentSaveFile = LoadSaveFileFromFileName(lastUsedSave);
                }
                return currentSaveFile;
            }
        }        
        public static void SetupSaveFolder()
        {
            if (!Directory.Exists(Constants.saveFileLocation))
            {
                try
                {
                    Directory.CreateDirectory(Constants.saveFileLocation);
                    Debug.Log($"Save file directory created at {Constants.saveFileLocation}");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to create save file directory: {ex.Message}");
                }
            }
            else
            {
                Debug.Log($"Save file directory already exists at {Constants.saveFileLocation}");
            }
        }
        public static SaveFile[] GetAllSaveFiles()
        {
            var saveFiles = Directory.GetFiles(Constants.saveFileLocation, "*.json")
                .Select(file => Path.GetFileNameWithoutExtension(file))
                .Select(fileName => LoadSaveFileFromFileName(fileName))
                .Where(saveFile => saveFile != null)
                .ToArray();
            return saveFiles;
        }

        internal static void SaveNewSaveFile(SaveFile newSaveFile)
        {
            if (newSaveFile == null)
            {
                Debug.LogError("Cannot save a null SaveFile.");
                return;
            }
            if (string.IsNullOrEmpty(newSaveFile.SaveName))
            {
                Debug.LogError("SaveFile must have a valid SaveName.");
                return;
            }
            //newSaveFile.SaveDate = DateTime.Now; // Update the save date to current time
            WriteToDisk(newSaveFile);
        }

        internal static void UpdateSaveFile(Character[] characters)
        {
            currentSaveFile.Characters = characters.Select(x => x.ToData()).ToArray();
            
            // write back to same file.
            WriteToDisk(currentSaveFile);
        }

        /// <summary>Read JSON from disk and deserialize into SaveFile</summary>
        public static SaveFile LoadSaveFileFromFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;
            var filePath = Path.Combine(Constants.saveFileLocation, $"{fileName}.json");

            if (!File.Exists(filePath))
                return null;

            string json = File.ReadAllText(filePath);
            Debug.Log($"[LoadSaveFileFromFileName] Loading save file from {filePath} with content: {json}");
            var saveFile = JsonConvert.DeserializeObject<SaveFile>(json);
            Debug.Log($"Save file '{fileName}' loaded successfully with {saveFile?.Characters?.Length ?? 0} characters.");
            currentSaveFile = saveFile;
            return saveFile;
        }

        private static void WriteToDisk(SaveFile saveFile)
        {
            var filePath = Path.Combine(Constants.saveFileLocation, $"{saveFile.SaveName}.json");
            try
            {
                Debug.Log($"save name: {saveFile.SaveName}");
                // write newtonsoft json to file
                string json = JsonConvert.SerializeObject(saveFile, Formatting.Indented);

                Debug.Log($"Saving save file to {filePath} with content: {json}");
                File.WriteAllText(filePath, json);
                Debug.Log($"Save file '{saveFile.SaveName}' saved successfully.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to save the save file: {ex.Message}");
            }
        }

        public static CharacterData ToData(this Character c)
        {
            return new CharacterData
            {
                name = c.Name,
                level = c.Level,
                xp = c.XP,
                maxHealth = c.MaxHealth,
                currentHealth = c.CurrentHealth,
                combatStats = c.CombatStats,         // if CombatStats is serializable
                spriteAssetPath = c.Sprite.name,     // or AssetDatabase path in Editor
                equipmentResourcePaths = c.Equipments
                                      .Select(s => $"Equipments/{s.name}") // Assuming Equipment has a name property
                                      .ToArray(),
                skillResourcePaths = c.Skills
                                  .Select(s => $"Skills/{s.name}")
                                  .ToArray()
            };
        }

        public static Character FromData(this CharacterData d, CharacterSO template)
        {
            // You can load the Sprite by Resources.Load<Sprite>(d.spriteAssetPath)
            // Then pass it into your Character constructor or a factory method
            var character = new Character(template)
            {
                Name = d.name,
                Level = d.level,
                XP = d.xp,
                MaxHealth = d.maxHealth,
                CurrentHealth = d.currentHealth,
                CombatStats = d.combatStats,
                Sprite = Resources.Load<Sprite>(d.spriteAssetPath),
                Equipments = d.equipmentResourcePaths
                             .Select(path => Resources.Load<Equipment>(path))
                             .Where(e => e != null)
                             .ToList(),
                Skills = d.skillResourcePaths
                         .Select(path => Resources.Load<Skill>(path))
                         .Where(s => s != null)
                         .ToList()
            };
            return character;
        }
    }
}