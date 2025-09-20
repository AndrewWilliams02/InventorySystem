using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Item;

public class Inventory
{
    private List<Item> itemList;

    public Inventory()
    {
        itemList = new List<Item>();

        AddItem(new Item { itemtype = Item.ItemType.Weapon, amount = 1 });
        AddItem(new Item { itemtype = Item.ItemType.Armor, amount = 1 });
        AddItem(new Item { itemtype = Item.ItemType.HealthPotion, amount = 1 });
        AddItem(new Item { itemtype = Item.ItemType.CritPotion, amount = 1 });
        AddItem(new Item { itemtype = Item.ItemType.DamageReductionPotion, amount = 1 });
        AddItem(new Item { itemtype = Item.ItemType.DamageBuffPotion, amount = 1 });
        Debug.Log("Items: " + itemList.Count);
    }

    public void AddItem(Item item)
    {
        itemList.Add(item);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
}
