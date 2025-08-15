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

        // node should feed this seed to here.
        public void SetupCombat(double combatSeed)
        {
            var enemyGroup = PickEnemyGroup(combatSeed);
            combatPanel.SetActive(true);
            var characterDatas = SaveHelper.CurrentSaveFile.Characters;
            combatPanel.GetComponent<CombatPanelHelper>().Initialize(characterDatas.ToList().FromData(), enemyGroup);
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
    }
}
