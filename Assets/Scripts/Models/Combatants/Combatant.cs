namespace Models.Combatants {
    public abstract class Combatant {
        internal CombatStats _combatStats;
        private int _health;
        
        public int Health {
            get => _health;
            set => _health = value;
        }

        public void PlayTurn() {
            
        }
    }
}