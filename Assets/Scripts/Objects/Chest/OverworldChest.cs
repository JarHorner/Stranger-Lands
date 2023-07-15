using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OverworldChest : MonoBehaviour
{
    #region Variables
    [SerializeField] InputActionAsset inputMaster;
    private InputAction interact;
    private PlayerUI playerUI;
    // private DungeonManager dungeonManager;
    private PauseGame pauseGame;
    private ContextClue contextClue;
    [SerializeField] private Sprite closedChest;
    [SerializeField] private int chestNum;
    private bool canOpenChest = false;
    [SerializeField] private GameObject item;
    public bool usableItem;

    #endregion

    #region Methods

    void Start()
    {
        contextClue = FindObjectOfType<ContextClue>();
        playerUI = FindObjectOfType<PlayerUI>();
        // dungeonManager = FindObjectOfType<AllDungeonsManager>().GetDungeonManager(dungeonNum);
        pauseGame = FindObjectOfType<PauseGame>();
        //if chest has already been opened before, chest stays open so it cant be re-collected.
        //if(dungeonManager.GetChestStayOpen(chestNum))
        // {
        //     this.GetComponent<SpriteRenderer>().sprite = closedChest;
        //     canOpenChest = false;
        //     Destroy(item);
        // }
        var playerActionMap = inputMaster.FindActionMap("Player");

        interact = playerActionMap.FindAction("Interact");
    }

    private void InteractChest(InputAction.CallbackContext context)
    {
        //if player is within circle collider, chest has not been opened before and 'E' is pressed, chest is opened.
        //needs to check if chest has been opened again because the first check is only for animation.
        if (canOpenChest) //&& !dungeonManager.GetChestStayOpen(chestNum))
        {
            StartCoroutine(Open());
        }
    }

    //uses the Pause() function from GameManager to prevent movement and play music of receiving chest item.
    private IEnumerator Open()
    {
        canOpenChest = false;
        this.GetComponent<SpriteRenderer>().sprite = closedChest;

        pauseGame.Pause(false);

        if (!usableItem)
        {
            //shows item of chest and places it above chest.
            item.GetComponent<SpriteRenderer>().enabled = true;
            item.transform.localPosition = new Vector2(0f, 0.5f);
            GetItem();
            //after 1.5 seconds, everything returns to normal.
            yield return new WaitForSeconds(1.5f);
            Destroy(item);
            pauseGame.UnPause();
        }
        else
        {
            PlayerController.player.Animator.SetBool("collectItem", true);

            item.GetComponent<SpriteRenderer>().enabled = true;
            Vector2 playerPos = PlayerController.player.transform.position;
            item.transform.position = new Vector2(playerPos.x, (playerPos.y + 0.8f));
            GetItem();

            //after 1.5 seconds, everything returns to normal.
            yield return new WaitForSeconds(1.5f);
            PlayerController.player.Animator.SetBool("collectItem", false);
            Destroy(item);
            pauseGame.UnPause();
        }

        //adds chest to list so it cannot be opened again.
        // dungeonManager.AddChestStayOpen(chestNum);
    }

    //gets the child of the object, which is the item within the chest, and determines what it is.
    //depending on what the item is, it is collected.
    private void GetItem()
    {
        SpriteRenderer item = this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        string itemSpriteName = item.sprite.name;
        if (itemSpriteName.Contains("Money"))
        {
            //takes the last bit of the sprite name (which is the value amt) and adds it to money.
            int index1 = itemSpriteName.IndexOf('_');
            int index2 = itemSpriteName.IndexOf('_', index1 + 1) - 1;
            int indexLength = index2 - index1;
            int moneyAmt = int.Parse(itemSpriteName.Substring(index1 + 1, indexLength));
            playerUI.AddMoney(moneyAmt);
        }
        // else if (itemSpriteName.Contains("Boss_Key"))
        // {
        //     dungeonManager.HasBossKey = true;
        // }
        // else if (itemSpriteName.Contains("Dungeon_Key"))
        // {
        //     dungeonManager.CurrentKeys += 1;
        //     playerUI.ChangeKeyCountText(dungeonNum);
        // }
        else if (itemSpriteName.Contains("Health"))
        {
            HealthVisual.healthSystemStatic.Heal(4);
        }
        // else if (itemSpriteName.Contains("Map"))
        // {
        //     dungeonManager.HasMap = true;
        // }
        else if (itemSpriteName.Contains("Bow"))
        {
            FindObjectOfType<InventoryMenu>().PopulateInventorySlot("Bow");
        }
    }

    //player in range, so chest can be opened.
    private void OnTriggerEnter2D(Collider2D Collider)
    {
        if (Collider.tag == "InteractBox")
        {
            canOpenChest = true;
            contextClue.ChangeContextClue(false);
            interact.performed += InteractChest;
            interact.Enable();
        }
    }

    //player not in range, so chest cant be opened.
    private void OnTriggerExit2D(Collider2D Collider)
    {
        if (Collider.tag == "InteractBox")
        {
            canOpenChest = false;
            contextClue.Disappear();
            interact.performed += InteractChest;
            interact.Disable();
        }
    }

    #endregion
}
