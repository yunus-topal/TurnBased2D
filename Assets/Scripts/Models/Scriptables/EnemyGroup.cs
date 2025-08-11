using System.Collections.Generic;
using UnityEngine;

namespace Models.Scriptables
{
    [CreateAssetMenu(fileName = "EnemyGroup", menuName = "Scriptable Objects/EnemyGroup")]
    public class EnemyGroup : ScriptableObject
    {
        public List<CharacterSO> characters = new List<CharacterSO>();
        public float weight = 1f;
        // to have constraints on enemy spawn. Can also create separate lists for each arc.
        public int chapter;

        
        
        public List<CharacterSO> Characters => characters;
    }
}