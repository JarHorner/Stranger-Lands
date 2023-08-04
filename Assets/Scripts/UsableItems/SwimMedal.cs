using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SwimMedal : MonoBehaviour
{
    [SerializeField] TilemapCollider2D waterCollider;
    [SerializeField] GameObject outOfWaterCollider;
    public void AllowSwimming()
    {
        waterCollider.isTrigger = true;
        outOfWaterCollider.tag = "OutOfWater";
    }
}
