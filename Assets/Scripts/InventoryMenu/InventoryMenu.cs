using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InventoryMenu : MonoBehaviour
{
    #region Variables    
    private static bool exists;

    [Header("Inventory Information")]
    public PlayerInventory playerInventory;
    public PlayerInventory usableItems;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private GameObject itemBox1;
    [SerializeField] private GameObject itemBox2;
    public List<InventorySlot> myInventorySlots = new List<InventorySlot>();
    public InventoryItem currentItem;
    #endregion

    #region Methods

    //gets the item boxes and populates some items in inventory. populated items will be removed eventually.
    void Awake()
    {
        PopulateInventorySlot("Sword");
        PopulateInventorySlot("WoodShield");
        PopulateInventorySlot("LeatherArmor");

        //PopulateInventorySlot("SwimMedal");
        
        //PopulateInventorySlot("Lanturn");
        //PopulateInventorySlot("Bomb");
        // PopulateInventorySlot("Bow");
        //PopulateInventorySlot("Earthquake");

        //Singleton Effect
        if (!exists)
        {
            exists = true;
            //ensures same player object is not destoyed when loading new scences
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // if game was loaded, ensures the item slots are loaded with items the player already has
        if (SaveSystem.LoadedGame)
        {
            LoadItemsToSlots();
        }
    }

    //Sets the text and description to certain item. Used in InventorySlot when clicking on button.
    public void SetTextAndButton(string description, string name, InventoryItem newItem, GameObject itemText)
    {
        if (itemText)
        {
            itemText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
            itemText.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = description;
        }
        currentItem = newItem;
    }

    //used when picking up new item, ex. OpenChest.
    public void PopulateInventorySlot(string itemName)
    {
        if (playerInventory)
        {
            for (int i = 0; i < playerInventory.myInventory.Count; i++)
            {
                if (playerInventory.myInventory[i] == null)
                {
                    //Move on, will delete later
                }
                else if (playerInventory.myInventory[i].ToString().Contains(itemName))
                {
                    myInventorySlots[i].Setup(playerInventory.myInventory[i], this);
                    break;
                }
            }
        }
    }

    // if game was loaded, ensures the useable item slots are loaded with items the player was using when last played
    private void LoadItemsToSlots()
    {
        InventoryItem itemInSlot1 = usableItems.myInventory[SaveSystem.CurrentPlayerData.item1];
        InventoryItem itemInSlot2 = usableItems.myInventory[SaveSystem.CurrentPlayerData.item2];

        if (itemInSlot1 != null)
        {
            itemBox1.transform.GetChild(0).GetComponent<Image>().enabled = true;
            itemBox1.transform.GetChild(0).GetComponent<Image>().sprite = itemInSlot1.itemImage;
            if (itemInSlot1.numberHeld > -1)
                itemBox1.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = itemInSlot1.numberHeld.ToString();
            else
                itemBox1.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
        }
        if (itemInSlot2 != null)
        {
            itemBox2.transform.GetChild(0).GetComponent<Image>().enabled = true;
            itemBox2.transform.GetChild(0).GetComponent<Image>().sprite = itemInSlot2.itemImage;
            if (itemInSlot2.numberHeld > -1)
                itemBox2.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = itemInSlot2.numberHeld.ToString();
            else
                itemBox2.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "";
        }
    }

    public void AssignItem(InputAction.CallbackContext context)
    {
        AssignItemToSlot(context);
    }

    //helps assign an item to a button, ensuring 2 of the same item cannot be in the item boxes. once item is assigned, popup is deactivated.
    private void AssignItemToSlot(InputAction.CallbackContext context)
    {
        currentItem = EventSystem.current.currentSelectedGameObject.GetComponent<InventorySlot>().thisItem;
        try
        {
            if (context.action.name.Equals("AssignItem1") && currentItem.usable)
            {
                itemBox1.transform.GetChild(0).GetComponent<Image>().enabled = true;
                itemBox1.transform.GetChild(0).GetComponent<Image>().sprite = currentItem.itemImage;
                if (currentItem.numberHeld > -1)
                    itemBox1.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = currentItem.numberHeld.ToString();
                else
                    itemBox1.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                usableItems.myInventory[0] = currentItem;

                if (usableItems.myInventory[1] == usableItems.myInventory[0])
                {
                    itemBox2.transform.GetChild(0).GetComponent<Image>().sprite = null;
                    itemBox2.transform.GetChild(0).GetComponent<Image>().enabled = false;
                    itemBox2.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                    usableItems.myInventory[1] = null;
                }
            }
            else if (context.action.name.Equals("AssignItem2") && currentItem.usable)
            {
                itemBox2.transform.GetChild(0).GetComponent<Image>().enabled = true;
                itemBox2.transform.GetChild(0).GetComponent<Image>().sprite = currentItem.itemImage;
                if (currentItem.numberHeld > -1)
                    itemBox2.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = currentItem.numberHeld.ToString();
                else
                    itemBox2.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                usableItems.myInventory[1] = currentItem;

                if (usableItems.myInventory[0] == usableItems.myInventory[1])
                {
                    itemBox1.transform.GetChild(0).GetComponent<Image>().sprite = null;
                    itemBox1.transform.GetChild(0).GetComponent<Image>().enabled = false;
                    itemBox1.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                    usableItems.myInventory[0] = null;
                }
            }
        }
        catch (System.Exception)
        {

            Debug.Log("This item does not exist");
        }

    }

    public bool HasSwimMedal()
    {
        foreach (var item in playerInventory.myInventory)
        {
            if (item != null && item.name == "SwimMedal")
            {
                if (item.playerOwns == true)
                    return true;
            }
        }
        return false;
    }

    #endregion
}
