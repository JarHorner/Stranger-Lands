using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{

    #region Variables
    [SerializeField] private GameObject newGameButton;
    [SerializeField] private GameObject loadGameButton;
    public bool openingNewFile
    {
        get;
        private set;
    }
    public bool deleting
    {
        get;
        set;
    }

    #endregion

    #region Unity Methods

    private void Awake()
    {
        ControlScheme.GetControlSchemes();
    }

    void Update()
    {
        ControlScheme.GetUsedControlScheme();
    }

    // activates file menu with a flag of new so selecting a file properly opens the naming file panel
    public void NewGame(GameObject panel)
    {
        Debug.Log("NewGame");
        openingNewFile = true;
        ActivateMenu(panel);
    }

    //exits the application.
    public void ExitGame()
    {
        Application.Quit();
    }

    // checks the name of the panel, then opens a menu based off the name
    public void ActivateMenu(GameObject panel)
    {
        Debug.Log("ActivateMenu");
        if (panel.name == "OptionsPanel")
        {
            panel.SetActive(true);

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(GameObject.Find("ResolutionDropdown").gameObject);
        }
        else if (panel.name == "FilePanel")
        {
            panel.SetActive(true);
            if (openingNewFile)
            {
                newGameButton.GetComponent<Button>().enabled = false;
                newGameButton.GetComponent<Image>().enabled = false;
            }
            else
            {
                loadGameButton.GetComponent<Button>().enabled = false;
                loadGameButton.GetComponent<Image>().enabled = false;
            }

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(GameObject.Find("SaveFile1").gameObject);
        }
    }

    // checks the name of the panel, then closes a menu based off the name
    public void DeactivateMenu(GameObject panel)
    {
        if (panel.name == "OptionsPanel")
        {
            panel.SetActive(false);
        }
        else if (panel.name == "FilePanel")
        {
            panel.SetActive(false);
            deleting = false;
            if (openingNewFile)
            {
                newGameButton.GetComponent<Button>().enabled = true;
                newGameButton.GetComponent<Image>().enabled = true;
            }
            else
            {
                loadGameButton.GetComponent<Button>().enabled = true;
                loadGameButton.GetComponent<Image>().enabled = true;
            }
            openingNewFile = false;
        }
        //clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //set a new selected object
        EventSystem.current.SetSelectedGameObject(newGameButton);
    }

    #endregion
}
