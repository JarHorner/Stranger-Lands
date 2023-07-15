using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DungeonManager
{
    #region Variables
    private bool isDungeonOpened;
    private bool hasMap;
    private bool hasBossKey;
    //Key: door number, Value: is door open
    private List<MutableKeyValPair<int, bool>> keyDoors = new List<MutableKeyValPair<int, bool>>();
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
        isDungeonOpened = false;
        hasMap = false;
        hasBossKey = false;
    }

    //adds a new door to stay opened to list, used in OpenKeyDoor when player unlocks door
    public void AddDoorStayOpen(int doorNum)
    {
        keyDoors.Add(new MutableKeyValPair<int, bool>(doorNum, true));
    }

    //checks to see if doorNum is in list, if not, door will not be opened when scene loads
    public bool GetDoorStayOpen(int doorNum)
    {
        foreach (var item in keyDoors)
        {
            if (item.key == doorNum)
            {
                return item.value;
            }
        }
        return false;
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

    public bool IsDungeonOpened
    {
        get { return isDungeonOpened; }
        set { isDungeonOpened = value; }
    }

    public bool HasMap
    {
        get { return hasMap; }
        set { hasMap = value; }
    }

    public bool HasBossKey
    {
        get { return hasBossKey; }
        set { hasBossKey = value; }
    }
    #endregion
}
