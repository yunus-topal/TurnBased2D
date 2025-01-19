using System.Collections.Generic;
using Models.Scriptables;
using UnityEngine;

namespace Models.Combatants {
    public class Enemy : Combatant{
        private string _enemyName;
        private int _experienceReward;
        private Sprite _enemySprite;
        
        private List<Skill> _skills;
        
        public Enemy(string enemyName, CombatStats combatStats, int health, int experienceReward = 0, Sprite enemySprite = null, List<Skill> skills = null)
        {
            _enemyName = enemyName;
            _combatStats = combatStats;
            Health = health;
            _experienceReward = experienceReward;
            _enemySprite = enemySprite;
            _skills = skills ?? new List<Skill>(); // Default to an empty list
        }
        
        public Enemy(EnemyScriptable enemyScriptable)
        {
            _enemyName = enemyScriptable.EnemyName;
            _combatStats = new CombatStats(5, 5, 5, 5, 5);
            Health = enemyScriptable.MaxHealth;
            _experienceReward = enemyScriptable.ExperienceReward;
            _enemySprite = enemyScriptable.EnemySprite;
            _skills = enemyScriptable.Skills;
        }
        
        #region Properties

        public string EnemyName {
            get => _enemyName;
            set => _enemyName = value;
        }

        public int ExperienceReward {
            get => _experienceReward;
            set => _experienceReward = value;
        }

        public Sprite EnemySprite {
            get => _enemySprite;
            set => _enemySprite = value;
        }

        public List<Skill> Skills {
            get => _skills;
            set => _skills = value;
        }

        public CombatStats CombatStats {
            get => _combatStats;
            set => _combatStats = value;
        }
        #endregion
    }
}