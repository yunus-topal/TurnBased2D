using System.Collections;
using System.Collections.Generic;
using Models;
using Models.Scriptables;
using UnityEngine;

namespace Combat
{
    // handle combat turns
    public class TurnManager
    {
        internal IEnumerator PlayTurn(Character character)
        {
            // TODO: show a text such as xxx's turn.
            switch (character.Team)
            {
                case Team.Player:
                    // TODO: update skill UI with this character.
                    // wait until user selects a skill and a target.
                    break;
                case Team.Enemy:
                    yield return new WaitForSeconds(1f); // “think” for 1 second
                    var skill = AIChooseSkill(character);
                    var target = AIChooseTarget(character, skill);
                    //yield return StartCoroutine(ExecuteAction(character, );
                    break;
                case Team.Neutral:
                    break;
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