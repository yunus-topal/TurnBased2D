using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Combat.UI;
using Helpers;
using JetBrains.Annotations;
using Models;
using Models.Scriptables;
using UnityEngine;
using Random = System.Random;

namespace Combat
{
    // handle combat turns
    [RequireComponent(typeof(CombatManager))]
    public class TurnManager : MonoBehaviour
    {
        private CombatPanelHelper _combatPanelHelper;
        private SkillUIHelper  _skillUIHelper;
        private CombatManager _combatManager;
        
        [CanBeNull] private Skill _selectedSkill;
        [CanBeNull] private Character _targetCharacter;
        [CanBeNull] private Character _actingCharacter;
        
        private void Start()
        {
            _combatManager = GetComponent<CombatManager>();
            _combatPanelHelper = FindAnyObjectByType<CombatPanelHelper>(FindObjectsInactive.Include);
            _skillUIHelper = FindAnyObjectByType<SkillUIHelper>(FindObjectsInactive.Include);
            if (_combatPanelHelper == null || _skillUIHelper == null)
                Debug.LogError("Combat Panel Helper or Skill UI Helper not found");   
        }

        internal IEnumerator PlayTurn(Character character)
        {
            //Debug.Log($"Playing turn of character: {character.Name}");
            _combatPanelHelper.SetTurnLabel(character.Name);
            _actingCharacter = character;
            _selectedSkill = null;
            _targetCharacter = null;
            
            _skillUIHelper.InitializeSkillsUI(character, this);

            switch (character.Team)
            {
                case Team.Player:

                    // if target is not suitable, set target to null and keep waiting.
                    yield return new WaitUntil(SelectionReady);
                    _selectedSkill!.Cast(_actingCharacter, _targetCharacter, _combatManager.PlayerCharacters, _combatManager.EnemyCharacters);
                    // reflect character updates on UI.
                    _combatPanelHelper.UpdateCharUIs(_combatManager.PlayerCharacters, _combatManager.EnemyCharacters);
                    break;
                case Team.Enemy:
                    yield return new WaitForSeconds(2f); // “think” for 1 second
                    var skill = AIChooseSkill(character);
                    var target = AIChooseTarget(character, skill);
                    skill.Cast(_actingCharacter, target,_combatManager.PlayerCharacters, _combatManager.EnemyCharacters);
                    _combatPanelHelper.UpdateCharUIs(_combatManager.PlayerCharacters, _combatManager.EnemyCharacters);
                    break;
                case Team.Neutral:
                    break;
            }
        }

        // ideally, skill selection should depend on opponent status.
        private Skill AIChooseSkill(Character character)
        {
            // get a random number for now
            var index = Constants.CombatRng.Next(0,character.Skills.Count);
            return character.Skills[index];
        }

        private Character AIChooseTarget(Character character, Skill skill)
        {
            var availableTargets = new List<Character>();
            availableTargets.AddRange(_combatManager.PlayerCharacters.Where(p => p.CurrentHealth > 0));
            var index = Constants.CombatRng.Next(0,availableTargets.Count);
            return availableTargets[index];
        }

        public void SetSelectedSkill(SkillButton skillButton)
        {
            // TODO: check if skill can be cast (mana cost, cooldown etc.)
            Debug.Log($"Turn manager selected skill: {skillButton.BoundSkill}");
            _selectedSkill = skillButton.BoundSkill;
        }

        public void SetSelectedTarget(Character target)
        {
            Debug.Log($"Turn manager selected target: {target.ToString()}");
            _targetCharacter = target;
        }
        
        // WaitUntil predicate – true only when the selection is complete and valid.
        private bool SelectionReady()
        {
            if (_targetCharacter == null) return false;
            if (_selectedSkill == null)   return false;

            if (!IsValidTarget(_actingCharacter, _selectedSkill, _targetCharacter))
            {
                // Keep waiting; also make sure we don't proceed with a bad target.
                _targetCharacter = null;
                return false;
            }

            return true;
        }
        
        private bool IsValidTarget(Character actor, Skill skill, Character target)
        {
            if (actor == null || skill == null || target == null) return false;

            // Example skeleton:
            return skill.castingTarget switch // adapt to your Skill Scriptable
            {
                SkillTarget.Self => target == actor,
                SkillTarget.SingleAlly => target.Team == Team.Player && target.CurrentHealth > 0,
                SkillTarget.AllAllies => target.Team == Team.Player && target.CurrentHealth > 0,
                SkillTarget.SingleEnemy  => target.Team == Team.Enemy && target.CurrentHealth > 0,
                SkillTarget.AllEnemies => target.Team == Team.Enemy && target.CurrentHealth > 0,
                _ => false
            };
        }
        
    }
}