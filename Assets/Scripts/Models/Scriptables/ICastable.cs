using System.Collections.Generic;
using UnityEngine;

namespace Models.Scriptables
{
    public interface ICastable
    {
        public void Cast(Character caster, Character target, List<Character> playerChars, List<Character> enemyChars);
    }
    
    public abstract class CastBehavior : ScriptableObject, ICastable
    {
        public abstract void Cast(Character caster, Character target, List<Character> playerChars, List<Character> enemyChars);
    }
}