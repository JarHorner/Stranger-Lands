using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor.Events;
using UnityEngine.SceneManagement;
using TMPro;

public class FileMenu : MonoBehaviour
{
    #region  variables
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private OnScreenKeyboard onScreenKeyboard;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text buttonText;
    [SerializeField] private SaveFile file1;
    [SerializeField] private SaveFile file2;
    [SerializeField] private SaveFile file3;
    private bool startGame = false;

    #endregion

    #region Mathods

    // Retrieves the playerPrefs, confirming if any files exist, then add the content to the file's panel.
    private void Start()
    {
        file1.FileName.text = PlayerPrefs.GetString(file1.name);
        file2.FileName.text = PlayerPrefs.GetString(file2.name);
        file3.FileName.text = PlayerPrefs.GetString(file3.name);

        AddContentToSaveFile();
    }

    // Used when user selects file panel. If a file exists, the game will start regularly, if not, 
    public void StartORLoadGame(TMP_Text saveFileName)
    {
        if (startGame && saveFileName.text != "")
        {
            StartGame();
            return;
        }

        if (mainMenu.openingNewFile)
        {
            if (saveFileName.text != "")
            {
                return;
            }
            else
            {
                onScreenKeyboard.EnterFileName(saveFileName);
                startGame = true;
            }
        }
        else
        {
            SaveSystem.CurrentFileName = $"/{saveFileName.text}.SL";
            SaveSystem.CurrentPlayerData = SaveSystem.LoadPlayer();
            // loads the player into the last scene they were in.
            if (SaveSystem.CurrentPlayerData != null)
            {
                SaveSystem.LoadedGame = true;
                SceneManager.LoadScene("Forest"); // SaveSystem.CurrentPlayerData.lastScene);
                // if (GameObject.Find("Player(Clone)") != null)
                //     GameObject.Find("Player(Clone)").GetComponent<PlayerController>().enabled = true;
            }
        }
    }

    // Used when selecting to delete a file. Changes the onClick() of each panel to a new delegate called DeleteFile().
    public void ChangeButtonToDeleteFile()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(file1.gameObject);

        if (!mainMenu.deleting)
        {
            mainMenu.deleting = true;
            file1.gameObject.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            file2.gameObject.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            file3.gameObject.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();

            UnityEventTools.AddPersistentListener(file1.gameObject.GetComponent<Button>().onClick, delegate { DeleteFile(file1.FileName); });
            UnityEventTools.AddPersistentListener(file2.gameObject.GetComponent<Button>().onClick, delegate { DeleteFile(file2.FileName); });
            UnityEventTools.AddPersistentListener(file3.gameObject.GetComponent<Button>().onClick, delegate { DeleteFile(file3.FileName); });

            buttonText.text = "Stop";   
            title.text = "Delete File";
        }
        else
        {
            RevertFromDelete();
        }

    }

    // Changes each panel back to original delegate of StartORLoadGame, then deletes that file selected. Also cleans up any progress shown on the panel.
    void DeleteFile(TMP_Text saveFileName)
    {
        RevertFromDelete();

        string addition;
        switch (saveFileName.transform.parent.gameObject.name)
        {
            case "SaveFile1":
                addition = "_h1869238163";
                break;
            case "SaveFile2":
                addition = "_h1869238160";
                break;
            case "SaveFile3":
                addition = "_h1869238161";
                break;
            default:
                addition = "";
                break;
        }

        // removes the playerpref from the registry and the file from the appData
        PlayerPrefs.DeleteKey(saveFileName.transform.parent.gameObject.name + addition);
        SaveSystem.DeletePlayer(saveFileName.text);
        AddContentToSaveFile();

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(file1.gameObject);
    }

    // Reverts the file buttons back to return function, not delete. Changes the onClick() of each panel to a new delegate called StartORLoadGame().
    private void RevertFromDelete()
    {
        mainMenu.deleting = false;
        
        file1.gameObject.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
        file2.gameObject.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
        file3.gameObject.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();

        UnityEventTools.AddPersistentListener(file1.gameObject.GetComponent<Button>().onClick, delegate { StartORLoadGame(file1.FileName); });
        UnityEventTools.AddPersistentListener(file2.gameObject.GetComponent<Button>().onClick, delegate { StartORLoadGame(file2.FileName); });
        UnityEventTools.AddPersistentListener(file3.gameObject.GetComponent<Button>().onClick, delegate { StartORLoadGame(file3.FileName); });

        buttonText.text = "Delete";
        title.text = "File Menu";
    }

    //loads the first scene of the game
    private void StartGame()
    {
        SaveSystem.LoadedGame = false;
        startGame = false;
        SceneManager.LoadScene("Forest");
        // if (GameObject.Find("Player(Clone)") != null)
        //     GameObject.Find("Player(Clone)").GetComponent<PlayerController>().enabled = true;
    }

    // Checks the file data to see if specific progress is made, so icons will show on the files panel.
    private void AddContentToSaveFile()
    {
        PlayerData file1Data = SaveSystem.LoadPlayer($"/{PlayerPrefs.GetString(file1.name)}.SL");
        PlayerData file2Data = SaveSystem.LoadPlayer($"/{PlayerPrefs.GetString(file2.name)}.SL");
        PlayerData file3Data = SaveSystem.LoadPlayer($"/{PlayerPrefs.GetString(file3.name)}.SL");
        if (file1Data != null)
            CheckCollectedItems(file1Data, file1);
        else
            RemoveCollectedItems(file1);

        if (file2Data != null)
            CheckCollectedItems(file2Data, file2);
        else
            RemoveCollectedItems(file2);

        if (file3Data != null)
            CheckCollectedItems(file3Data, file3);
        else
            RemoveCollectedItems(file3);
    }

    // Enables icons on the panel that the file does contain.
    private void CheckCollectedItems(PlayerData playerData, SaveFile file)
    {
        for (int i = 0; i < playerData.totalHearts; i++)
        {
            file.Hearts[i].enabled = true;
        }
    }

    // Removes icons on the panel that the file does not contain.
    private void RemoveCollectedItems(SaveFile file)
    {
        file.FileName.text = "";
        for (int i = 0; i < file.Hearts.Length; i++)
        {
            file.Hearts[i].enabled = false;
        }
    }

    #endregion
}
