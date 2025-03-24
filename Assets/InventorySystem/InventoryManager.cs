using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    [SerializeField] private GameObject inventoryUI;

    [SerializeField]public GameObject itemDetailsUI;
    public static InventoryManager Instance { get; private set; }

    private Dictionary<ItemType, int> itemCounts = new Dictionary<ItemType, int>();
    private Dictionary<ItemType, GameObject> itemUIMap = new Dictionary<ItemType, GameObject>();

    [SerializeField] private Transform inventoryListParent; // Parent container in UI
    [SerializeField] private GameObject inventoryItemPrefab; // UI prefab for inventory item

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    private void ToggleInventory()
    {
        bool isActive = inventoryUI.activeSelf;
        inventoryUI.SetActive(!isActive);
    }

    
    private void Start()
    {
        itemDetailsUI.transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(CloseItemDetails);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
    }

    public void AddItem(ItemType itemType, int amount)
    {
        if (itemCounts.ContainsKey(itemType))
        {
            itemCounts[itemType] += amount;
            UpdateItemUI(itemType);
        }
        else
        {
            itemCounts[itemType] = amount;
            CreateItemUI(itemType);
        }
    }

    public void RemoveItem(ItemType itemType, int amount)
    {
        if (itemCounts.ContainsKey(itemType))
        {
            itemCounts[itemType] -= amount;
            if (itemCounts[itemType] <= 0)
            {
                itemCounts.Remove(itemType);
                Destroy(itemUIMap[itemType]); // Remove from UI
                itemUIMap.Remove(itemType);
            }
            else
            {
                UpdateItemUI(itemType);
            }
        }
    }

    private void CreateItemUI(ItemType itemType)
    {
        GameObject newItem = Instantiate(inventoryItemPrefab, inventoryListParent);
        newItem.transform.Find("ItemIcon").GetComponent<Image>().sprite = GetItemSprite(itemType);
        newItem.transform.Find("ItemName").GetComponent<TMPro.TextMeshProUGUI>().text = itemType.ToString();
        newItem.transform.Find("ItemCount").GetComponent<TMPro.TextMeshProUGUI>().text = itemCounts[itemType].ToString();

        newItem.GetComponent<Button>().onClick.AddListener(() => OnInventoryItemClicked(itemType));
        
        itemUIMap[itemType] = newItem;
    }

    private void UpdateItemUI(ItemType itemType)
    {
        if (itemUIMap.ContainsKey(itemType))
        {
            itemUIMap[itemType].transform.Find("ItemCount").GetComponent<TMPro.TextMeshProUGUI>().text = itemCounts[itemType].ToString();
        }
    }

    private Sprite GetItemSprite(ItemType itemType)
    {
        ItemAssets itemAsset = Resources.Load<ItemAssets>($"Items/{itemType}");
        return itemAsset != null ? itemAsset.sprite : null;
    }

    public int GetItemCount(ItemType itemType)
    {
        return itemCounts.TryGetValue(itemType, out int count) ? count : 0;
    }
    
    public void OnInventoryItemClicked(ItemType itemType)
    {
        // Retrieve the actual item data from the resources
        ItemAssets item = Resources.Load<ItemAssets>($"Items/{itemType}");
        if (item == null) return;

        // Show the item details UI
        itemDetailsUI.SetActive(true);

        // Set the UI elements
        itemDetailsUI.transform.Find("ItemIcon").GetComponent<Image>().sprite = item.sprite;
        itemDetailsUI.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = item.itemName;
        itemDetailsUI.transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text = item.description; // Add a real description

        // Setup the "Use" and "Discard" buttons
        itemDetailsUI.transform.Find("UseButton").GetComponent<Button>().onClick.RemoveAllListeners();
        // itemDetailsUI.transform.Find("UseButton").GetComponent<Button>().onClick.AddListener(() => UseItem(item)); 

        itemDetailsUI.transform.Find("DiscardButton").GetComponent<Button>().onClick.RemoveAllListeners();
        itemDetailsUI.transform.Find("DiscardButton").GetComponent<Button>().onClick.AddListener(() => RemoveItem(itemType, 1));
    }

    
    public void CloseItemDetails()
    {
        itemDetailsUI.SetActive(false);
    }

}
