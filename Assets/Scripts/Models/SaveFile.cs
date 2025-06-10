

using System;
using System.Diagnostics.CodeAnalysis;

namespace Models
{
    [Serializable]
    public class SaveFile
    {
        // party of the current run
        [MaybeNull] public CharacterData[] Characters { get; set; }
        public string SaveName { get; set; } // Name of the save file
        //public DateTime SaveDate { get; set; } // Date when the last save was made
    }

    [Serializable]
    public class CharacterData
    {
        public string name;
        public int level;
        public int xp;
        public int maxHealth;
        public int currentHealth;

        // Assuming CombatStats is also [Serializable] with public fields
        public CombatStats combatStats;

        // Store only the data you need; e.g. asset path instead of Sprite
        public string spriteAssetPath;

        public string[] equipmentResourcePaths;
        public string[] skillResourcePaths;
    }
}