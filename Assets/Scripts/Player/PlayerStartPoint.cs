using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartPoint : MonoBehaviour
{
    #region Variables
    private PlayerController player;
    private PlayerCamera cam;
    public string pointName;
    [SerializeField] private Vector2 minPosition;
    [SerializeField] private Vector2 maxPosition;
    #endregion

    #region Unity Methods

    void Start()
    {
        //places player and camera according to the start point
        player = PlayerController.player;

        //needed to change players movement back to normal, if changed before starting again.
        player.moveSpeed = 6f;

        //used to transport to the correct location, when multiple start points are in the same scene.
        //example if player is given the 'dungeon in' string and enters scene, the player will start
        //at the start point with the point name 'dungeon in'.
        if (player.startPoint == pointName)
        {
            Debug.Log("Placing Player & Camera");
            player.transform.position = transform.position;

            cam = FindObjectOfType<PlayerCamera>();
            cam.Target = player.transform;
            cam.MinPosition = minPosition;
            cam.MaxPosition = maxPosition;
        }
    }

    #endregion
}
