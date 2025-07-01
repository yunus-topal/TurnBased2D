using System;
using System.Collections.Generic;
using Models;
using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    [Serializable]
    public class EncounterSpriteMapping
    {
        public Encounter encounterType;
        public Sprite sprite;
    }
    public class MapNodeHelper : MonoBehaviour
    {
        private Image nodeImage;
        private Button button;
        
        [SerializeField] private List<EncounterSpriteMapping> encounterSprites;

        private Encounter _encounterType;
        private Dictionary<Encounter, Sprite> _spriteLookup;

        private void Awake()
        {
            nodeImage = GetComponent<Image>();
            button = GetComponent<Button>();
            // Convert list to dictionary for quick lookup
            _spriteLookup = new Dictionary<Encounter, Sprite>();
            foreach (var mapping in encounterSprites)
            {
                if (!_spriteLookup.ContainsKey(mapping.encounterType))
                    _spriteLookup.Add(mapping.encounterType, mapping.sprite);
            }
        }
        public void Initialize(Encounter encounterType)
        {
            _encounterType = encounterType;
            if (_spriteLookup.TryGetValue(_encounterType, out Sprite sprite))
            {
                nodeImage.sprite = sprite;
            }
            else
            {
                Debug.LogError($"No sprite found for encounter type {_encounterType}");
            }
        }

        public void SetNodeInteractable(bool interactable)
        {
            button.interactable = interactable;
        }
    }
}
