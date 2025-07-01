

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
        public int SeedNumber { get; set; }
        public int floorNumber { get; set; }
        public int nodeNumber { get; set; }
        //public DateTime SaveDate { get; set; } // Date when the last save was made

        public SaveFile(string saveName, CharacterData[] characters = null)
        {
            this.SaveName = saveName;
            this.Characters = characters;
            Random random = new Random();
            this.SeedNumber = random.Next();
            this.floorNumber = -1;
            this.nodeNumber = -1;
        }

        public void SetSeed(int seed)
        {
            this.SeedNumber = seed;
        }
        
        // override to string method
        public override string ToString()
        {
            return $"SaveFile: {SaveName}, Characters: {Characters?.Length ?? 0}, SeedNumber: {SeedNumber}";
        }
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