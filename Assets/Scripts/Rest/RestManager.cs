using System.Collections.Generic;
using System.Linq;
using Helpers;
using Models;
using Models.Scriptables;
using UnityEngine;

namespace Rest
{
    public class RestManager : MonoBehaviour
    {
        // Show each character. Show their status.
        // Allow healing and
        // TODO: maybe other options such as increasing their stats (like training)

        private RestPanelHelper _restPanel;
        private GameManager _gameManager;
        private List<Character> _playerCharacters = new();
        private List<RestChoice> _choices = new();
        

        private void Awake()
        {
            _restPanel = FindAnyObjectByType<RestPanelHelper>(FindObjectsInactive.Include);
            _gameManager = FindAnyObjectByType<GameManager>();
        }

        public void Setup()
        {
            _gameManager.ActivateRestPanel();
            var characterDatas = SaveHelper.CurrentSaveFile.Characters;
            _playerCharacters = characterDatas.ToList().FromData();
            
            _choices = Enumerable.Repeat<RestChoice>(new RestChoice(), _playerCharacters.Count).ToList();
            _restPanel.Initialize(_playerCharacters);
        }

        public void UpdateChoiceAsUpgrade(Character character, Skill skill)
        {
            // find the character index and skill index.
            var charIndex = _playerCharacters.FindIndex(c => c.InstanceId == character.InstanceId);
            if (charIndex == -1)
            {
                Debug.LogError("Character not found");
                return;
            }

            var skillIndex = _playerCharacters[charIndex].Skills.FindIndex(s => s.Equals(skill));
            if (skillIndex == -1)
            {
                Debug.LogError("Skill not found");
                return;
            }

            _choices[charIndex].Index = skillIndex;
            _choices[charIndex].RestAction = RestAction.Upgrade;
        }

        public void UpdateChoiceAsRest(Character character, RestAction action)
        {
            var charIndex = _playerCharacters.FindIndex(c => c.InstanceId == character.InstanceId);
            if (charIndex == -1)
            {
                Debug.LogError("Character not found");
                return;
            }

            _choices[charIndex].Index = 0;
            _choices[charIndex].RestAction = action;
        }

    }

    public class RestChoice
    {
        public RestAction RestAction { get; set; } = RestAction.Rest;

        // will be used for skill upgrades.
        public int Index { get; set; } = 0;
    }

    public enum RestAction
    {
        Rest,
        Upgrade
    }
}
