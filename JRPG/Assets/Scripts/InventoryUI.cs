using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryUI;
    public Transform itemsContainer;
    public GameObject itemSlotPrefab;
    public TextMeshProUGUI currencyText;
    public static GameObject Canvas;

    public Button useButton;
    public Button equipButton;
    public Button discardButton;
    
    private InventorySlot selectedItem;

    private void Awake()
    {
        if (Canvas && Canvas.CompareTag("MainCanvas"))
        { 
            foreach (Canvas canvas in FindObjectsOfType<Canvas>())
            {
                if (!canvas.CompareTag("MainCanvas"))
                {
                    canvas.gameObject.SetActive(false);
                    break;
                }
            }
        }
        else if (Canvas == null)
        {
            Canvas = transform.GetComponentInParent<Canvas>().gameObject;
            DontDestroyOnLoad(Canvas);
        }
    }

    void Start()
    {
        Canvas.gameObject.tag = "MainCanvas";
        Inventory.Instance.onInventoryChangedCallback += UpdateUI;
        UpdateUI();
        inventoryUI.SetActive(false);
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().Equals(SceneManager.GetSceneByBuildIndex(0)) && Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    public void ToggleInventory()
    {
        inventoryUI.SetActive(!inventoryUI.activeSelf);
        if (inventoryUI.activeSelf)
        {
            UpdateUI();
        }
        else TooltipManager.Instance?.HideTooltip();
    }

    void UpdateUI()
    {
        foreach (Transform child in itemsContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var slot in Inventory.Instance.items)
        {
            GameObject itemSlot = Instantiate(itemSlotPrefab, itemsContainer);
            ItemSlotUI slotUI = itemSlot.GetComponent<ItemSlotUI>();

            slotUI.Setup(slot, this);
        }

        currencyText.text = $"Золото: {Inventory.Instance.currency}";

        UpdateButtonInteractivity();
    }

    public void SelectItem(InventorySlot item)
    {
        selectedItem = item;
        UpdateButtonInteractivity();
    }

    void UpdateButtonInteractivity()
    {
        useButton.interactable = selectedItem != null && selectedItem.item.IsUsable();
        equipButton.interactable = selectedItem != null && selectedItem.item is EquipmentItem;
        discardButton.interactable = selectedItem != null && SceneManager.GetActiveScene().Equals(SceneManager.GetSceneByBuildIndex(0));
    }

    public void OnUseButton()
    {
        if (selectedItem != null)
        {
            Inventory.Instance.UseItem(selectedItem.item);
            if (SceneManager.GetActiveScene().Equals(SceneManager.GetSceneByBuildIndex(1)))
            {
                ToggleInventory();
                BattleSystem.Instance.state = BattleState.ENEMYTURN;
                BattleSystem.Instance.BattlePanel.SetActive(true);
            }
        }
    }

    public void OnEquipButton()
    {
        if (selectedItem.item is EquipmentItem equipment)
        {
            selectedItem.isEquipped = selectedItem.isEquipped ? false : true;
            EquipmentManager.Instance.Equip(equipment);
            if (SceneManager.GetActiveScene().Equals(SceneManager.GetSceneByBuildIndex(1)))
            {
                ToggleInventory();
                BattleSystem.Instance.state = BattleState.ENEMYTURN;
                BattleSystem.Instance.BattlePanel.SetActive(true);
            }
        }
    }

    public void OnDiscardButton()
    {
        if (selectedItem != null)
        {
            Inventory.Instance.RemoveItem(selectedItem.item, 1);
            selectedItem = null;
            UpdateButtonInteractivity();
        }
    }

    public void OnSortButton()
    {
        Inventory.Instance.SortInventory();
    }
}