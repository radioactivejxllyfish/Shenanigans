using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class ItemShop : MonoBehaviour
{

    public int currentTokens;

    [SerializeField] private GameObject shopUI;

    [SerializeField]public GameObject itemDetailsUI;
    public static ItemShop Instance { get; private set; }

    
    private Dictionary<ItemType, int> itemCounts = new Dictionary<ItemType, int>();
    private Dictionary<ItemType, GameObject> itemUIMap = new Dictionary<ItemType, GameObject>();

    [SerializeField] private Transform inventoryListParent; // Parent container in UI
    [SerializeField] private GameObject inventoryItemPrefab; // UI prefab for inventory item

    private bool playerInRange;
    private CircleCollider2D circleCollider2D;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerInRange)
        {
            ToggleInventory();
        }

        if (!playerInRange)
        {
            shopUI.SetActive(false);
        }
        currentTokens = InventoryManager.Instance.GetItemCount(ItemType.Tokens);
    }

    private void ToggleInventory()
    {
        bool isActive = shopUI.activeSelf;
        shopUI.SetActive(!isActive);
    }

    
    private void Start()
    {
        AddItem(ItemType.Medkit, Random.Range(0, 8));
        AddItem(ItemType.Bandage, Random.Range(0, 26));
        AddItem(ItemType.ArmorPlateKevlar, Random.Range(0, 3));
        AddItem(ItemType.ArmorPlateSteel, Random.Range(0, 1));
        AddItem(ItemType.EnergyDrink, Random.Range(0,5));
        circleCollider2D = GetComponent<CircleCollider2D>();
        itemDetailsUI.transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(CloseItemDetails);
        InventoryManager.Instance.AddItem(ItemType.Tokens, Random.Range(4000, 21000));
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player In Range");

        if (collision != null)
        {
            if (collision.gameObject.tag == "PlayerRB")
            {
                playerInRange = true;
                Debug.Log("Player In Range");
            }
        }

    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.tag == "PlayerRB")
            {
                playerInRange = false;
            }
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
        ItemAssets item = Resources.Load<ItemAssets>($"Items/{itemType}");
        if (item == null) return;

        itemDetailsUI.SetActive(true);

        itemDetailsUI.transform.Find("ItemIcon").GetComponent<Image>().sprite = item.sprite;
        itemDetailsUI.transform.Find("ItemName").GetComponent<TextMeshProUGUI>().text = item.itemName;
        itemDetailsUI.transform.Find("ItemDescription").GetComponent<TextMeshProUGUI>().text = item.description; 

        itemDetailsUI.transform.Find("BuyButton").GetComponent<Button>().onClick.RemoveAllListeners();
        itemDetailsUI.transform.Find("BuyButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            ItemAssets itemData = Resources.Load<ItemAssets>($"Items/{itemType}");
            BuyItem(itemType, 1, currentTokens, Random.Range(itemData.basePrice * 90/100, itemData.basePrice * 110/100) );
        });
        
        itemDetailsUI.transform.Find("SellButton").GetComponent<Button>().onClick.RemoveAllListeners();
        itemDetailsUI.transform.Find("SellButton").GetComponent<Button>().onClick.AddListener(() =>
        {
            ItemAssets itemData = Resources.Load<ItemAssets>($"Items/{itemType}");
            SellItem(itemType, 1, currentTokens, itemData.basePrice * 70/100);
        });
    }
    
    
    public void CloseItemDetails()
    {
        itemDetailsUI.SetActive(false);
    }


    public void BuyItem(ItemType itemType, int amount, int currentTokens, int cost)
    {
        if (currentTokens >= cost && itemType != ItemType.Tokens && GetItemCount(itemType) > 0)
        {
            InventoryManager.Instance.RemoveItem(ItemType.Tokens, cost);
            InventoryManager.Instance.AddItem(itemType, amount);
            AddItem(ItemType.Tokens, cost);
            RemoveItem(itemType, 1);
            if (GetItemCount(itemType) == 0)
            {
                itemDetailsUI.SetActive(false); 
            }
        }
    }

    public void SellItem(ItemType itemType, int amount, int currentTokens, int cost)
    {
        if (InventoryManager.Instance.GetItemCount(itemType) > 0 && itemType != ItemType.Tokens)
        {
            InventoryManager.Instance.RemoveItem(itemType, 1);
            InventoryManager.Instance.AddItem(ItemType.Tokens, cost);
            RemoveItem(ItemType.Tokens, cost);
            AddItem(itemType, 1);
        }

    }
}
