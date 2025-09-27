using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TooltipManager : MonoBehaviour
{
    public static TooltipManager Instance { get; private set; }
    public GameObject tooltipPanel;
    public TextMeshProUGUI itemTooltipText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            tooltipPanel.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowTooltip(InventorySlot slot)
    {
        Item item = slot.item;
        if (item == null) return;

        itemTooltipText.text = item.itemName + ": "  + slot.quantity + "\n" + item.description;

        string statsText = "";

        if (item is ConsumableItem consumable)
        {
            if (consumable.hpRestore > 0) statsText += $"Восстанавливает HP: {consumable.hpRestore}\n";
            if (consumable.mpRestore > 0) statsText += $"Восстанавливает MP: {consumable.mpRestore}\n";
        }
        else if (item is EquipmentItem equipment)
        {
            if (equipment.attackBonus > 0) statsText += $"Атака: +{equipment.attackBonus}\n";
            if (equipment.defenseBonus > 0) statsText += $"Защита: +{equipment.defenseBonus}\n";
            if (equipment.magicBonus > 0) statsText += $"Магия: +{equipment.magicBonus}\n";
            if (equipment.speedBonus > 0) statsText += $"Скорость: +{equipment.speedBonus}\n";
        }

        statsText += $"\nЦена: {item.buyPrice} зол. | Продажа: {item.sellPrice} зол.";
        itemTooltipText.text += "\n" + statsText;
        itemTooltipText.text += slot.isEquipped ? "\nЭкипировано" : "";

        tooltipPanel.SetActive(true);

        Vector2 mousePosition = Input.mousePosition;
        float offsetX = tooltipPanel.GetComponent<RectTransform>().rect.width / 2;
        float offsetY = tooltipPanel.GetComponent<RectTransform>().rect.height / 2;
        if (mousePosition.x > Screen.width / 2)
            offsetX *= -1;
        if (mousePosition.y > Screen.height / 2)
            offsetY *= -1;
        tooltipPanel.transform.position = new Vector2(
            mousePosition.x + offsetX,
            mousePosition.y + offsetY
        );
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().Equals(SceneManager.GetSceneByBuildIndex(0)) && !tooltipPanel && !itemTooltipText)
        {
            Transform canvas = GameObject.Find("Canvas").transform;
            foreach (RectTransform rt in canvas.GetComponentsInChildren<RectTransform>())
            {
                GameObject go = rt.gameObject;
                if (go.CompareTag("Tooltip"))
                {
                    tooltipPanel = go;
                    break;
                }
            }
            if (tooltipPanel)
                itemTooltipText = tooltipPanel.GetComponentInChildren<TextMeshProUGUI>();
            HideTooltip();
        }
    }
}