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
        // TODO: keep track of each image associated with each encounter type and assign it.
        
        private Image nodeImage;
        [SerializeField] private List<EncounterSpriteMapping> encounterSprites;

        private Encounter _encounterType;
        private Dictionary<Encounter, Sprite> _spriteLookup;

        private void Awake()
        {
            nodeImage = GetComponent<Image>();
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
    }
}
