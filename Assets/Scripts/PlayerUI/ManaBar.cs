using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    #region Variables
    private Mana mana;
    private float barMaskHeight;
    private RectTransform barMaskRectTransform;
    private RectTransform edgeRectTransform;
    private RawImage barImage;
    #endregion

    #region Methods
    void Awake() 
    {
        barMaskRectTransform = transform.Find("BarMask").GetComponent<RectTransform>();
        barImage = transform.Find("BarMask").Find("Bar").GetComponent<RawImage>();
        edgeRectTransform = transform.Find("Edge").GetComponent<RectTransform>();

        barMaskHeight = barMaskRectTransform.sizeDelta.y;
        mana = new Mana();

    }

    void Update()
    {
        Rect uvRect = barImage.uvRect;
        uvRect.y -= 0.2f * Time.deltaTime;
        barImage.uvRect = uvRect; 

        Mana.Update();

        Vector2 barMaskSizeDelta = barMaskRectTransform.sizeDelta;
        barMaskSizeDelta.y = mana.ManaAmountNormalized() * barMaskHeight;
        barMaskRectTransform.sizeDelta = barMaskSizeDelta;

        edgeRectTransform.gameObject.SetActive(mana.ManaAmountNormalized() < 1f && mana.ManaAmountNormalized() > 0f );
        edgeRectTransform.anchoredPosition = new Vector2(0, (mana.ManaAmountNormalized() * barMaskHeight) + 7);
    }

    public Mana Mana
    {
        get { return mana; }
    }
    #endregion
}

public class Mana {
    public const int MANA_MAX = 100;
    private float manaAmount;
    private float newManaAmount;
    private float recoverSpeed;
    private float useSpeed;

    public Mana()
    {
        manaAmount = MANA_MAX;
        newManaAmount = MANA_MAX;
        recoverSpeed = 60f;
        useSpeed = 120f;
    }

    public void Update()
    {
        if (ManaAmountNormalized() < NewManaAmountNormalized())
        {
            manaAmount += recoverSpeed * Time.deltaTime;
            manaAmount = Mathf.Clamp(manaAmount, manaAmount, newManaAmount);
        }
        else if (ManaAmountNormalized() > NewManaAmountNormalized())
        {
            manaAmount -= useSpeed * Time.deltaTime;
            manaAmount = Mathf.Clamp(manaAmount, newManaAmount, manaAmount);
        }
    }


    public void SpendMana(float amount)
    {
        newManaAmount -= amount;
        if (newManaAmount < 0)
        {
            newManaAmount = 0;
        }
    }

    public void RecoverMana(float amount)
    {
        newManaAmount += amount;
        if (newManaAmount > MANA_MAX)
        {
            newManaAmount = MANA_MAX;
        }
    }

    public bool CanSpend(float amount)
    {
        if (manaAmount - amount < 0)
        {
            return false;        
        }
        else
        {
            return true;
        }
    }

    public float ManaAmountNormalized()
    {
        return (manaAmount / MANA_MAX);
    }
    public float NewManaAmountNormalized()
    {
        return (newManaAmount / MANA_MAX);
    }

    public float ManaAmount
    {
        get { return manaAmount; }
    }

    public float NewManaAmount
    {
        get { return newManaAmount; }
    }
}
