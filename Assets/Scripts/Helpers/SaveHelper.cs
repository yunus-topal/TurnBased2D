
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Models;
using Models.Scriptables;
using UnityEngine;
using Newtonsoft.Json;
using Random = System.Random;

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

        public static void UpdateSaveFileSeed()
        {
            Random rand =  new Random();
            currentSaveFile.SeedNumber = rand.Next();
            WriteToDisk(currentSaveFile);
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
        
        internal static void UpdateSaveFile(Character[] characters, int floorNum)
        {
            currentSaveFile.Characters = characters.Select(x => x.ToData()).ToArray();
            currentSaveFile.floorNumber = floorNum;
            
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

        public static List<CharacterData> ToData(this List<Character> characters)
        {
            return characters.Select(c => c.ToData()).ToList();
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
                scriptableObjectPath = c.scriptableObjectPath,
            };
        }

        public static List<Character> FromData(this List<CharacterData> datas)
        {
            return datas.Select(item => item.FromData()).ToList();
        }

        public static Character FromData(this CharacterData d)
        {
            // You can load the Sprite by Resources.Load<Sprite>(d.spriteAssetPath)
            // Then pass it into your Character constructor or a factory method
            try
            {
                var c = Resources.Load<CharacterSO>(d.scriptableObjectPath);
                var character = c.ToCharacter(d);
                return character;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            
            Debug.LogError($"Failed to load character {d.name}");
            
            // TODO: should be unreachable
            return new Character
            {
                Name = d.name,
                Level = d.level,
                XP = d.xp,
                MaxHealth = d.maxHealth,
                CurrentHealth = d.currentHealth,
                CombatStats = d.combatStats,
            };
        }
    }
}