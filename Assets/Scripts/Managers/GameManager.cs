using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    #region Variables
    private static bool exists;
    private string currentScene;
    private PlayerUI playerUI;
    [SerializeField] InventoryMenu inventoryMenu;
    
    #endregion

    #region Unity Methods

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

    void Start()
    {
        SaveSystem.SavePlayer(FindObjectOfType<PlayerController>(), FindObjectOfType<PlayerCamera>(), FindObjectOfType<InventoryMenu>());
        ChangeCurrentScene();
        playerUI = FindObjectOfType<PlayerUI>();
        //changes the amt of keys shown in the UI depending on scene (Will add more with more dungeons)
        if (CurrentScene.Contains("Dungeon"))
        {
            //gets the last index (which will be the number of the dungeon)
            char dungeonNum = CurrentScene[CurrentScene.Length - 1];
            Debug.Log(dungeonNum);
            //converts the char to int
            playerUI.ChangeKeyCountText(int.Parse(dungeonNum.ToString()));
        }
        else
        {
            playerUI.ChangeKeyCountText(-1);
        }
    }

    public void UseItem(InputAction.CallbackContext context)
    {
        FindItemToUse(context);
    }

    private void FindItemToUse(InputAction.CallbackContext context)
    {
        Debug.Log(context.action.name);
        if (Time.timeScale != 0 && context.action.name == "UseItem1")
        {
            if (inventoryMenu.usableItems.myInventory[0] != null)
                inventoryMenu.usableItems.myInventory[0].Use();
        }
        else if (Time.timeScale != 0 && context.action.name == "UseItem2")
        {
            if (inventoryMenu.usableItems.myInventory[1] != null)
                inventoryMenu.usableItems.myInventory[1].Use();
        }
    }

    private void ChangeCurrentScene()
    {
        var scene = SceneManager.GetActiveScene();
        currentScene = scene.name;
    }

    public string CurrentScene
    {
        get { return currentScene; }
    }
    #endregion
}
