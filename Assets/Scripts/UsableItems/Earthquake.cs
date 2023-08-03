using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthquake : MonoBehaviour
{
    #region Variables
    #endregion

    #region Methods

    public void UnleashEarthquake(float cost)
    {
        ManaBar manaBar = FindObjectOfType<ManaBar>();

        if (manaBar.Mana.CanSpend(cost))
        {
            manaBar.Mana.SpendMana(cost);
        }
    }

    #endregion
}
