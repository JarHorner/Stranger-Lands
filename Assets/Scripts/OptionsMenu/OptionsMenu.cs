using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System;

public class OptionsMenu : MonoBehaviour
{

    #region Variables
    public AudioMixer audioMixer;
    Resolution[] resolutions;
    [SerializeField] TMP_Dropdown resolutionDropdown;
    [SerializeField] GameObject changeControlsButton;
    [SerializeField] GameObject firstControlsMenuButton;
    [SerializeField] Toggle fullscreenToggle;
    [SerializeField] Toggle muteToggle;
    [SerializeField] TMP_Text masterVolumeText;
    [SerializeField] TMP_Text bgmVolumeText;
    [SerializeField] TMP_Text effectVolumeText;
    [SerializeField] Slider masterVolumeSlider;
    [SerializeField] Slider bgmVolumeSlider;
    [SerializeField] Slider effectVolumeSlider;
    [SerializeField] Button exitOptionButton;
    private float previousVolume;
    #endregion

    #region Methods

    void Start() 
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        //make a new string list, and populate it with each resolution as a string.
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = $"{resolutions[i].width} x {resolutions[i].height}";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        //add list of options to the dropsown (only takes list of strings)
        options.Reverse();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();

        AdjustMenu();
    }

    // Properly the menu with prefab values
    private void AdjustMenu()
    {
        float masterVolume = PlayerPrefs.GetFloat("MasterVol");
        audioMixer.SetFloat("MasterVolume", masterVolume);
        masterVolumeSlider.value = masterVolume;
        masterVolumeText.text = GetProperTextPercentage(masterVolume);

        float backgroundVolume = PlayerPrefs.GetFloat("BGVol");
        audioMixer.SetFloat("BGVolume", backgroundVolume);
        bgmVolumeSlider.value = backgroundVolume;
        bgmVolumeText.text = GetProperTextPercentage(backgroundVolume);

        float effectVolume = PlayerPrefs.GetFloat("SFXVol");
        audioMixer.SetFloat("SFXVolume", effectVolume);
        effectVolumeSlider.value = effectVolume;
        effectVolumeText.text = GetProperTextPercentage(effectVolume);

        SetFullscreen(PlayerPrefs.GetInt("Fullscreen", 1) != 0);
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) != 0;

        SetMute(PlayerPrefs.GetInt("Mute", 0) != 0);
        muteToggle.isOn = PlayerPrefs.GetInt("Mute", 0) != 0;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    //opens control change menu
    public void OpenChangeControlsMenu(GameObject controlPanel)
    {
        controlPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected object
        EventSystem.current.SetSelectedGameObject(firstControlsMenuButton);
    }

    //closes control change menu
    public void CloseChangeControlsMenu(GameObject controlPanel)
    {
        controlPanel.SetActive(false);

        //clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected object
        EventSystem.current.SetSelectedGameObject(changeControlsButton);
    }

    //makes game fullcreen
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;

        if(isFullscreen)
        {
            PlayerPrefs.SetInt("Fullscreen", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Fullscreen", 0);
        }
    }

    //mutes the master volume, but also disabes the volume slider and changes the navigation so the slider cannot be used.
    public void SetMute(bool isMuted)
    {
        float volume;
        Navigation navigation;
        if (isMuted)
        {
            audioMixer.GetFloat("MasterVolume", out previousVolume);
            volume = -80;
            masterVolumeSlider.interactable = false;
            bgmVolumeSlider.interactable = false;
            effectVolumeSlider.interactable = false;

            navigation = exitOptionButton.navigation;
            navigation.selectOnUp = muteToggle;
            exitOptionButton.navigation = navigation;

            navigation = muteToggle.navigation;
            navigation.selectOnDown = exitOptionButton;
            muteToggle.navigation = navigation;

            PlayerPrefs.SetInt("Mute", 1);
        }
        else
        {
            volume = previousVolume;
            masterVolumeSlider.interactable = true;
            bgmVolumeSlider.interactable = true;
            effectVolumeSlider.interactable = true;

            navigation = exitOptionButton.navigation;
            navigation.selectOnUp = effectVolumeSlider;
            exitOptionButton.navigation = navigation;

            navigation = muteToggle.navigation;
            navigation.selectOnDown = masterVolumeSlider;
            muteToggle.navigation = navigation;

            PlayerPrefs.SetInt("Mute", 0);
        }
        audioMixer.SetFloat("MasterVolume", volume);
    }

    //sets the master volume of the game. (ensure all different groups are affected)
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
        masterVolumeText.text = GetProperTextPercentage(volume);
        PlayerPrefs.SetFloat("MasterVol", volume);
    }

    //sets the background volume of the game. (ensure all different groups are affected)
    public void SetBackgroundVolume(float volume)
    {
        audioMixer.SetFloat("BGVolume", volume);
        bgmVolumeText.text = GetProperTextPercentage(volume);
        PlayerPrefs.SetFloat("BGVol", volume);
    }

    //sets the sound effect volume of the game. (ensure all different groups are affected)
    public void SetSoundEffectVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
        effectVolumeText.text = GetProperTextPercentage(volume);
        PlayerPrefs.SetFloat("SFXVol", volume);
    }

    // tool used to get proper percentage from float to int
    private string GetProperTextPercentage(float value)
    {
        return (Mathf.RoundToInt(value * 1.25f) + 100) + "%";
    }




    #endregion
}
