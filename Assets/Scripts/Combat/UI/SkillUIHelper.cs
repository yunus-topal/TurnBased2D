using Models.Scriptables;
using UnityEngine;
using UnityEngine.UI;

namespace Combat.UI {
    public class SkillUIHelper : MonoBehaviour
    {
        private Skill[] _skills = new Skill[4];
        private Skill _selectedSkill;

        [SerializeField]
        private Image portrait;
        
        [SerializeField]
        private Button[] skillButtons;
        
        public void SetPortrait(Sprite sprite) {
            portrait.sprite = sprite;
        }

        public void SetSkill(Skill skill, int index) {
            if (index < 0 || index >= _skills.Length) {
                Debug.LogError("Index out of range");
                return;
            }
            skillButtons[index].GetComponent<Image>().sprite = skill.skillIcon;
            _skills[index] = skill;
        }
        
        public void LoadCharacterSkills(Skill[] skills) {
            for (int i = 0; i < skills.Length; i++) {
                SetSkill(skills[i], i);
            }
        }
        
        public void SelectSkill(int index) {
            if (index < 0 || index >= _skills.Length) {
                Debug.LogError("Index out of range");
                return;
            }
            Debug.Log($"Selected skill: {_skills[index].skillName}");
            _selectedSkill = _skills[index];
        }
        
    }
}
