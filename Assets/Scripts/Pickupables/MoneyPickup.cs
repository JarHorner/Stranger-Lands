using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyPickup : MonoBehaviour
{
    #region Variables
    [SerializeField] private int moneyCount;
    [SerializeField] private Animator animator;
    private PlayerUI playerUI;
    private float timeToShine;
    private float maxTimeToShine;
    #endregion

    #region Methods
    void Start()
    {
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

    //enables grabbing money and adding to total (UiManager)
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            playerUI.AddMoney(moneyCount);
            Destroy(gameObject);
        }
    }
    #endregion
}
