using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArrowsPickup : MonoBehaviour
{
    #region Variables

    [SerializeField] private InventoryItem bowInvItem;
    private PlayerUI playerUI;

    #endregion

    #region Methods

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            int total = bowInvItem.numberHeld += 5;

            playerUI = FindObjectOfType<PlayerUI>();
            Image item1 = playerUI.ItemBox1.transform.GetChild(0).GetComponent<Image>();
            Image item2 = playerUI.ItemBox2.transform.GetChild(0).GetComponent<Image>();

            TextMeshProUGUI textToChange;

            //checks which item box has the bow, and adjusts arrow value
            if (item1.sprite.name == "Bow")
            {
                textToChange = playerUI.ItemBox1.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            }
            else if (item2.sprite.name == "Bow")
            {
                textToChange = playerUI.ItemBox2.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            }
            else
            {
                textToChange = null;
            }

            if (total > bowInvItem.maxNumberHeld)
            {
                bowInvItem.numberHeld = bowInvItem.maxNumberHeld;
                textToChange.text = "" + bowInvItem.maxNumberHeld;
            }
            else
            {
                bowInvItem.numberHeld = total;
                if (textToChange != null)
                    textToChange.text = "" + total;
            }

            Destroy(this.gameObject);
        }
    }

    #endregion
}
