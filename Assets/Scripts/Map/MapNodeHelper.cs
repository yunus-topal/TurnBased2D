using System;
using System.Collections.Generic;
using Combat;
using Helpers;
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
        private int floorIndex;
        private int nodeIndex;
        private Image nodeImage;
        private Button button;
        private GameManager _gameManager;
        private CombatManager _combatManager;
        private double nodeSeed;
        
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
            
            _gameManager = FindAnyObjectByType<GameManager>();
            _combatManager = FindAnyObjectByType<CombatManager>();
            if (_gameManager == null || _combatManager == null)
                Debug.LogError("GameManager or CombatManager not found");
        }
        public void Initialize(Encounter encounterType, int floorIndex, int nodeIndex, double nodeSeed)
        {
            this.floorIndex = floorIndex;
            this.nodeIndex = nodeIndex;
            this.nodeSeed = nodeSeed;
            _encounterType = encounterType;
            if (_spriteLookup.TryGetValue(_encounterType, out Sprite sprite))
            {
                nodeImage.sprite = sprite;
            }
            else
            {
                Debug.LogError($"No sprite found for encounter type {_encounterType}");
            }

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnNodeSelected);
            switch (_encounterType)
            {
                case Encounter.Merchant:
                    button.onClick.AddListener(MerchantSetup);
                    break;
                case Encounter.Boss:
                    button.onClick.AddListener(BossSetup);
                    break;
                case Encounter.Combat:
                    button.onClick.AddListener(CombatSetup);
                    break;
                case Encounter.Rest:
                    button.onClick.AddListener(RestSetup);
                    break;
                case Encounter.MiniBoss:
                    button.onClick.AddListener(MiniBossSetup);
                    break;
            }
            
        }

        public void SetNodeInteractable(bool interactable)
        {
            button.interactable = interactable;
        }

        private void OnNodeSelected()
        {
            nodeImage.color = Color.forestGreen;
        }
        private void CombatSetup()
        {
            // pick enemies for the encounter.
            _gameManager.SetActiveCombatPanel(true);
            _gameManager.SetActiveMapPanel(false);
            
            _combatManager.SetupCombat(nodeSeed);
        }

        private void BossSetup()
        {
            
        }

        private void MiniBossSetup()
        {
            
        }

        private void RestSetup()
        {
            
        }

        private void MerchantSetup()
        {
            
        }
    }
}
