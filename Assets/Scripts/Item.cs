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

    public ItemType itemtype;
    public int amount;

    public Sprite GetSprite()
    {
        switch (itemtype)
        {
            default:
            case ItemType.Armor:                    return ItemAssets.Instance.armorSprite;
            case ItemType.Weapon:                   return ItemAssets.Instance.weaponSprite;
            case ItemType.HealthPotion:             return ItemAssets.Instance.healthPotionSprite;
            case ItemType.CritPotion:               return ItemAssets.Instance.critPotionSprite;
            case ItemType.DamageReductionPotion:    return ItemAssets.Instance.damageReductionPotionSprite;
            case ItemType.DamageBuffPotion:         return ItemAssets.Instance.damageBuffPotionSprite;
        }
    }
}
