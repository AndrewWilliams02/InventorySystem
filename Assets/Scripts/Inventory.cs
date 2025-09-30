using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Item;

public class Inventory
{
    //Event used to check when inventory list is changed to refresh UI
    public event EventHandler OnItemListChanged;

    private Action<Item> useItemAction; //Handles the actions from using an item
    private List<Item> itemList; //List of items in the inventory

    //Handles max inventory slots and max capacity message
    private int maxItemSlots = 28;
    private String inventroyFull = "Inventroy is full!";

    //Handles the currently equipped items
    public Item weaponEquip; 
    public Item armorEquip;


    //Initializes inventory and use item action
    public Inventory(Action<Item> useItemAction)
    {
        this.useItemAction = useItemAction;
        itemList = new List<Item>();
        Debug.Log($"Items: {itemList.Count}");
    }

    //Function to add items to the inventory by conditions
    public void AddItem(Item item)
    {
        //Checks if item is stackable
        if (item.IsStackable())
        {
            bool itemAlreadyInInventory = false; //Check for if stackable item is already in inventory
            foreach (Item inventoryItem in itemList)
            {
                //If item is already in inventory, increase the amount of said item in inventory list
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
            }

            //If item is not in inventory, add it to the inventory list unless inventory is full
            if (!itemAlreadyInInventory && itemList.Count < maxItemSlots)
            {
                itemList.Add(item);
            } 
            else if (!itemAlreadyInInventory && itemList.Count > maxItemSlots)
            {
                Debug.Log(inventroyFull);
            }
        }
        //If item isn't stackable and inventory is not full, add item to inventory list
        else if (itemList.Count < maxItemSlots)
        {
            itemList.Add(item);
        } 
        else
        {
            Debug.Log(inventroyFull);
        }

        OnItemListChanged?.Invoke(this, EventArgs.Empty); //Trigger event to refresh inventory
        Debug.Log($"Items: {itemList.Count}");
    }

    //Function for getting item list
    public List<Item> GetItemList()
    {
        return itemList;
    }

    //Function to sell items for currency depending on item
    public int SellItem(Item item, int currency)
    {
        RemoveItem(item);
        currency = ItemValue(item.itemType);
        return currency;
    }

    //Function to remove item from inventory list
    public void RemoveItem(Item item)
    {
        //Checks if item is stackable
        if (item.IsStackable())
        {
            Item itemInInventory = null; //Variable to temporarily hold the item if found
            foreach (Item inventoryItem in itemList)
            {
                //If item is stackable and exists in the inventory, removes 1 from items amount in list
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount -= 1;
                    itemInInventory = inventoryItem;
                }
            }

            //If their is no more of the item left in the inventory remove its slot from list
            if (itemInInventory != null && itemInInventory.amount <= 0)
            {
                itemList.Remove(itemInInventory);
            }
        }
        //If the item isnt stackable remove its slot from list
        else
        {
            itemList.Remove(item);
        }

        OnItemListChanged?.Invoke(this, EventArgs.Empty); //Trigger event to refresh inventory
    }

    //Function used to set item action functionality depending on item
    public void UseItem(Item item)
    {
        useItemAction(item);
    }

    //Function that checks the item type of the item being equipped then adds it to the proper equipment slot
    public void EquipItem(Item item)
    {
        switch (item.itemType)
        {
            default:
            case ItemType.Weapon:
                weaponEquip = item;
                break;
            case ItemType.Armor:
                armorEquip = item;
                break;
        }
    }

    //Function to check which item is being sold and set its item value for selling
    private int ItemValue(Item.ItemType itemType)
    {
        switch(itemType)
        {
            default:
            case ItemType.HealthPotion: 
                return 5;
            case ItemType.CritPotion:
            case ItemType.DamageReductionPotion:
            case ItemType.DamageBuffPotion: 
                return 10;
            case ItemType.Armor:
            case ItemType.Weapon: 
                return 20;
        }
    }
}
