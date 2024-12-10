using System;
using System.Collections.Generic;
using Models;
using Models.Combatants;
using TMPro;
using UnityEngine;

namespace Controllers {
    public class GameController : MonoBehaviour {
        [SerializeField] private GameObject _mainPanel;
        [SerializeField] private TMP_Text _characterNames;
        
        private CombatController _combatController;
        private List<Character> _characters;
        private List<Enemy> _enemies;

        private void Start() {
            TryGetComponent(out _combatController);
            if (_combatController == null) {
                Debug.LogError("Combat Controller is not set in the Game Controller");
            }
            _characters = new List<Character>();
            _enemies = new List<Enemy>();
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
        
        // test method to create an enemy.
        public void CreateEnemy() {
            var enemy = new Enemy("Enemy", new CombatStats(5, 5, 5, 5, 5), 100, 50, null, null);
            _enemies.Add(enemy);
        }
        
        public void StartCombat() {
            if(_enemies.Count == 0) CreateEnemy();
            
            _mainPanel.SetActive(false);
            var combatants = new List<Combatant>();
            combatants.AddRange(_characters);
            combatants.AddRange(_enemies);
            _combatController.StartCombat(combatants);
        }
        
    }
}