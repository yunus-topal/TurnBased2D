using UnityEngine;

namespace Models.Scriptables {
    [CreateAssetMenu(fileName = "Equipment", menuName = "Scriptable Objects/Equipment")]
    public class Equipment : ScriptableObject {
        [SerializeField] private string _equipmentName;
        [SerializeField] private EquipmentType _equipmentType;
        [SerializeField] private int _baseValue;

        // Properties
        public string EquipmentName
        {
            get => _equipmentName;
            set => _equipmentName = value;
        }
        public EquipmentType EquipmentType
        {
            get => _equipmentType;
            set => _equipmentType = value;
        }
        public int BaseValue
        {
            get => _baseValue;
            set => _baseValue = value;
        }

    }

    public enum EquipmentType {
        Weapon,
        Armor,
        Accessory
    }
}