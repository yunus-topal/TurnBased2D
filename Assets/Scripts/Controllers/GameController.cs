using System;
using System.Collections.Generic;
using Models;
using Models.Combatants;
using Models.Scriptables;
using TMPro;
using UnityEngine;

namespace Controllers {
    public class GameController : MonoBehaviour {
        [SerializeField] private GameObject _mainPanel;
        [SerializeField] private TMP_Text _characterNames;
        
        private CombatController _combatController;
        private List<Character> _characters;
        private List<Enemy> _enemies;
        [SerializeField] private EnemyScriptable dummyEnemy;

        private void Start() {
            TryGetComponent(out _combatController);
            if (_combatController == null) {
                Debug.LogError("Combat Controller is not set in the Game Controller");
            }
            _characters = new List<Character>();
            _enemies = new List<Enemy>();
            // for testing purposes.
            _enemies.Add(new Enemy(dummyEnemy));
            
        }

        public void AddCharacter(Character character) {
            _characters.Add(character);
            ShowCharacterNames();
        }
        
        private void ShowCharacterNames() {
            _characterNames.text = "";
            foreach (var character in _characters) {
                _characterNames.text += character.CharacterName + "\n";
            }
        }
        
        public void StartCombat() {
            
            _mainPanel.SetActive(false);
            var combatants = new List<Combatant>();
            combatants.AddRange(_characters);
            combatants.AddRange(_enemies);
            _combatController.StartCombat(combatants);
        }
        
    }
}