namespace Models {
    public class Skill {
        private string _skillName;
        private int _level;
        private int _cost;
        private int _cooldown;
    }
    
    public enum SkillType {
        Active,
        Passive,
        Toggle
    }
    
    public enum SkillTarget{
        Self,
        Enemy,
        Ally,
        All
    }
}