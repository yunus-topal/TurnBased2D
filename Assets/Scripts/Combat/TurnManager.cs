using System;
using System.Collections;
using System.Collections.Generic;
using Models;
using Models.Scriptables;
using UnityEngine;

namespace Combat
{
    // handle combat turns
    public class TurnManager : MonoBehaviour
    {
        private CombatPanelHelper _combatPanelHelper;

        private void Start()
        {
            _combatPanelHelper = FindAnyObjectByType<CombatPanelHelper>(FindObjectsInactive.Include);
            if (_combatPanelHelper == null)
                Debug.LogError("No CombatPanelHelper found");   
        }

        internal IEnumerator PlayTurn(Character character)
        {
            Debug.Log($"Playing turn of character: {character.ToString()}");
            _combatPanelHelper.SetTurnLabel(character.Name);
            // TODO: show a text such as xxx's turn.
            switch (character.Team)
            {
                case Team.Player:
                    // TODO: update skill UI with this character.
                    // wait until user selects a skill and a target.
                    _combatPanelHelper.SetSkillsUI(character);
                    yield return new WaitForSeconds(10f); // for testing
                    yield break;
                case Team.Enemy:
                    _combatPanelHelper.SetSkillsUI(character);
                    yield return new WaitForSeconds(10f); // “think” for 1 second
                    var skill = AIChooseSkill(character);
                    var target = AIChooseTarget(character, skill);
                    //yield return StartCoroutine(ExecuteAction(character, );
                    yield break;
                case Team.Neutral:
                    yield break;
            }
        }

        // ideally, skill selection should depend on opponent status.
        private Skill AIChooseSkill(Character character)
        {
            return null;
        }

        private List<Character> AIChooseTarget(Character character, Skill skill)
        {
            return new();
        }

        private IEnumerator ExecuteAction(Character character, Skill skill)
        {
            // play VFX, SFX
            // update states of effected characters.
            yield return new WaitForSeconds(1f);
        }
    }
}