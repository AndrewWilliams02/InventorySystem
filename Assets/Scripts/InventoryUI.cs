using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;
    [SerializeField] float itemSlotCellSize = 90;
    [SerializeField] float itemsPerRow = 7;

    public TextMeshProUGUI currencyText;
    private int currency = 0;


    private void Awake()
    {
        itemSlotContainer = transform.Find("ItemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("ItemSlotTemplate");

        currencyText.text = $"Currency: ${currency}";
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    private void RefreshInventoryItems()
    {
        foreach(Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }
        
        int x = 0;
        int y = 0;
        foreach (Item item in inventory.GetItemList())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            if (Input.GetMouseButtonDown(1))
            {
                inventory.SellItem(item, currency);
                currencyText.text = $"Currency: ${currency}";
            }

            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
            Image image = itemSlotRectTransform.Find("ItemIcon").GetComponent<Image>();
            image.sprite = item.GetSprite();
            TextMeshProUGUI itemText = itemSlotRectTransform.Find("ItemCount").GetComponent<TextMeshProUGUI>();
            if (item.amount > 1)
            {
                itemText.SetText(item.amount.ToString());
            } else
            {
                itemText.SetText("");
            }

                x--;
            if ((-1) * x >= itemsPerRow)
            {
                x = 0;
                y--;
            }
        }
    }
}
