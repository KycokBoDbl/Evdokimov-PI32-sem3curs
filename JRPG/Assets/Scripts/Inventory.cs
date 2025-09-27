using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }

    public List<InventorySlot> items = new List<InventorySlot>();
    public int maxSlots = 20;
    public int currency = 1000;

    public delegate void OnInventoryChanged();
    public OnInventoryChanged onInventoryChangedCallback;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool AddItem(Item item, int quantity = 1)
    {
        if (item.maxStack > 1)
        {
            foreach (var slot in items)
            {
                if (slot.item == item && slot.quantity < item.maxStack)
                {
                    slot.quantity += quantity;
                    onInventoryChangedCallback?.Invoke();
                    return true;
                }
            }
        }

        if (items.Count >= maxSlots)
        {
            Debug.Log("Инвентарь полон!");
            return false;
        }

        items.Add(new InventorySlot(item, quantity));
        onInventoryChangedCallback?.Invoke();
        return true;
    }

    public void RemoveItem(Item item, int quantity = 1)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item == item)
            {
                items[i].quantity -= quantity;

                if (items[i].quantity <= 0)
                {
                    items.RemoveAt(i);
                }

                onInventoryChangedCallback?.Invoke();
                return;
            }
        }
    }

    public bool HasItem(Item item, int quantity)
    {
        foreach (var slot in items)
        {
            if (slot.item == item && slot.quantity >= quantity)
            {
                return true;
            }
        }
        return false;
    }

    public int GetItemQuantity(Item item)
    {
        foreach (var slot in items)
        {
            if (slot.item == item)
            {
                return slot.quantity;
            }
        }
        return 0;
    }

    public void UseItem(Item item)
    {
        if (item.IsUsable() && HasItem(item, 1))
        {
            item.Use(); 
            RemoveItem(item, 1);
        }
    }

    public void EquipItem(EquipmentItem equipment)
    {
        EquipmentManager.Instance.Equip(equipment);
    }

    public void AddCurrency(int amount)
    {
        currency += amount;
        onInventoryChangedCallback?.Invoke();
    }

    public bool RemoveCurrency(int amount)
    {
        if (currency >= amount)
        {
            currency -= amount;
            onInventoryChangedCallback?.Invoke();
            return true;
        }
        return false;
    }

    public void SortInventory()
    {
        items.Sort((a, b) =>
        {
            int typeCompare = a.item.itemType.CompareTo(b.item.itemType);
            if (typeCompare != 0) return typeCompare;
            return string.Compare(a.item.itemName, b.item.itemName);
        });

        onInventoryChangedCallback?.Invoke();
    }
}
