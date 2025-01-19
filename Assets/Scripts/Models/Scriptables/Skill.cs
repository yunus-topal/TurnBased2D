using UnityEngine;
using UnityEngine.Serialization;

namespace Models.Scriptables {
    [CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Objects/Skill")]
    public class Skill : ScriptableObject {
        [SerializeField] private Sprite skillIcon;
        [SerializeField] private string skillName;
        [SerializeField] private int level;
        [SerializeField] private int cost;
        [SerializeField] private int cooldown;
        [SerializeField] private SkillType skillType;
        [SerializeField] private SkillTarget skillTarget;
        [SerializeField] private string description;
        
        public Sprite SkillIcon {
            get => skillIcon;
            set => skillIcon = value;
        }
        public string SkillName {
            get => skillName;
            set => skillName = value;
        }

        public int Level {
            get => level;
            set => level = value;
        }

        public int Cost {
            get => cost;
            set => cost = value;
        }

        public int Cooldown {
            get => cooldown;
            set => cooldown = value;
        }

        public SkillType SkillType {
            get => skillType;
            set => skillType = value;
        }

        public SkillTarget SkillTarget {
            get => skillTarget;
            set => skillTarget = value;
        }
        
        public string Description {
            get => description;
            set => description = value;
        }
    }
    
    public enum SkillType {
        Active,
        Passive,
        Toggle
    }
    
    public enum SkillTarget{
        Self,
        Enemy,
        Ally,
        AllEnemies,
        AllAllies
    }
}