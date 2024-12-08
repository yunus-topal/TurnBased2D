namespace Models {
    public class CombatStats {
        private int _strength;
        private int _dexterity;
        private int _intelligence;
        private int _constitution;
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