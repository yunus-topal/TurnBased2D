using System;
using System.Collections;
using System.Collections.Generic;
using Combat.UI;
using JetBrains.Annotations;
using Models;
using Models.Scriptables;
using UnityEngine;

namespace Combat
{
    // handle combat turns
    public class TurnManager : MonoBehaviour
    {
        private CombatPanelHelper _combatPanelHelper;
        private SkillUIHelper  _skillUIHelper;

        
        [CanBeNull] private Skill _selectedSkill;
        [CanBeNull] private Character _targetCharacter;
        
        private void Start()
        {
            _combatPanelHelper = FindAnyObjectByType<CombatPanelHelper>(FindObjectsInactive.Include);
            _skillUIHelper = FindAnyObjectByType<SkillUIHelper>(FindObjectsInactive.Include);
            if (_combatPanelHelper == null || _skillUIHelper == null)
                Debug.LogError("Combat Panel Helper or Skill UI Helper not found");   
        }

        internal IEnumerator PlayTurn(Character character)
        {
            Debug.Log($"Playing turn of character: {character.ToString()}");
            _combatPanelHelper.SetTurnLabel(character.Name);
            // TODO: show a text such as xxx's turn.
            switch (character.Team)
            {
                case Team.Player:
                    _skillUIHelper.InitializeSkillsUI(character, this);

                    // TODO: trigger a check here when selected skill and target character is set.
                    // if target is not suitable, set target to null and keep waiting.
                    yield return new WaitForSeconds(50f); 
                    
                    
                    yield break;
                case Team.Enemy:
                    _skillUIHelper.InitializeSkillsUI(character, this);
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

        public void SetSelectedSkill(SkillButton skillButton)
        {
            Debug.Log($"Turn manager selected skill: {skillButton.BoundSkill}");
            _selectedSkill = skillButton.BoundSkill;
        }

        public void SetSelectedTarget(Character target)
        {
            Debug.Log($"Turn manager selected target: {target.ToString()}");
            _targetCharacter = target;
        }

    }
}