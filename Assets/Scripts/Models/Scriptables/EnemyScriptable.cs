using System.Collections.Generic;
using UnityEngine;

namespace Models.Scriptables {
    [CreateAssetMenu(fileName = "EnemyScriptable", menuName = "Scriptable Objects/EnemyScriptable")]
    public class EnemyScriptable : ScriptableObject
    {
        [SerializeField] private string enemyName;
        [SerializeField] private int maxHealth;
        [SerializeField] private int experienceReward;
        [SerializeField] private Sprite enemySprite;
        [SerializeField] private List<Skill> skills;
        
        public string EnemyName {
            get => enemyName;
            set => enemyName = value;
        }

        public int MaxHealth {
            get => maxHealth;
            set => maxHealth = value;
        }

        public int ExperienceReward {
            get => experienceReward;
            set => experienceReward = value;
        }

        public Sprite EnemySprite {
            get => enemySprite;
            set => enemySprite = value;
        }
        
        public List<Skill> Skills {
            get => skills;
            set => skills = value;
        }
    }
}
