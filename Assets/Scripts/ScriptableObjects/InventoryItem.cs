using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Basic structure of an Item, can be used to create all items.
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Items")]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    public string itemDescription;
    public Sprite itemImage;
    public int numberHeld;
    public int maxNumberHeld;
    public bool usable;
    public bool unique;
    public bool playerOwns;
    public UnityEvent thisEvent;

    //used event assigned to this item
    public void Use()
    {
        Debug.Log("Using Item");
        thisEvent.Invoke();
    }
}
