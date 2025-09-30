using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using static Item;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public Inventory inventory;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private TextMeshProUGUI stats;
    [SerializeField] private GameObject HUD;
    private bool hudEnabled = true;

    private List<Item.ItemType> possibleItems = new List<Item.ItemType>();

    int HP=0, Dmg=0, DmgRed=0, Crit=0;

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
                Crit += 1;
                UpdateStats();
                break;
            case ItemType.DamageReductionPotion:
                Debug.Log("Player Used Damage Reduction Potion!");
                inventory.RemoveItem(new Item { itemType = item.itemType, amount = 1 });
                DmgRed += 2;
                UpdateStats();
                break;
            case ItemType.DamageBuffPotion:
                Debug.Log("Player Used Damage Buff Potion!");
                inventory.RemoveItem(new Item { itemType = item.itemType, amount = 1 });
                Dmg += 5;
                UpdateStats();
                break;
            case ItemType.Weapon:
                if (inventory.weaponEquip == null)
                {
                    Debug.Log("Item Has Been Equiped!");
                    inventory.EquipItem(item);
                    inventory.RemoveItem(item);
                    Dmg += 15;
                    Crit += 5;
                    UpdateStats();
                }
                break;
            case ItemType.Armor:
                if (inventory.armorEquip == null)
                {
                    Debug.Log("Item Has Been Equiped!");
                    inventory.EquipItem(item);
                    inventory.RemoveItem(item);
                    HP += 25;
                    DmgRed += 20;
                    UpdateStats();
                }
                break;
        }
    }

    public void GiveRandomItem()
    {
        int randomIndex = Random.Range(0, 5);

        inventory.AddItem(new Item { itemType = possibleItems[randomIndex], amount = 1 });
    }

    public void UpdateStats()
    {
        stats.text = $"HP: +{HP}\nDamage Reduction: +{DmgRed}%\nDamage: +{Dmg}\nCrit Chance: +{Crit}%";
    }

    public void UnequipStatChange(int index)
    {
        if (index == 0 && inventory.weaponEquip != null)
        {
            Dmg -= 15;
            Crit -= 5;
        }
        else if (index == 1 && inventory.armorEquip != null)
        {
            HP -= 25;
            DmgRed -= 20;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (hudEnabled)
            {
                HUD.SetActive(false);
                hudEnabled = false;
            }
            else
            {
                HUD.SetActive(true);
                hudEnabled = true;
            }
        }
    }
}

