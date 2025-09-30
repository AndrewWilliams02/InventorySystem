using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Item;

public class Inventory
{
    public event EventHandler OnItemListChanged;

    private Action<Item> useItemAction;
    private List<Item> itemList;
    public Item weaponEquip;
    public Item armorEquip;
    private int maxItemSlots = 28;
    private String inventroyFull = "Inventroy is full!";


    public Inventory(Action<Item> useItemAction)
    {
        this.useItemAction = useItemAction;
        itemList = new List<Item>();
        Debug.Log($"Items: {itemList.Count}");
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
        Debug.Log($"Items: {itemList.Count}");
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }

    public int SellItem(Item item, int currency)
    {
        RemoveItem(item);
        currency = ItemValue(item.itemType);
        return currency;
    }

    public void RemoveItem(Item item)
    {
        if (item.IsStackable())
        {
            Item itemInInventory = null;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount -= 1;
                    itemInInventory = inventoryItem;
                }
            }
            if (itemInInventory != null && itemInInventory.amount <= 0)
            {
                itemList.Remove(itemInInventory);
            }
        }
        else
        {
            itemList.Remove(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UseItem(Item item)
    {
        useItemAction(item);
    }

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
