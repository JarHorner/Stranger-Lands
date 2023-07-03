using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour
{
    #region Variables
    [Header("UI Stuff to change")]
    [SerializeField] private TextMeshProUGUI itemNumberInventoryText;
    [SerializeField] private Image itemImage;

    [Header("Variables from the item")]
    public InventoryItem thisItem;
    public InventoryMenu inventoryMenu;
    #endregion

    #region Methods

    private void OnEnable() 
    {
        if (thisItem != null && !thisItem.unique)
        {
            int numHeld; 
            int.TryParse(itemNumberInventoryText.text, out numHeld);
            Debug.Log(numHeld);
            if (thisItem.numberHeld != numHeld)
            {
                itemNumberInventoryText.text = "" + thisItem.numberHeld;
            }
        }
    }
    
    //puts item in inventory (sprite, number held, etc.) and becomes clickable.
    public void Setup(InventoryItem item, InventoryMenu manager)
    {
        if (thisItem != null && !thisItem.unique)
        {
            int numHeld = thisItem.numberHeld;
            itemNumberInventoryText.text = "" + numHeld;
        }
        else
        {
            thisItem = item;
            inventoryMenu = manager;
            if (thisItem)
            {
                itemImage.GetComponent<Image>().enabled = true;
                itemImage.sprite = thisItem.itemImage;
                itemImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                thisItem.playerOwns = true;
                if (!thisItem.unique)
                {
                    int numHeld = thisItem.numberHeld;
                    itemNumberInventoryText.text = "" + numHeld;
                }
            }
        }
    }

    //when clicked on, shows the name and description of item.
    public void ShowDescription(GameObject itemText)
    {
        if (thisItem)
        {
            inventoryMenu.SetTextAndButton(thisItem.itemDescription, thisItem.itemName, thisItem, itemText);
        }
    }

    #endregion
}
