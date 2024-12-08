namespace Models {
    public class Equipment {
        private string _equipmentName;
        private EquipmentType _equipmentType;
        private int _baseValue;
        
        
    }
    
    public enum EquipmentType {
        Weapon,
        Armor,
        Accessory
    }
}