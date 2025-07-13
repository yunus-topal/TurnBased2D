using System;
using System.Collections.Generic;
using Helpers;
using Models.Scriptables;
using UnityEngine;

namespace Models {
    public class Character
    {
        // character info
        public string Name { get; set; } 
        public int Level { get; set; }
        public int XP { get; set; }
        public Sprite Sprite { get; set; }
        public int MaxHealth { get; set; } 
        public int CurrentHealth { get; set; } 
        public CombatStats CombatStats { get; set; } 

        // equipment and skills and potentially new features.
        public List<Equipment> Equipments { get; set; }
        public List<Skill> Skills { get; set; }
        
        public Character(CharacterSO characterScriptable)
        {
            Name = characterScriptable.characterName;
            Level = characterScriptable.level;
            Sprite = characterScriptable.characterSprite;
            CombatStats = characterScriptable.combatStats;
            Equipments = new List<Equipment>(characterScriptable.equipments);
            Skills = new List<Skill>(characterScriptable.skills);

            // Calculate health based on combat stats.
            MaxHealth = CalculateHealth(CombatStats);
            CurrentHealth = MaxHealth; // Initialize current health to max health.
        }


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
    }
}
