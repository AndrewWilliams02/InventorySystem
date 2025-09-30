using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    //Enum list of different item types
    public enum ItemType
    {
        Armor,
        Weapon,
        HealthPotion,
        CritPotion,
        DamageReductionPotion,
        DamageBuffPotion,
    }

    //Handles item type variables and count of said item
    public ItemType itemType;
    public int amount;

    //Function that checks for item type then returns the proper sprite for said item
    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.Weapon:                   return ItemAssets.Instance.weaponSprite;
            case ItemType.Armor:                    return ItemAssets.Instance.armorSprite;
            case ItemType.HealthPotion:             return ItemAssets.Instance.healthPotionSprite;
            case ItemType.CritPotion:               return ItemAssets.Instance.critPotionSprite;
            case ItemType.DamageReductionPotion:    return ItemAssets.Instance.damageReductionPotionSprite;
            case ItemType.DamageBuffPotion:         return ItemAssets.Instance.damageBuffPotionSprite;
        }
    }

    //Function that defines which items can be stacked and which cant
    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.HealthPotion:
            case ItemType.CritPotion:
            case ItemType.DamageReductionPotion:
            case ItemType.DamageBuffPotion:
                return true;
            case ItemType.Weapon:
            case ItemType.Armor:
                return false;
        }
    }
}
