using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class Bow : MonoBehaviour
{
    #region Variables
    private PlayerController player;
    private PlayerUI playerUI;
    [SerializeField] private InventoryItem bowInvItem;
    [SerializeField] GameObject projectile;
    #endregion


    #region Methods

    public void ShootArrow()
    {
        player = PlayerController.player;
        if (bowInvItem.numberHeld != 0 && player.ShootCounter <= 0f)
        {
            player.Animator.SetTrigger("Shoot");
            Vector2 temp = new Vector2(player.Animator.GetFloat("Horizontal"), player.Animator.GetFloat("Vertical"));
            Arrow arrow = Instantiate(projectile, player.gameObject.transform.position, Quaternion.identity).GetComponent<Arrow>();
            arrow.Setup(temp, ShootDirection());
            UseArrow();
            player.ShootCounter = 0.5f;
        }
    }

    //calculates the proper degree the player is facing, the arrow will shoot in that direction.
    private Vector3 ShootDirection()
    {
        //degree measure of how much to rotate arrow
        float temp = Mathf.Atan2(player.Animator.GetFloat("Vertical"), player.Animator.GetFloat("Horizontal")) * Mathf.Rad2Deg;
        return new Vector3(0, 0, temp + -90);
    }

    private void UseArrow()
    {
        playerUI = FindObjectOfType<PlayerUI>();
        TextMeshProUGUI item1Count = playerUI.ItemBox1.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        Image item1 = playerUI.ItemBox1.transform.GetChild(0).GetComponent<Image>();
        TextMeshProUGUI item2Count = playerUI.ItemBox2.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        Image item2 = playerUI.ItemBox2.transform.GetChild(0).GetComponent<Image>();

        Debug.Log(GameManager.gameManager.lastItemSlotUsed);
        bowInvItem.numberHeld--;

        //checks which item box has the bow, and adjusts arrow value
        if (GameManager.gameManager.lastItemSlotUsed == "UseItem1")
        {
            item1Count.text = "" + bowInvItem.numberHeld;

        }
        else if (GameManager.gameManager.lastItemSlotUsed == "UseItem2")
        {
            item2Count.text = "" + bowInvItem.numberHeld;
        }

    }

    #endregion
}
