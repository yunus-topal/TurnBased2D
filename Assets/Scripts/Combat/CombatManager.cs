using System.Collections.Generic;
using Models.Scriptables;
using UnityEngine;

namespace Combat
{
    public class CombatManager : MonoBehaviour
    {
        [SerializeField] private List<EnemyGroup> enemyGroups;
        
        public void Initialize()
        {
            
        }

        // for dummy purposes. Spawn some enemies and enter combat mode.
        public void StartDummyCombat()
        {
            
        }
        
        // node should feed this seed to here.
        public void SetupCombat(double combatSeed)
        {
            var enemyGroup = PickEnemyGroup(combatSeed);
            // TODO: spawn enemy and player groups in ui.
            
            // TODO: start combat cycle.


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
