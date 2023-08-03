using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPickup : MonoBehaviour
{
    #region Varibles

    #endregion

    #region Methods

    private void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.tag == "Player")
        {
            ManaBar manaBar = FindObjectOfType<ManaBar>();
            manaBar.Mana.RecoverMana(20);
            Destroy(this.gameObject);
        }
    }
    #endregion
}
