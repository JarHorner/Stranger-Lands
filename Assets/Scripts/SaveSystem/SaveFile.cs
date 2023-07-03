using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveFile : MonoBehaviour
{
    #region Variables

    [SerializeField] private TMP_Text fileName;
    [SerializeField] private Image[] hearts;

    #endregion

    #region Methods

    public Image[] Hearts
    {
        get { return hearts; }
    }

    public TMP_Text FileName
    {
        get { return fileName; }
        set { fileName = value; }
    }

    #endregion
}
