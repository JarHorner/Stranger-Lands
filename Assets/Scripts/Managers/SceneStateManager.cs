using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStateManager : MonoBehaviour
{
    #region Variables
    public static SceneStateManager _instance;
    //[SerializeField] DungeonEntranceKey dungeon0Key;
    [SerializeField] DungeonStateManager dungeonManager;
    private List<MutableKeyValPair<int, DungeonStateManager>> dungeons = new List<MutableKeyValPair<int, DungeonStateManager>>();
    private List<MutableKeyValPair<int, OverworldStateManager>> overworldScenes = new List<MutableKeyValPair<int, OverworldStateManager>>();
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

        //creates a keyvaluepair list to store amount of dungeons.
        for (int i = 0; i < 1; i++)
        {
            DungeonStateManager dungeon = new DungeonStateManager();
            AddDungeon(i, dungeon);
        }

        //creates a keyvaluepair list to store the entrance keys to open each dungeon
        for (int i = 0; i < 1; i++)
        {
            dungeonEntranceKeys.Add(new MutableKeyValPair<int, bool>(i, false));
        }

        //creates a keyvaluepair list to store amount of scenes in the overworld.
        for (int i = 0; i < 2; i++)
        {
            OverworldStateManager overworldScene = new OverworldStateManager();
            AddOverworldScene(i, overworldScene);
        }
    }

    private void OnEnable()
    {
        //creates a keyvaluepair list to store amount of dungeons.
        for (int i = 0; i < 1; i++)
        {
            DungeonStateManager dungeon = new DungeonStateManager();
            AddDungeon(i, dungeon);
        }

        //creates a keyvaluepair list to store the entrance keys to open each dungeon
        for (int i = 0; i < 1; i++)
        {
            dungeonEntranceKeys.Add(new MutableKeyValPair<int, bool>(i, false));
        }

        //creates a keyvaluepair list to store amount of scenes in the overworld.
        for (int i = 0; i < 2; i++)
        {
            OverworldStateManager overworldScene = new OverworldStateManager();
            AddOverworldScene(i, overworldScene);
        }
    }

    //adds dungeon to to list
    private void AddDungeon(int num, DungeonStateManager manager)
    {
        dungeons.Add(new MutableKeyValPair<int, DungeonStateManager>(num, manager));
    }

    //adds overworld scene to to list
    private void AddOverworldScene(int num, OverworldStateManager manager)
    {
        overworldScenes.Add(new MutableKeyValPair<int, OverworldStateManager>(num, manager));
    }

    //gets the DungeonManager object assigned to the dungeonNum key
    public DungeonStateManager GetDungeonStateManager(int num)
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

    //gets the DungeonManager object assigned to the dungeonNum key
    public OverworldStateManager GetOverworldStateManager(int num)
    {
        foreach (var item in overworldScenes)
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
                // dungeon0Key.gameObject.SetActive(true);
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
