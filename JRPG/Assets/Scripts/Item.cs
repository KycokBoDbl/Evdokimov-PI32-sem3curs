using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Consumable,
        Weapon,
        Armor,
        Accessory,
        Material,
        KeyItem
    }

    public string itemID;
    public string itemName;
    public string description;
    public ItemType itemType;
    public int maxStack = 1;
    public int buyPrice;
    public int sellPrice;
    public Sprite icon;

    public virtual void Use()
    {
        Debug.Log($"Using {itemName}");

    }

    public virtual bool IsUsable()
    {
        return itemType == ItemType.Consumable;
    }
}