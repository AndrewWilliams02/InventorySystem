using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    //Creates an instance for item assets
    public static ItemAssets Instance
    {
        get; private set;
    }
    private void Awake()
    {
        Instance = this;
    }

    //Refrences to sprites for items
    public Sprite armorSprite;
    public Sprite weaponSprite;
    public Sprite healthPotionSprite;
    public Sprite critPotionSprite;
    public Sprite damageReductionPotionSprite;
    public Sprite damageBuffPotionSprite;
}
