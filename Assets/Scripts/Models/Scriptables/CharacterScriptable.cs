using System.Collections.Generic;
using UnityEngine;

namespace Models.Scriptables {
    [CreateAssetMenu(fileName = "CharacterScriptable", menuName = "Scriptable Objects/CharacterScriptable")]
    public class CharacterScriptable : ScriptableObject
    {
        [SerializeField] private string characterName;
        [SerializeField] private int level;
        [SerializeField] private int experience;
        [SerializeField] private Sprite characterSprite;
        [SerializeField] private int health;
        [SerializeField] private List<Equipment> equipments;
        [SerializeField] private List<Skill> skills;
        
        public string CharacterName {
            get => characterName;
            set => characterName = value;
        }

        public int Level {
            get => level;
            set => level = value;
        }

        public int Experience {
            get => experience;
            set => experience = value;
        }

        public Sprite CharacterSprite {
            get => characterSprite;
            set => characterSprite = value;
        }
        
        public List<Skill> Skills {
            get => skills;
            set => skills = value;
        }
        
        public List<Equipment> Equipments {
            get => equipments;
            set => equipments = value;
        }
        
        public int Health {
            get => health;
            set => health = value;
        }
    }
}
