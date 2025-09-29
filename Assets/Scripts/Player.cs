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
        inventory = new Inventory();
        inventoryUI.SetInventory(inventory);

        possibleItems.Add(Item.ItemType.Weapon);
        possibleItems.Add(Item.ItemType.Armor);
        possibleItems.Add(Item.ItemType.HealthPotion);
        possibleItems.Add(Item.ItemType.CritPotion);
        possibleItems.Add(Item.ItemType.DamageReductionPotion);
        possibleItems.Add(Item.ItemType.DamageBuffPotion);
    }

    public void GiveRandomItem()
    {
        int randomIndex = Random.Range(0, 5);

        inventory.AddItem(new Item { itemType = possibleItems[randomIndex], amount = 1 });
    }
}

