using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item;

public class Inventory
{
    public event EventHandler OnItemListChanged;

    private List<Item> itemList;
    private int maxItemSlots = 28;
    private String inventroyFull = "Inventroy is full!";


    public Inventory()
    {
        itemList = new List<Item>();
        Debug.Log("Items: " + itemList.Count);
    }

    public void AddItem(Item item)
    {
        if (item.IsStackable())
        {
            bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
            }
            if (!itemAlreadyInInventory && itemList.Count < maxItemSlots)
            {
                itemList.Add(item);
            } 
            else
            {
                Debug.Log(inventroyFull);
            }
        }
        else if (itemList.Count < maxItemSlots)
        {
            itemList.Add(item);
        } 
        else
        {
            Debug.Log(inventroyFull);
        }

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
        Debug.Log("Items: " + itemList.Count);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }

    public void SellItem(Item item, int currency)
    {
        if (item.IsStackable())
        {
            Item itemInInventory = null;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount -= item.amount;
                    itemInInventory = inventoryItem;
                }
            }
            if (itemInInventory != null && itemInInventory.amount <=0)
            {
                itemList.Remove(itemInInventory);
            }
        }
        else
        {
            itemList.Remove(item);
        }

        currency += ItemValue(item.itemType);

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
        Debug.Log("Items: " + itemList.Count);
    }

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
