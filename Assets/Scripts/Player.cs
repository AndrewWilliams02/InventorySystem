using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;
using static Item;

public class Player : MonoBehaviour
{
    private Inventory inventory;
    [SerializeField] private InventoryUI inventoryUI;

    void Awake()
    {
        inventory = new Inventory();
        inventoryUI.SetInventory(inventory);
    }
}

