using System;
using System.Collections.Generic;
using Helpers;
using Models;
using TMPro;
using UnityEngine;

namespace Rest
{
    public class RestPanelHelper : MonoBehaviour
    {
        [SerializeField] private List<RestCharacterUIHelper> characterUIs;

        public void Initialize(List<Character> characters)
        {
            var playerCount = GeneralHelper.ClampAndWarn(characters.Count, characterUIs.Count, "Player");
            
            for (int i = 0; i < characterUIs.Count; i++)
            {
                bool active = i < playerCount;
                characterUIs[i].gameObject.SetActive(active);
                if (active) characterUIs[i].Initialize(characters[i]);
            }
        }
    }
}
