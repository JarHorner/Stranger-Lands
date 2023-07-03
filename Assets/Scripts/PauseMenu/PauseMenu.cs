using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    #region Variables
    private static bool exists;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private Animator gameSavedNoticeAnim;
    public GameObject optionsFirstButton, optionsClosedButton;

    #endregion

    #region Methods

    private void Awake() 
    {
        //Singleton Effect
        if (!exists)
        {
            exists = true;
            //ensures same player object is not destoyed when loading new scences
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy (gameObject);
        }
    }

    // saves the game
    public void SaveGame() 
    {
        SaveSystem.SavePlayer(FindObjectOfType<PlayerController>()); // FindObjectOfType<CameraController>(), FindObjectOfType<InventoryManager>());
        gameSavedNoticeAnim.SetTrigger("Fade");
    }

    // opens the options menu
    public void OptionsMenu()
    {
        optionsMenu.SetActive(true);
        //clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected object
        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
    }

    // closes options menu
    public void CloseOptionsMenu()
    {
        optionsMenu.SetActive(false);

        //clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected object
        EventSystem.current.SetSelectedGameObject(optionsClosedButton);
    }

    //exits application
    public void ExitGame() 
    {
        SaveSystem.SavePlayer(FindObjectOfType<PlayerController>());//, FindObjectOfType<CameraController>(), FindObjectOfType<InventoryManager>());
        UIManager.Instance.DeactivatePauseScreen();
        SceneManager.LoadScene("MainMenu");
    }

    #endregion
}
