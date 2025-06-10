
using System.IO;
using System.Linq;
using Models;
using Models.Scriptables;
using UnityEngine;

namespace Helpers
{
    public static class SaveHelper
    {
        public static SaveFile[] GetAllSaveFiles()
        {
            var saveFiles = Directory.GetFiles(Application.persistentDataPath, "*.json")
                .Select(file => Path.GetFileNameWithoutExtension(file))
                .Select(fileName => LoadSaveFileFromFileName(fileName))
                .Where(saveFile => saveFile != null)
                .ToArray();
            return saveFiles;
        }

        /// <summary>Read JSON from disk and deserialize into SaveFile</summary>
        public static SaveFile LoadSaveFileFromFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;
            var filePath = Path.Combine(Application.persistentDataPath, $"{fileName}.json");

            if (!File.Exists(filePath))
                return null;

            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<SaveFile>(json);
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