using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Water : MonoBehaviour
{
    private InventoryMenu inventoryMenu;
    [SerializeField] TilemapCollider2D waterCollider;
    
    void Start()
    {
        inventoryMenu = FindObjectOfType<InventoryMenu>();

        if (inventoryMenu.HasSwimMedal())
        {
            waterCollider.isTrigger = true;
        }
    }
}
