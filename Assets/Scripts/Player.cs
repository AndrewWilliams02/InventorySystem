using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using static Item;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Inventory inventory;
    [SerializeField] private InventoryUI inventoryUI;

    private List<Item.ItemType> possibleItems = new List<Item.ItemType>();

    void Awake()
    {
        inventory = new Inventory(UseItem);
        inventoryUI.SetInventory(inventory);

        possibleItems.Add(Item.ItemType.Weapon);
        possibleItems.Add(Item.ItemType.Armor);
        possibleItems.Add(Item.ItemType.HealthPotion);
        possibleItems.Add(Item.ItemType.CritPotion);
        possibleItems.Add(Item.ItemType.DamageReductionPotion);
        possibleItems.Add(Item.ItemType.DamageBuffPotion);
    }

    private void UseItem(Item item)
    {
        switch (item.itemType)
        {
            default:
            case ItemType.HealthPotion:
                Debug.Log("Player Used Health Potion!");
                inventory.RemoveItem(new Item { itemType = item.itemType, amount = 1 });
                break;
            case ItemType.CritPotion:
                Debug.Log("Player Used Crit Potion!");
                inventory.RemoveItem(new Item { itemType = item.itemType, amount = 1 });
                break;
            case ItemType.DamageReductionPotion:
                Debug.Log("Player Used Damage Reduction Potion!");
                inventory.RemoveItem(new Item { itemType = item.itemType, amount = 1 });
                break;
            case ItemType.DamageBuffPotion:
                Debug.Log("Player Used Damage Buff Potion!");
                inventory.RemoveItem(new Item { itemType = item.itemType, amount = 1 });
                break;
            case ItemType.Weapon:
            case ItemType.Armor:
                Debug.Log("Item Cannot Be Used!");
                break;
        }
    }

    public void GiveRandomItem()
    {
        int randomIndex = Random.Range(0, 5);

        inventory.AddItem(new Item { itemType = possibleItems[randomIndex], amount = 1 });
    }
}

