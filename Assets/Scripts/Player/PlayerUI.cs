using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerUI : MonoBehaviour
{

    #region Variables
    private static bool exists;
    [SerializeField] private TMP_Text keyCount;
    [SerializeField] private TMP_Text moneyCount;
    [SerializeField] private GameObject itemBox1;
    [SerializeField] private GameObject itemBox2;
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

    }

    private void Update() 
    {

    }

    //changes key count to the amt of keys you have in a specific dungeon
    public void ChangeKeyCountText(int dungeonNum)
    {
        if (dungeonNum == -1) 
        {
            keyCount.text = "-";
        }
        else 
        {
            //int numOfKeys = allDungeonsManager.GetDungeonManager(dungeonNum).CurrentKeys;
            //keyCount.text = numOfKeys.ToString();
        }
    }

    //gets the current amount of money
    public int GetMoneyCount()
    {
        return int.Parse(moneyCount.text);
    }

    //adds money to total
    public void AddMoney(int amt) 
    {
        string count = moneyCount.text;
        Debug.Log(count);
        int total = int.Parse(moneyCount.text) + amt;
        Debug.Log(total.ToString());
        moneyCount.text = total.ToString();
    }

    //remove money to total
    public void RemoveMoney(int amt)
    {
        string count = moneyCount.text;
        int total = int.Parse(moneyCount.text) - amt;
        moneyCount.text = total.ToString();
    }
    
    public GameObject ItemBox1
    {
        get { return itemBox1; }
    }
    public GameObject ItemBox2
    {
        get { return itemBox2; }
    }
    #endregion
}
