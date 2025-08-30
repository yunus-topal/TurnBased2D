using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Helpers;
using Models;
using Models.Scriptables;
using UnityEngine;

namespace Combat
{
    public class CombatManager : MonoBehaviour
    {
        [SerializeField] private List<EnemyGroup> enemyGroups;
        [SerializeField] private GameObject combatPanel;

        private TurnManager _turnManager;
        private List<Character> playerCharacters = new();
        private List<Character> enemyCharacters = new();

        private void Start()
        {
            _turnManager = GetComponent<TurnManager>();
        }

        // node should feed this seed to here.
        public void SetupCombat(double combatSeed)
        {
            var enemyGroup = PickEnemyGroup(combatSeed);
            combatPanel.SetActive(true);
            var characterDatas = SaveHelper.CurrentSaveFile.Characters;
            playerCharacters = characterDatas.ToList().FromData();
            enemyCharacters = enemyGroup.Characters.ToCharacters();
            combatPanel.GetComponent<CombatPanelHelper>().Initialize(playerCharacters, enemyGroup);
            StartCoroutine(StartCombat());
        }

        private EnemyGroup PickEnemyGroup(double combatSeed)
        {
            // normalize combat seed to total weights in enemy groups.
            float totalWeight = 0f;
            foreach (var enemyGroup in enemyGroups)
            {
                totalWeight += enemyGroup.weight;
            }
            combatSeed *= totalWeight;
            totalWeight = 0f;
            foreach (var enemyGroup in enemyGroups)
            {
                totalWeight += enemyGroup.weight;
                if (combatSeed < totalWeight)
                    return enemyGroup;
            }

            // should be unreachable since combatSeed will between 0 and 1.
            return enemyGroups[^1];
        }

        private IEnumerator StartCombat()
        {
            // Handle turn order.
            List<Character> turns = new();
            // play the turns one by one.
            while (!IsCombatOver())
            {
                // turn order can change depending on character buffs.
                turns = GetTurnOrder();
                // skip if a character's health is less than 0
                foreach (var character in turns)
                {
                    if(character.CurrentHealth <= 0) continue;
                    yield return StartCoroutine(_turnManager.PlayTurn(character));
                    //Debug.Log($"{character.Name} played their turn.");
                }
            }
        }

        
        // TODO: make calculations according to stats later.
        // For now, just combine the lists and return.
        private List<Character> GetTurnOrder()
        {
            // remove characters with health less than zero.
            return playerCharacters
                .Concat(enemyCharacters)
                .Where(c => c.CurrentHealth > 0)
                .ToList();        
        }

        // check if one of the sides are completely dead.
        private bool IsCombatOver()
        {
            return false;
        }
    }
}
