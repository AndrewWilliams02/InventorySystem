using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static Item;

public class InventoryUI : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;

    [SerializeField] private GameObject weaponSlot, armorSlot;
    [SerializeField] private Image weaponIcon, equippedWeaponIcon;
    [SerializeField] private Image armorIcon, equippedArmorIcon;


    [SerializeField] float itemSlotCellSize = 90;
    [SerializeField] float itemsPerRow = 7;

    public TextMeshProUGUI currencyText;
    private int currency = 0;


    private void Awake()
    {
        itemSlotContainer = transform.Find("ItemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("ItemSlotTemplate");

        currencyText.text = $"${currency}";
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
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        int x = 0;
        int y = 0;
        foreach (Item item in inventory.GetItemList())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();

            itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () =>
            {
                EquipItem(item);

                inventory.UseItem(item);
            };
            itemSlotRectTransform.GetComponent<Button_UI>().MouseRightClickFunc = () =>
            {
                int earned = inventory.SellItem(item, currency);
                currency += earned;
                currencyText.text = $"${currency}";
            };

            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
            Image image = itemSlotRectTransform.Find("ItemIcon").GetComponent<Image>();
            image.sprite = item.GetSprite();
            TextMeshProUGUI itemText = itemSlotRectTransform.Find("ItemCount").GetComponent<TextMeshProUGUI>();
            if (item.amount > 1)
            {
                itemText.SetText(item.amount.ToString());
            }
            else
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

    private void EquipItem(Item item)
    {
        if (item.itemType == Item.ItemType.Weapon && inventory.weaponEquip == null)
        {
            equippedWeaponIcon.sprite = item.GetSprite();
            equippedWeaponIcon.gameObject.SetActive(true);
            weaponIcon.gameObject.SetActive(false);
            RefreshInventoryItems();
        }
        else if (item.itemType == Item.ItemType.Armor && inventory.armorEquip == null)
        {
            equippedArmorIcon.sprite = item.GetSprite();
            equippedArmorIcon.gameObject.SetActive(true);
            armorIcon.gameObject.SetActive(false);
            RefreshInventoryItems();
        }
        else
        {
            Debug.Log("Equipment Slot Full!");
        }
    }

    public void UnequipItem(int index)
    {
        if (index == 0 && inventory.weaponEquip != null)
        {
            equippedWeaponIcon.sprite = null;
            equippedWeaponIcon.gameObject.SetActive(false);
            weaponIcon.gameObject.SetActive(true);
            inventory.AddItem(inventory.weaponEquip);
            inventory.weaponEquip = null;
            RefreshInventoryItems();
        }
        else if (index == 1 && inventory.armorEquip != null)
        {
            equippedArmorIcon.sprite = null;
            equippedArmorIcon.gameObject.SetActive(false);
            armorIcon.gameObject.SetActive(true);
            inventory.AddItem(inventory.armorEquip);
            inventory.armorEquip = null;
            RefreshInventoryItems();
        }
    }

    public void BuyItem(int index)
    {
        int cost = 0;
        Item.ItemType itemType = ItemType.HealthPotion;

        switch (index)
        {
            case 0:
                cost = 40;
                itemType = ItemType.Weapon;
                break;
            case 1:
                cost = 40;
                itemType = ItemType.Armor;
                break;
            case 2:
                cost = 10;
                itemType = ItemType.HealthPotion;
                break;
            case 3:
                cost = 20;
                itemType = ItemType.CritPotion;
                break;
            case 4:
                cost = 20;
                itemType = ItemType.DamageReductionPotion;
                break;
            case 5:
                cost = 20;
                itemType = ItemType.DamageBuffPotion;
                break;
        }

        if (currency >= cost)
        {
            inventory.AddItem(new Item { itemType = itemType, amount = 1 });
            currency -= cost;
        }
        else
        {
            Debug.Log("Not Enough Currency!");
        }

        currencyText.text = $"${currency}";
    }
}
