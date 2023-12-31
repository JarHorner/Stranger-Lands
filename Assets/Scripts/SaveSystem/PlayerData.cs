using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    #region Variables
    public string lastScene;
    public float[] lastPosition;
    public float[] cameraMinPos;
    public float[] cameraMaxPos;
    public int totalHearts;
    public int currentHealth;
    public int item1;
    public int item2;
    public int currentKeys;
    public List<int> overworldChests;
    public List<int> overworldKeys;
    public List<string> items;

    #endregion

    #region Methods

    public PlayerData(PlayerController player, PlayerCamera camera, InventoryMenu inventoryMenu, OverworldStateManager overworldStateManager)
    {
        Debug.Log(overworldStateManager);
        // if any of these are null (they should not be), nothing is saved
        if (player != null && camera != null && inventoryMenu != null && overworldStateManager != null)
        {
            lastScene = SceneTracker.LastSceneName;
            lastPosition = new float[2];
            lastPosition[0] = player.LastPlayerLocation.x;
            lastPosition[1] = player.LastPlayerLocation.y;
            cameraMinPos = new float[2];
            cameraMinPos[0] = camera.MinPosition.x;
            cameraMinPos[1] = camera.MinPosition.y;
            cameraMaxPos = new float[2];
            cameraMaxPos[0] = camera.MaxPosition.x;
            cameraMaxPos[1] = camera.MaxPosition.y;
            totalHearts = player.TotalHearts;
            HealthSystem healthSystem = HealthVisual.healthSystemStatic;
            for (int i = 0; i < healthSystem.HeartList.Count; i++)
            {
                currentHealth += healthSystem.HeartList[i].Fragments;
            }
            item1 = 0;
            item2 = 1;

            items = new List<string>();
            for (int i = 0; i < inventoryMenu.myInventorySlots.Count; i++)
            {
                if (inventoryMenu.myInventorySlots[i].thisItem != null)
                {
                    string itemName = inventoryMenu.myInventorySlots[i].thisItem.itemName;
                    itemName = itemName.Replace(" ", "");
                    Debug.Log(itemName);
                    items.Add(itemName);
                }
            }

            currentKeys = overworldStateManager.CurrentKeys;

            // takes the MutableKeyValPair and turns it into a simple item list of the chest and key number to be able to serialize
            overworldChests = new List<int>();
            int iteration = 0;
            foreach (var item in overworldStateManager.OverworldChests)
            {
                Debug.Log(item);
                if (item.value)
                {
                    overworldChests.Add(item.key);
                }
                iteration++;
            }
            iteration = 0;

            overworldKeys = new List<int>();
            foreach (var item in overworldStateManager.OverworldKeys)
            {
                Debug.Log(item);
                if (item.value)
                {
                    overworldKeys.Add(item.key);
                }
                iteration++;
            }
            iteration = 0;
        }
    }

    #endregion
}
