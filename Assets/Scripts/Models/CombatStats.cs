namespace Models {

    public static class CombatStatsExtensions
    {
        public static CombatStats Clone(this CombatStats stats)
        {
            return new CombatStats(stats.Strength, stats.Dexterity, stats.Intelligence, stats.Constitution, stats.Luck);
        }
    }
    [System.Serializable]
    public class CombatStats {
        [UnityEngine.SerializeField]
        private int _strength;
        [UnityEngine.SerializeField]
        private int _dexterity;
        [UnityEngine.SerializeField]
        private int _intelligence;
        [UnityEngine.SerializeField]
        private int _constitution;
        [UnityEngine.SerializeField]
        private int _luck;
        
        public CombatStats(int strength, int dexterity, int intelligence, int constitution, int luck)
        {
            _strength = strength;
            _dexterity = dexterity;
            _intelligence = intelligence;
            _constitution = constitution;
            _luck = luck;
        }

        public int Strength {
            get => _strength;
            set => _strength = value;
        }

        public int Dexterity {
            get => _dexterity;
            set => _dexterity = value;
        }

        public int Intelligence {
            get => _intelligence;
            set => _intelligence = value;
        }

        public int Constitution {
            get => _constitution;
            set => _constitution = value;
        }

        public int Luck {
            get => _luck;
            set => _luck = value;
        }
    }
}