using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class OnScreenKeyboard : MonoBehaviour
{
    #region Variables
    [SerializeField] private GameObject controllerKeyboard;
    [SerializeField] private GameObject enteredFileName;
    [SerializeField] private GameObject[] keys;
    private TMP_Text selectedFileName;
    private TMP_InputField enteredFileNameText;
    private bool isCaps = true;

    #endregion

    #region Methods

    // starts the process of naming a file, and if user is using gamepad, keyboard will be showing
    public void EnterFileName(TMP_Text selectedFileName)
    {
        this.gameObject.SetActive(true);
        this.selectedFileName = selectedFileName;
        enteredFileNameText = enteredFileName.GetComponent<TMP_InputField>();
        if (ControlScheme.isController)
        {
            controllerKeyboard.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(GameObject.Find("Q").gameObject);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(enteredFileName);
        }
    }
    
    // Checks if enterkey was pressed and if a title was given, a file is created.
    private void Update() 
    {
        if (!ControlScheme.isController)
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame && enteredFileNameText.text.Length > 0)
            {
                CreateFile();
            }
        }
    }

    // Removes any characters that are not letters. 
    private void OnGUI() 
    {
        if (!ControlScheme.isController)
        {
            char chr = Event.current.character;
            enteredFileNameText.text = Regex.Replace(enteredFileNameText.text, @"[^a-zA-Z]", "");
        }
    }

    // Adds a letter. Used when player is using controller.
    public void InputLetter(TMP_Text letter)
    {
        enteredFileNameText.text += letter.text;
    }

    // Removes the last letter. Used when player is using controller.
    public void Backspace()
    {
        enteredFileNameText.text = enteredFileNameText.text.Substring(0, enteredFileNameText.text.Length - 1);
    }

    // Changes letters into caps and vis-versa. Used when player is using controller.
    public void ChangeCaps()
    {
        if (isCaps)
            isCaps = false;
        else
            isCaps = true;

        foreach (GameObject key in keys)
        {
            string keyText = key.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text;
            Debug.Log("before: " + keyText);
            if (isCaps)
                key.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = keyText.ToUpper();
            else
                key.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = keyText.ToLower();
            Debug.Log("after: " + key.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text);
        }
    }

    // confirms if a name has been given
    public void Enter()
    {
        if (enteredFileNameText.text.Length > 0)
        {
            CreateFile();
        }
    }

    // Creates the save file, organizing all the basic information. Also creates PlayerPrefs.
    private void CreateFile()
    {
        string path = Application.persistentDataPath + $"/{enteredFileNameText.text}.SL";
        if ( File.Exists(path) )
        {
            Debug.Log("file exists, changing name");
            enteredFileNameText.text = enteredFileNameText.text + "1";
        }
        Debug.Log("Create file: " + Application.persistentDataPath + $"/{enteredFileNameText.text}.SL");
        selectedFileName.text = enteredFileNameText.text;
        SaveSystem.CurrentFileName = $"/{enteredFileNameText.text}.SL";

        PlayerPrefs.SetString(selectedFileName.transform.parent.gameObject.name, enteredFileNameText.text);

        SceneManager.LoadScene("Forest");
    }

    // Limits the amount of characters so panel looks nice
    public void LimitCharacter(TMP_InputField inputText)
    {
        if (inputText.text.Length > 8)
        {
            inputText.text = inputText.text.Substring(0, inputText.text.Length - 1);
        }
    }

    #endregion
}
