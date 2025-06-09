
using System.IO;
using System.Linq;
using Models;
using Models.Scriptables;
using UnityEngine;

namespace Helpers
{
    public static class SaveManager
    {
        /// <summary>Write the given object as JSON to disk.</summary>
        public static void Save<T>(T data, string filePath)
        {
            string json = JsonUtility.ToJson(data, prettyPrint: true);
            File.WriteAllText(filePath, json);
        }

        /// <summary>Read JSON from disk and deserialize into T (or default(T) if missing).</summary>
        public static T Load<T>(string filePath)
        {
            if (!File.Exists(filePath))
                return default;

            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<T>(json);
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