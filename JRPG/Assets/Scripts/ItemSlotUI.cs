using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ItemSlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color selectedColor = new Color(0.8f, 0.8f, 0.8f, 1f);

    private InventorySlot slot;
    private InventoryUI inventoryUI;
    private bool isSelected = false;

    public void Setup(InventorySlot slot, InventoryUI inventoryUI)
    {
        this.slot = slot;
        this.inventoryUI = inventoryUI;
        Sprite icon = slot.item.icon;
        if (icon != null)
        {
            GetComponent<Image>().sprite = icon;
        }
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnClick);
        }
    }

    public void OnClick()
    {
        if (slot != null && slot.item != null)
        {
            inventoryUI.SelectItem(slot);
            SetSelected(true);
        }
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnClick();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }

    private void OnRightClick()
    {
        if (slot == null || slot.item == null) return;

        if (slot.item.IsUsable())
        {
            Inventory.Instance.UseItem(slot.item);
        }
        else if (slot.item is EquipmentItem)
        {
            EquipmentManager.Instance.Equip((EquipmentItem)slot.item);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Instance?.ShowTooltip(slot);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance?.HideTooltip();
    }

    public void RefreshSlot()
    {
        if (slot != null)
        {
            Setup(slot, inventoryUI);
        }
    }

    public void ClearSlot()
    {
        if (GetComponent<Image>().sprite != null) GetComponent<Image>().sprite = null;
        slot = null;
    }
}