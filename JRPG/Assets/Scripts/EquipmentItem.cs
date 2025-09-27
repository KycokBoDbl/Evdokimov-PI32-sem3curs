using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class EquipmentItem : Item
{
    public enum EquipmentSlot
    {
        Weapon,
        Head,
        Body,
        Accessory
    }

    public EquipmentSlot equipmentSlot;
    public int attackBonus;
    public int defenseBonus;
    public int magicBonus;
    public int speedBonus;

    public override void Use()
    {
        base.Use();
        EquipmentManager.Instance.Equip(this);
    }
}