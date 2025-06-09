using UnityEngine;

namespace Models.Scriptables {
    [CreateAssetMenu(fileName = "Equipment", menuName = "Scriptable Objects/Equipment")]
    public class Equipment : ScriptableObject {
        public string EquipmentName { get; set; }
        public EquipmentType EquipmentType { get; set; }
    }

    public enum EquipmentType {
        Weapon,
        Armor,
        Accessory
    }
}