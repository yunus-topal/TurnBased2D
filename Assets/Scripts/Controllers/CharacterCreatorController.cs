using System.Collections.Generic;
using Models;
using Models.Combatants;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers {
    public class CharacterCreatorController: MonoBehaviour {
        
        [SerializeField] private TMP_InputField characterName;
        [SerializeField] private Image characterImage;
        [SerializeField] private List<Sprite> characterSprites;
        
        private GameController _gameController;
        private Sprite _selectedSprite;
        private int _selectedSpriteIndex;
        
        private void Start() {
            TryGetComponent(out _gameController);
            if (_gameController == null) {
                Debug.LogError("Game Controller is not set in the Character Creator Controller");
            }
            if(characterSprites.Count == 0) {
                Debug.LogError("Character Sprites are not set in the Character Creator Controller");
                return;
            }

            _selectedSpriteIndex = 0;
            _selectedSprite = characterSprites[0];
            characterImage.sprite = _selectedSprite;
        }
        
        public void ChangeSprite() {
            _selectedSpriteIndex = (_selectedSpriteIndex + 1) % characterSprites.Count;
            _selectedSprite = characterSprites[_selectedSpriteIndex];
            characterImage.sprite = _selectedSprite;
        }
        
        public void CreateCharacter() {
            // TODO: check if character name is not empty
            var character = new Character(characterName.text, 1, 0, _selectedSprite, new CombatStats(5, 5, 5, 5, 5), 100);
            _gameController.AddCharacter(character);
            // reset fields
            characterName.text = "";
            characterImage.sprite = characterSprites[0];
            _selectedSprite = characterSprites[0];
            _selectedSpriteIndex = 0;
        }
    }
}