using System;
using System.Collections.Generic;
using Helpers;
using Models.Scriptables;
using UnityEngine;

namespace Models {
    public enum Team {Player, Enemy, Neutral}
    public class Character
    {
        private const string characterResourcePath = "Combatants/";
        // character info
        public string Name { get; set; } 
        public int Level { get; set; }
        public int XP { get; set; }
        public Sprite Sprite { get; set; }
        public int MaxHealth { get; set; } 
        public int CurrentHealth { get; set; } 
        public CombatStats CombatStats { get; set; } 
        // only for combat 
        public Team Team { get; set; }
        // equipment and skills and potentially new features.
        public List<Equipment> Equipments { get; set; }
        public List<Skill> Skills { get; set; }
        
        public string scriptableObjectPath { get; set; }
        
        public Character(CharacterSO characterScriptable)
        {
            Name = characterScriptable.characterName;
            Level = characterScriptable.level;
            Sprite = characterScriptable.characterSprite;
            CombatStats = characterScriptable.combatStats;
            Equipments = new List<Equipment>(characterScriptable.equipments);
            Skills = new List<Skill>(characterScriptable.skills);
            Team = characterScriptable.team;
            
            // Calculate health based on combat stats.
            MaxHealth = CalculateHealth(CombatStats);
            CurrentHealth = MaxHealth; // Initialize current health to max health.
            scriptableObjectPath = characterResourcePath + characterScriptable.name;
        }
        public Character(){}


        private int CalculateHealth(CombatStats combatStats)
        {
            // TODO: Implement a proper health calculation based on combat stats.
            return 100;
        }

        public void ApplyHeal(int heal)
        {
            
        }
        
        public void ApplyDamage(int damage)
        {
            
        }

        public void ApplyStatus(StatusEffectType statusEffectType, int durationInTurns)
        {
            
        } 
        
        public override string ToString()
        {
            return $"Character: {Name}" +
                   $"Team: {Team.ToString()}";
        }
    }
}
