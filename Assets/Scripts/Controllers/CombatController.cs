using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Models;
using Models.Combatants;
using UnityEngine;

namespace Controllers {
    public class CombatController : MonoBehaviour {
        
        [SerializeField] private GameObject _combatPanel;
        
        private List<Combatant> _combatants;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if(_combatPanel == null) {
                Debug.LogError("Combat Panel is not set in the Combat Controller");
            }
        }

        public void StartCombat(List<Combatant> combatants) {
            _combatants = combatants;
            _combatPanel.SetActive(true);
            CalculateInitiative();
            StartCoroutine(HandleCombatTurns());
        }

        private void CalculateInitiative() {
            // TODO: implement initiative calculation and sort the list.
        }

        private IEnumerator HandleCombatTurns()
        {
            while (!IsCombatEnded()) 
            {
                foreach (var combatant in _combatants.ToList()) // Use ToList() to allow modifying the original list
                {
                    // combat can finish at any point
                    if(IsCombatEnded()) break;
                    
                    // TODO: apply buffs and debuffs before the turn starts.
                    // ApplyBuffsAndDebuffs(combatant);
                    
                    if (combatant.Health <= 0)
                    {
                        // Remove dead combatant
                        _combatants.Remove(combatant);
                        // TODO: cleanup of combatant in the scene
                        // RemoveCombatantFromScene(combatant);
                        continue;
                    }

                    if (combatant is Character) // Check if it's the player's turn
                    {
                        Debug.Log("Player's turn!");
                        yield return WaitForPlayerInput(combatant as Character); // Handle player's turn
                    }
                    else
                    {
                        Debug.Log($"Combatant turn for {combatant.GetType().Name}!");
                        HandleCombatantTurn(combatant); // Handle enemy's turn
                        yield return new WaitForSeconds(1f); // Add delay for enemy actions
                    }
                }
            }

            EndCombat();
        }
        
        private bool IsCombatEnded()
        {
            return _combatants.Count <= 1;
        }
        
        private void EndCombat()
        {
            _combatPanel.SetActive(false);
            Debug.Log("Combat has ended!");
        }

        private IEnumerator WaitForPlayerInput(Character character) {
            // TODO: implement
            yield return null;
        }

        private void HandleCombatantTurn(Combatant combatant) {
            combatant.PlayTurn();
        }
    }
}
