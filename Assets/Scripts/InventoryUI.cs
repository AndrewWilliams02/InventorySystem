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
    private Inventory inventory; //Refrence to the inventory class

    //Refrence to the inventory slots and templater
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;

    //Refrence to equipment slots and images
    [SerializeField] private GameObject weaponSlot, armorSlot;
    [SerializeField] private Image weaponIcon, equippedWeaponIcon;
    [SerializeField] private Image armorIcon, equippedArmorIcon;

    //Tracks maximum item slots per row and x-axis incrementation between items
    [SerializeField] float itemSlotCellSize = 90;
    [SerializeField] float itemsPerRow = 7;

    //Handles currency value and UI
    public TextMeshProUGUI currencyText;
    private int currency = 0;


    //Function to initiate slots and currency UI
    private void Awake()
    {
        itemSlotContainer = transform.Find("ItemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("ItemSlotTemplate");

        currencyText.text = $"${currency}";
    }

    //Function that initiates the players inventory and refreshes UI
    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    //Functiuon that checks for item change event and refreshes UI if triggered
    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        RefreshInventoryItems();
    }

    //Function that refreshes inventory UI
    private void RefreshInventoryItems()
    {
        //destroys all current inventory slot in UI
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }

        //Sets initiale slot position
        int x = 0;
        int y = 0;

        //For each item in the inventory list creates an instantiated slot for the items using the template at the next empty slot position
        foreach (Item item in inventory.GetItemList())
        {
            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();

            //Function that checks if item is left clicked and uses/equips item depending on conditions
            itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () =>
            {
                EquipItem(item);
                inventory.UseItem(item);
            };
            //Function that checks if item is right clicked and sells the item for a set value deleting it from inventory
            itemSlotRectTransform.GetComponent<Button_UI>().MouseRightClickFunc = () =>
            {
                int earned = inventory.SellItem(item, currency);
                currency += earned;
                currencyText.text = $"${currency}";
            };

            //Sets the position for the item on UI
            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);

            //Changes the item sprite to match the list item type
            Image image = itemSlotRectTransform.Find("ItemIcon").GetComponent<Image>();
            image.sprite = item.GetSprite();

            //Alters item text if item is stackable and above 1 amount to show the count
            TextMeshProUGUI itemText = itemSlotRectTransform.Find("ItemCount").GetComponent<TextMeshProUGUI>();
            if (item.amount > 1)
            {
                itemText.SetText(item.amount.ToString());
            }
            else
            {
                itemText.SetText("");
            }

            //Checks if item is past the max items per row and starts the next row if so
            x--;
            if ((-1) * x >= itemsPerRow)
            {
                x = 0;
                y--;
            }
        }
    }

    //Function that checks item type and equips item to slot if the slot is empty
    private void EquipItem(Item item)
    {
        //Checks if item is type weapon and if weapon slot is empty then equips
        if (item.itemType == Item.ItemType.Weapon && inventory.weaponEquip == null)
        {
            equippedWeaponIcon.sprite = item.GetSprite();
            equippedWeaponIcon.gameObject.SetActive(true);
            weaponIcon.gameObject.SetActive(false);
            RefreshInventoryItems();
        }
        //Checks if item is type armor and if armor slot is empty then equips
        else if (item.itemType == Item.ItemType.Armor && inventory.armorEquip == null)
        {
            equippedArmorIcon.sprite = item.GetSprite();
            equippedArmorIcon.gameObject.SetActive(true);
            armorIcon.gameObject.SetActive(false);
            RefreshInventoryItems();
        }
        //States equipment slot is full if conditions above arent met
        else
        {
            Debug.Log("Equipment Slot Full!");
        }
    }

    //Function to unequip weapons and armor if the equipment slot isnt empty
    public void UnequipItem(int index)
    {
        //Checks if the index matches the weapon slot and unequips weapon if there is one present, then resets slot image
        if (index == 0 && inventory.weaponEquip != null)
        {
            equippedWeaponIcon.sprite = null;
            equippedWeaponIcon.gameObject.SetActive(false);
            weaponIcon.gameObject.SetActive(true);
            inventory.AddItem(inventory.weaponEquip);
            inventory.weaponEquip = null;
            RefreshInventoryItems();
        }
        //Checks if the index matches the weapon slot and unequips weapon if there is one present, then resets slot image
        else if (index == 1 && inventory.armorEquip != null)
        {
            equippedArmorIcon.sprite = null;
            equippedArmorIcon.gameObject.SetActive(false);
            armorIcon.gameObject.SetActive(true);
            inventory.AddItem(inventory.armorEquip);
            inventory.armorEquip = null;
            RefreshInventoryItems();
        }
        //States equipment slot is empty if conditions above arent met
        else
        {
            Debug.Log("Equipment Slot Empty!");
        }
    }

    //Function that checks item index then purchases item if player has enough currency
    public void BuyItem(int index)
    {
        //Initiale values
        int cost = 0;
        Item.ItemType itemType = ItemType.HealthPotion;

        //Switch statement that checks index then sets corresponding cost and item type
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

        //Checks if player has enough currency then subtracts the amount while adding item to inventory
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
