using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public Item item;
    public int quantity;
    public bool isEquipped;

    public InventorySlot(Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
        this.isEquipped = false;
    }
}