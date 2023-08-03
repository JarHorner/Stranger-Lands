using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    #region Variables
    private PlayerUI playerUI;
    private OverworldStateManager overworldStateManager;
    [SerializeField] private Animator animator;
    [SerializeField] private int sceneNum;
    [SerializeField] private int keyNum;
    private float timeToShine;
    private float maxTimeToShine;
    #endregion

    #region Methods

    void Start()
    {
        overworldStateManager = FindObjectOfType<OverworldStateManager>();

        //if key has already been grabbed before, destroys object so it cant be re-collected.
        if (overworldStateManager.GetKeyStayDestroyed(keyNum))
        {
            Destroy(gameObject);
        }

        playerUI = FindObjectOfType<PlayerUI>();

        timeToShine = maxTimeToShine = animator.GetFloat("timeToShine");
    }

    void Update()
    {
        timeToShine -= Time.deltaTime;

        if (timeToShine < 0)
        {
            animator.SetTrigger("shine");
            timeToShine = maxTimeToShine;
        }
    }

    //enables grabbing a key and adding to dungeon currently in.
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player" && !overworldStateManager.GetKeyStayDestroyed(keyNum))
        {
            overworldStateManager.AddKeyStayDestoryed(keyNum);
            overworldStateManager.CurrentKeys += 1;
            playerUI.ChangeKeyCountText(sceneNum);
            Destroy(gameObject);
        }
    }
    #endregion
}
