using UnityEngine;

namespace Helpers {
    public class CombatPanelHelper : MonoBehaviour {
        [SerializeField] private GameObject combatantUIPrefab;
        [SerializeField] private GameObject playerGroup;
        [SerializeField] private GameObject enemyGroup;
    
        public void AddCharacter(Sprite sprite, string charName, int maxHealth) {
            var combatCharacter = Instantiate(combatantUIPrefab, playerGroup.transform);
            combatCharacter.GetComponent<CombatCharacterUIHelper>().SetCharacter(sprite, charName, maxHealth);
        }
    
        public void AddEnemy(Sprite sprite, string charName, int maxHealth) {
            var combatCharacter = Instantiate(combatantUIPrefab, enemyGroup.transform);
            combatCharacter.GetComponent<CombatCharacterUIHelper>().SetCharacter(sprite, charName, maxHealth);
            combatCharacter.transform.parent = enemyGroup.transform;
        }
    }
}
