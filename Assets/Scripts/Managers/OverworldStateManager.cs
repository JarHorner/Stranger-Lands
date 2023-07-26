using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldStateManager : MonoBehaviour
{
    #region Variables
    //Key: chest number, Value: is chest open
    private List<MutableKeyValPair<int, bool>> chests = new List<MutableKeyValPair<int, bool>>();
    //Key: key number, Value: is key collected
    private List<MutableKeyValPair<int, bool>> keys = new List<MutableKeyValPair<int, bool>>();
    private int currentKeys;
    #endregion

    #region Unity Methods

    void Awake()
    {
        currentKeys = 0;
    }

    //adds a new chest to stay opened to list, used in OpenChest Update function when player unlocks door
    public void AddChestStayOpen(int chestNum)
    {
        chests.Add(new MutableKeyValPair<int, bool>(chestNum, true));
    }

    //checks to see if chestNum is in list, if not, chest will not be opened when scene loads
    public bool GetChestStayOpen(int chestNum)
    {
        foreach (var item in chests)
        {
            if (item.key == chestNum)
            {
                return item.value;
            }
        }
        return false;
    }

    //adds a new key to stay destroyed to list, used in Key OnTriggerEnter2D function when player unlocks door
    public void AddKeyStayDestoryed(int keyNum)
    {
        keys.Add(new MutableKeyValPair<int, bool>(keyNum, true));
    }

    //checks to see if keyNum is in list, if not, key will not be destroyed on opening the scene.
    public bool GetKeyStayDestroyed(int keyNum)
    {
        foreach (var item in keys)
        {
            if (item.key == keyNum)
            {
                return item.value;
            }
        }
        return false;
    }

    public int CurrentKeys
    {
        get { return currentKeys; }
        set { currentKeys = value; }
    }

    #endregion
}
