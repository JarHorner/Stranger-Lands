using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Located with player under Managers empty object. In this location so it is never destoryed and carried on through each scene.
public class AllDungeonsManager : MonoBehaviour
{
    #region Variables
    public static AllDungeonsManager _instance;
    //[SerializeField] DungeonEntranceKey dungeon0Key;
    [SerializeField] DungeonManager dungeonManager;
    private List<MutableKeyValPair<int, DungeonManager>> dungeons = new List<MutableKeyValPair<int, DungeonManager>>();
    private List<MutableKeyValPair<int, bool>> dungeonEntranceKeys = new List<MutableKeyValPair<int, bool>>();
    private bool exists;
    #endregion

    #region Methods

    void Awake()
    {
        //Singleton Effect
        if (_instance != null && _instance != this)
        {
            Debug.Log($"Destroyed {this.gameObject}");
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        //creates a keyvaluepair list to store amount of dungeons. (Will be improved with more dungeons)
        for (int i = 0; i < 8; i++)
        {
            DungeonManager dungeon = new DungeonManager();
            AddDungeon(i, dungeon);
        }

        //creates a keyvaluepair list to store the entrance keys to open each dungeon
        for (int i = 0; i < 8; i++)
        {
            dungeonEntranceKeys.Add(new MutableKeyValPair<int, bool>(i, false));
        }
    }

    private void OnEnable()
    {
        //creates a keyvaluepair list to store amount of dungeons. (Will be improved with more dungeons)
        for (int i = 0; i < 8; i++)
        {
            DungeonManager dungeon = new DungeonManager();
            AddDungeon(i, dungeon);
        }

        //creates a keyvaluepair list to store the entrance keys to open each dungeon
        for (int i = 0; i < 8; i++)
        {
            dungeonEntranceKeys.Add(new MutableKeyValPair<int, bool>(i, false));
        }
    }

    //adds dungeon to to list
    private void AddDungeon(int num, DungeonManager manager)
    {
        dungeons.Add(new MutableKeyValPair<int, DungeonManager>(num, manager));
    }

    //gets the DungeonManager object assigned to the dungeonNum key
    public DungeonManager GetDungeonManager(int num)
    {
        foreach (var item in dungeons)
        {
            if (item.key == num)
            {
                return item.value;
            }
        }
        return null;
    }

    //"activates" the dungeon key as true, so correlating dungeon animation can be played.
    public void ActivateDungeonEntranceKey(int dungeonNum)
    {
        foreach (var item in dungeonEntranceKeys)
        {
            if (item.key == dungeonNum)
            {
                item.value = true;
                //dungeon0Key.gameObject.SetActive(true);
            }
        }
    }

    //Checks if the dungeon entrance key has been activated (picked up) to play opening dungeon animation.
    public bool IsDungeonEntranceKeyActive(int dungeonNum)
    {
        foreach (var key in dungeonEntranceKeys)
        {
            if (key.key == dungeonNum && key.value == true)
            {
                return true;
            }
        }
        return false;
    }
    #endregion
}
