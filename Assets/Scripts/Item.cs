using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType
    {
        Armor,
        Weapon,
        HealthPotion,
        CritPotion,
        DamageReductionPotion,
        DamageBuffPotion,
    }

    public ItemType itemType;
    public int amount;

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
