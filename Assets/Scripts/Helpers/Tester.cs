using Models;

namespace Helpers {
    // This is a testing class to fetch some placeholder constant values.
    public static class Tester {
        public static CombatStats GetDummyCombatStats() {
            return new CombatStats(5, 5, 5, 5, 5);
        }
    }
}