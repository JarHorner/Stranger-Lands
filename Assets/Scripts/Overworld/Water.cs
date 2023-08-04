using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Water : MonoBehaviour
{
    private InventoryMenu inventoryMenu;
    [SerializeField] TilemapCollider2D waterCollider;
    [SerializeField] GameObject outOfWaterCollider;
    
    void Start()
    {
        inventoryMenu = FindObjectOfType<InventoryMenu>();

        if (inventoryMenu.HasSwimMedal())
        {
            waterCollider.isTrigger = true;
            outOfWaterCollider.tag = "OutOfWater";
            enabled = false;
        }
    }

    void Update()
    {
        if (inventoryMenu.HasSwimMedal())
        {
            waterCollider.isTrigger = true;
            outOfWaterCollider.tag = "OutOfWater";
            outOfWaterCollider.GetComponent<TilemapCollider2D>().enabled = false;
            enabled = false;
        }
    }
}
