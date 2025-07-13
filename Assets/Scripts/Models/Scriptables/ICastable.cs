using System.Collections.Generic;
using UnityEngine;

namespace Models.Scriptables
{
    public interface ICastable
    {
        public void Cast(Character caster, List<Character> target);
    }
    
    public abstract class CastBehavior : ScriptableObject, ICastable
    {
        public abstract void Cast(Character caster, List<Character> target);
    }
}