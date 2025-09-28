using System.Collections.Generic;
using Helpers;
using Models;
using UnityEngine;

namespace Rest
{
    public class RestPanelHelper : MonoBehaviour
    {
        [SerializeField] private GameObject subMenuOverlay;
        [SerializeField] private UpgradeSkillUIHelper upgradeSkillUIHelper;
        [SerializeField] private List<RestCharacterUIHelper> characterUIs;

        public void Initialize(List<Character> characters)
        {
            subMenuOverlay.SetActive(false);
            var playerCount = GeneralHelper.ClampAndWarn(characters.Count, characterUIs.Count, "Player");
            
            for (int i = 0; i < characterUIs.Count; i++)
            {
                bool active = i < playerCount;
                characterUIs[i].gameObject.SetActive(active);
                if (active) characterUIs[i].Initialize(characters[i], SetSubMenuOverlayActive);
            }
        }

        public void SetSubMenuOverlayActive(Character character, bool active)
        {
            // also fetch the character info from toggle event so we can initialize upgrade ui helper.
            subMenuOverlay.SetActive(active);
            if (active) upgradeSkillUIHelper.InitializeSkillsUI(character);
        }

        public void CancelUpgrade(Character character)
        {
            var index = characterUIs.FindIndex(c => c.Character.InstanceId == character.InstanceId);
            if (index == -1)
            {
                Debug.LogError("Character not found!");
                return;
            }
            
            characterUIs[index].SetCharacterUIRest();
        }
    }
}
