using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Assertions;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    [SerializeField] private InputActionAsset inputMaster;
    private InputAction cancel, exitPause, exitInventory, assignItem1, assignItem2;
    private PauseGame pauseGame;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject pauseFirstButton, inventoryFirstButton;
    [SerializeField] private GameObject inventoryScreen;
    [SerializeField] InventoryMenu inventoryMenu;
    private bool isPaused = false;
    private bool inventoryOpen = false;
    public static UIManager Instance { get { return _instance; } }
    
    void Awake() 
    {
        //Singleton Effect
        if (_instance != null && _instance != this)
        {
            Debug.Log($"Destroyed {this.gameObject}");
            Destroy (this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        pauseGame = FindObjectOfType<PauseGame>();
        
        var uiActionMap = inputMaster.FindActionMap("UI");

        cancel = uiActionMap.FindAction("Cancel");
        exitPause = uiActionMap.FindAction("Pause");
        exitInventory = uiActionMap.FindAction("Inventory");
        assignItem1 = uiActionMap.FindAction("AssignItem1");
        assignItem2 = uiActionMap.FindAction("AssignItem2");
    }

    //Used in PlayerController to access PauseGame method.
    public void OpenPauseMenu(InputAction.CallbackContext context)
    {
        PauseGame();
    }

    //Pause the game by changing timeScale, reducing volume, opening pause panel and disabling PlayerController script to stop movement
    public void PauseGame()
    {
        if(!isPaused)
        {
            //pauseGame.Pause(true);
            isPaused = true;
            pauseScreen.SetActive(true);

            exitPause.performed += OpenPauseMenu;
            exitPause.Enable();

            //clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            //set a new selected object
            EventSystem.current.SetSelectedGameObject(pauseFirstButton);
        }
        else
        {
            //pauseGame.UnPause();
            isPaused = false;
            pauseScreen.SetActive(false);

            exitPause.performed -= OpenPauseMenu;
            exitPause.Disable();
        }
    }

    public void DeactivatePauseScreen()
    {
        pauseScreen.SetActive(false);
    }

    //Used in PlayerController to access InventoryScreen method.
    public void OpenInventoryMenu(InputAction.CallbackContext context)
    {
        InventoryScreen();
    }

    //almost the same as the PauseGame() method but will be opening a different screen.
    private void InventoryScreen()
    {
        if (!inventoryOpen)
        {
            pauseGame.Pause(true);
            inventoryMenu.SetTextAndButton("", "", null, null);
            inventoryOpen = true;
            inventoryScreen.SetActive(true);

            assignItem1.performed += inventoryMenu.AssignItem;
            assignItem2.performed += inventoryMenu.AssignItem;
            assignItem1.Enable();
            assignItem2.Enable();

            exitInventory.performed += OpenInventoryMenu;
            exitInventory.Enable();

            //clear selected object
            EventSystem.current.SetSelectedGameObject(null);
            //set a new selected object
            EventSystem.current.SetSelectedGameObject(inventoryFirstButton);
        }
        else
        {
            pauseGame.UnPause();
            inventoryOpen = false;
            inventoryScreen.SetActive(false);

            assignItem1.performed -= inventoryMenu.AssignItem;
            assignItem2.performed -= inventoryMenu.AssignItem;
            assignItem1.Disable();
            assignItem2.Disable();

            exitInventory.performed -= OpenInventoryMenu;
            exitInventory.Disable();
        }
    }

    public bool InventoryOpen
    {
        get { return inventoryOpen; }
    }
}
