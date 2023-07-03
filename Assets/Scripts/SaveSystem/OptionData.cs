using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OptionData
{
    #region Variables
    public string resolution { get; set; }
    public int fullscreen { get; set; }
    public int mute { get; set; }
    public float masterVol { get; set; }
    public float BGVol { get; set; }
    public float SFXVol { get; set; }

    #endregion

    #region Methods

    public void SaveOptionData() {
        PlayerPrefs.SetString("resolution", resolution);
        PlayerPrefs.SetInt("Fullscreen", fullscreen);
        PlayerPrefs.SetInt("Mute", mute);
        PlayerPrefs.SetFloat("Master Volume", masterVol);
        PlayerPrefs.SetFloat("BG Volume", BGVol);
        PlayerPrefs.SetFloat("SFX Volume", SFXVol);
    }


    #endregion
}
