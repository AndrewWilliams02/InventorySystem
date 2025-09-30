using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using static Item;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public Inventory inventory; //Refrence to the inventory class
    [SerializeField] private InventoryUI inventoryUI; //Refrence to inventoryUI class
    [SerializeField] private TextMeshProUGUI stats; //Refrence to player stats UI
    [SerializeField] private GameObject HUD; //Refrence to inventory/shop UI
    private bool hudEnabled = true; //Handles whether inventory/shop UI is visible

    private List<Item.ItemType> possibleItems = new List<Item.ItemType>(); //Handles possible random item types

    int HP=0, Dmg=0, DmgRed=0, Crit=0; //Handles player stat values

    //Function that initiates variables
    void Awake()
    {
        inventory = new Inventory(UseItem); //Initiates player inventory
        inventoryUI.SetInventory(inventory); //Sets inventory to inventoryUI class

        //Initiates all possible random item types
        possibleItems.Add(Item.ItemType.Weapon);
        possibleItems.Add(Item.ItemType.Armor);
        possibleItems.Add(Item.ItemType.HealthPotion);
        possibleItems.Add(Item.ItemType.CritPotion);
        possibleItems.Add(Item.ItemType.DamageReductionPotion);
        possibleItems.Add(Item.ItemType.DamageBuffPotion);
    }

    //Function that checks the type of item and sets the action from using said item
    private void UseItem(Item item)
    {
        switch (item.itemType)
        {
            default:
            case ItemType.HealthPotion: //If item is health potion currently does nothing
                Debug.Log("Player Used Health Potion!");
                inventory.RemoveItem(new Item { itemType = item.itemType, amount = 1 });
                break;
            case ItemType.CritPotion: //If item is crit potion increases crit rate stat
                Debug.Log("Player Used Crit Potion!");
                inventory.RemoveItem(new Item { itemType = item.itemType, amount = 1 });
                Crit += 1;
                UpdateStats();
                break;
            case ItemType.DamageReductionPotion: //If item is damage reduction potion increases damage reduction stat
                Debug.Log("Player Used Damage Reduction Potion!");
                inventory.RemoveItem(new Item { itemType = item.itemType, amount = 1 });
                DmgRed += 2;
                UpdateStats();
                break;
            case ItemType.DamageBuffPotion: //If item is damage potion increases damage stat
                Debug.Log("Player Used Damage Buff Potion!");
                inventory.RemoveItem(new Item { itemType = item.itemType, amount = 1 });
                Dmg += 5;
                UpdateStats();
                break;
            case ItemType.Weapon: //If item is weapon and the weapon slot is empty, equips item and increases attack stats
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
            case ItemType.Armor: //If item is armor and the armor slot is empty, equips item and increases defensive stats
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

    //Function that adds a random item to player inventory
    public void GiveRandomItem()
    {
        int randomIndex = Random.Range(0, 5);

        inventory.AddItem(new Item { itemType = possibleItems[randomIndex], amount = 1 });
    }

    //Function that updates player stats UI
    public void UpdateStats()
    {
        stats.text = $"HP: +{HP}\nDamage Reduction: +{DmgRed}%\nDamage: +{Dmg}\nCrit Chance: +{Crit}%";
    }

    //Function that checks if an equipment slot of the matching index is full and then decreases stats if so
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

    //Function to check for key imputs
    public void Update()
    {
        //Either disables or enables inventory/shop UI depending on state
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

