using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager Instance { get; private set; }

    public Dictionary<EquipmentItem.EquipmentSlot, EquipmentItem> equippedItems = new Dictionary<EquipmentItem.EquipmentSlot, EquipmentItem>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            foreach (EquipmentItem.EquipmentSlot slot in System.Enum.GetValues(typeof(EquipmentItem.EquipmentSlot)))
            {
                equippedItems[slot] = null;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Equip(EquipmentItem item)
    {
        if (equippedItems[item.equipmentSlot] != null)
        {
            Unequip(item.equipmentSlot);
            return;
        }

        equippedItems[item.equipmentSlot] = item;
        PlayerCharacter.Instance.ApplyEquipmentBonuses(item);
        
        Debug.Log($"Экипировано: {item.itemName} в слот {item.equipmentSlot}");

        Inventory.Instance.onInventoryChangedCallback?.Invoke();
    }

    public void Unequip(EquipmentItem.EquipmentSlot slot)
    {
        if (equippedItems[slot] != null)
        {
            EquipmentItem oldItem = equippedItems[slot];
            PlayerCharacter.Instance.RemoveEquipmentBonuses(oldItem);
            equippedItems[slot] = null;

            Debug.Log($"Снято: {oldItem.itemName} из слота {slot}");

            Inventory.Instance.onInventoryChangedCallback?.Invoke();
        }
    }
}