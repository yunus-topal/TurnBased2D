using System;
using System.Collections.Generic;
using Models;
using UnityEngine;

namespace Combat.UI
{
    [CreateAssetMenu(menuName = "Combat/Status Effect Icons")]
    public sealed class StatusEffectIconDatabase : ScriptableObject
    {
        [Serializable]
        public struct StatusEffectIconEntry
        {
            public StatusEffectType Type;
            public Sprite Icon;
            public Color Tint;
            public string Description;
        }

        [SerializeField] private List<StatusEffectIconEntry> entries = new();

        private Dictionary<StatusEffectType, StatusEffectIconEntry> _map;

        private void OnEnable()
        {
            _map = new Dictionary<StatusEffectType, StatusEffectIconEntry>();
            foreach (var e in entries)
                _map[e.Type] = e;
        }

        public StatusEffectIconEntry TryGet(StatusEffectType type)
        {
            return _map.TryGetValue(type, out var entry) ? entry : new StatusEffectIconEntry();
        }
    }
}