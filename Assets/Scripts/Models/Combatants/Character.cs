using System.Collections.Generic;
using Helpers;
using Models.Scriptables;
using UnityEngine;

namespace Models.Combatants {
    // for playable characters.
    public class Character : Combatant
    {
        // character info
        private string _characterName;
        private int _level;
        private int _experience;
        private Sprite _characterSprite;
        
        // equipment and skills and potentially new features.
        private List<Equipment> _equipments;
        private List<Skill> _skills;
        
        public Character(string characterName, int level, int experience, Sprite characterSprite, CombatStats combatStats, List<Equipment> equipments = null, List<Skill> skills = null)
        {
            _characterName = characterName;
            _level = level;
            _experience = experience;
            _characterSprite = characterSprite;
            _combatStats = combatStats;
            Health = CalculateHealth(combatStats);
            _equipments = equipments ?? new List<Equipment>(); // Default to an empty list
            _skills = skills ?? new List<Skill>();             // Default to an empty list
        }
        
        public Character(CharacterSO characterScriptable) : 
            this(characterScriptable.CharacterName, characterScriptable.Level, characterScriptable.Experience, 
                characterScriptable.CharacterSprite, Tester.GetDummyCombatStats(), 
                characterScriptable.Equipments, characterScriptable.Skills) { }

        #region Properties

        // getter and setters
        public string CharacterName {
            get => _characterName;
            set => _characterName = value;
        }

        public int Level {
            get => _level;
            set => _level = value;
        }

        public int Experience {
            get => _experience;
            set => _experience = value;
        }

        public Sprite CharacterSprite {
            get => _characterSprite;
            set => _characterSprite = value;
        }

        public CombatStats CombatStats {
            get => _combatStats;
            set => _combatStats = value;
        }

        public List<Equipment> Equipments {
            get => _equipments;
            set => _equipments = value;
        }

        public List<Skill> Skills {
            get => _skills;
            set => _skills = value;
        }

        #endregion Properties


        private int CalculateHealth(CombatStats combatStats)
        {
            // TODO: Implement a proper health calculation based on combat stats.
            return 100;
        }
    }
}
