using System.Collections;
using System.Collections.Generic;
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

    #endregion

    #region Methods

    public PlayerData(PlayerController player, PlayerCamera camera, InventoryMenu inventoryMenu)
    {
        // if any of these are null (they should not be), nothing is saved
        if (player != null && camera != null && inventoryMenu != null)
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
            Debug.Log(healthSystem.HeartList.Count);
            for (int i = 0; i < healthSystem.HeartList.Count; i++)
            {
                currentHealth += healthSystem.HeartList[i].Fragments;
            }
            item1 = 0;
            item2 = 1;
        }
    }

    #endregion
}
