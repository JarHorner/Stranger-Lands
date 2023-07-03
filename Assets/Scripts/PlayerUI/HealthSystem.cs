using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem
{
    #region Variables
    public const int MAX_FRAGMENT_AMOUNT = 4;
    public event EventHandler OnDamaged;
    public event EventHandler OnHealed;
    public event EventHandler OnDead;
    private List<Heart> heartList;
    #endregion

    #region Methods

    public HealthSystem(int heartAmount)
    {
        heartList = new List<Heart>();
        for (int i=0; i<heartAmount; i++)
        {
            Heart heart = new Heart(4);
            heartList.Add(heart);
        }
    }

    public void Damage(int damageAmount)
    {
        //cycle through all hearts starting form the end
        for (int i=heartList.Count-1; i >= 0; i--)
        {
            Heart heart = heartList[i];
            //test is this heart can absorb damageAmount
            if (damageAmount > heart.Fragments)
            {
                //heart cannot absorb full damage, damage heart then keep going to next heart
                damageAmount -= heart.Fragments;
                heart.Damage(heart.Fragments);
            }
            else
            {
                //heart can absorb sull damage, then break out of cycle
                heart.Damage(damageAmount);
                break;
            }
        }

        if (OnDamaged != null) OnDamaged(this, EventArgs.Empty);

        if (IsDead())
            if (OnDead != null) OnDead(this, EventArgs.Empty);
    }

    public void Heal(int healAmount)
    {
        for (int i=0; i < heartList.Count; i++)
        {
            Heart heart = heartList[i];
            int missingFragments = MAX_FRAGMENT_AMOUNT - heart.Fragments;
            if (healAmount > missingFragments)
            {
                healAmount -= missingFragments;
                heart.Heal(missingFragments);
            }
            else
            {
                heart.Heal(healAmount);
                break;
            }
        }

        if (OnHealed != null) OnHealed(this, EventArgs.Empty);
    }

    public bool IsDead()
    {
        return heartList[0].Fragments == 0;
    }

    public List<Heart> HeartList
    {
        get { return heartList; }
    }

    //represents a single heart
    public class Heart 
    {
        private int fragments;

        public Heart(int fragments)
        {
            this.fragments = fragments;
        }

        public void Damage(int damageAmt)
        {
            if (damageAmt >= fragments)
            {
                fragments = 0;
            }
            else
            {
                fragments -= damageAmt;
            }
        }

        public void Heal(int healAmt)
        {
            if (fragments + healAmt > MAX_FRAGMENT_AMOUNT)
            {
                fragments = MAX_FRAGMENT_AMOUNT;
            }
            else
            {
                fragments += healAmt;
            }
        }

        public int Fragments
        {
            get { return fragments; }
            set { fragments = value; }
        }
    }
    #endregion
}
