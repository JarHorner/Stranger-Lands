using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    #region Variables

    public Dialogue dialogue;
    //private ContextClue contextClue;
    [SerializeField] private InputActionAsset inputMaster;
    private InputAction interact;
    private bool containsItem = false;
    [SerializeField] private bool oneTime;

    #endregion

    #region Methods

    private void Start() 
    {
        //contextClue = FindObjectOfType<ContextClue>();
        var playerActionMap = inputMaster.FindActionMap("Player");

        interact = playerActionMap.FindAction("Interact");

        //containsItem = this.gameObject.GetComponent<GettingItem>() != null;
    }

    public void TriggerDialogue(InputAction.CallbackContext context)
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);

        // if (containsItem)
        // {
        //     this.gameObject.GetComponent<GettingItem>().PickupItem();
        // }
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        // if (other.tag == "InteractBox")
        // {
            Debug.Log("can talk");
            //contextClue.Interact();
            if (!interact.enabled)
            {
                interact.performed += TriggerDialogue;
                interact.Enable();
            }
        // }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.tag == "InteractBox")
        {
            //contextClue.Disappear();
            interact.performed -= TriggerDialogue;
            interact.Disable();
            if (oneTime)
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    #endregion
}
