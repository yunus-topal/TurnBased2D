using System;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using Models;
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
            
            _restPanel.Initialize(_playerCharacters);
        }
    }
}
