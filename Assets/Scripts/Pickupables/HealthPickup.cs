using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    #region Varibles

    #endregion

    #region Methods
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            HealthVisual.healthSystemStatic.Heal(4);
            Destroy(this.gameObject);
        }
    }
    #endregion
}
