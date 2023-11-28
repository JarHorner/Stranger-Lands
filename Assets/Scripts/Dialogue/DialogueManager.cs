using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    #region Variables
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    [SerializeField] private Animator animator;
    private PauseGame pauseGame;
    private ContextClue contextClue;
    private static DialogueManager _instance;
    private Queue<string> sentences;
    private bool startedConversation;
    [SerializeField] private bool fullSentenceDisplayed;
    private bool talkingNPC;
    private string currentSentence;

    #endregion
    void Awake()
    {
        //Singleton Effect
        if (_instance != null && _instance != this)
        {
            Debug.Log($"Destroyed {this.gameObject}");
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        sentences = new Queue<string>();
        pauseGame = FindObjectOfType<PauseGame>();
        contextClue = FindObjectOfType<ContextClue>();
    }

    //when player interacts with NPC dialog. Sets up the dialog that object has, then uses the DisplayNextSentence() 
    public void StartDialogue(Dialogue dialogue, bool isNPC)
    {
        Debug.Log("dialogue");
        if (!startedConversation)
        {
            talkingNPC = isNPC;
            // changes the context clue to speach
            contextClue.Speach();

            PlayerController.player.currentState = PlayerState.interact;
            pauseGame.Pause(false);
            nameText.text = dialogue.name;

            animator.SetBool("isOpen", true);

            // changes the context clue to speach
            contextClue.Speach();

            //ensures queue is empty of past sentences, then populates queue with new sentences.
            sentences.Clear();

            foreach (string sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }
            startedConversation = true;
            fullSentenceDisplayed = true;
        }

        DisplayNextSentence();
    }

    //when player interacts with chest dialog. Sets up the dialog that object has, then uses the DisplayNextSentence() 
    public void StartChestDialogue(Dialogue dialogue)
    {
        Debug.Log("dialogue");
        if (!startedConversation)
        {
            contextClue.Disappear();

            PlayerController.player.currentState = PlayerState.interact;
            pauseGame.Pause(false);
            nameText.text = dialogue.name;

            animator.SetBool("isOpen", true);

            //ensures queue is empty of past sentences, then populates queue with new sentences.
            sentences.Clear();

            foreach (string sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }
            startedConversation = true;
        }

        DisplayNextSentence();
    }

    //Ends dialog if queue has no more sentences. Dequeue the sentence if the queue has another item.
    public void DisplayNextSentence()
    {
        StopAllCoroutines();

        if (fullSentenceDisplayed && sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        else if (fullSentenceDisplayed)
        {
            currentSentence = sentences.Dequeue();
            StartCoroutine(TypeSentence(currentSentence));
            fullSentenceDisplayed = false;
        }
        else
        {
            dialogueText.text = currentSentence;
            fullSentenceDisplayed = true;
        }
    }

    //types the sentence out 1 letter at a time in the text box.
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.01f);
        }
        fullSentenceDisplayed = true;
    }

    //un-pauses the game and closes the text menus.
    private void EndDialogue()
    {
        PlayerController.player.currentState = PlayerState.walk;
        pauseGame.UnPause();
        animator.SetBool("isOpen", false);

        contextClue.ChangeContextClue(talkingNPC);

        startedConversation = false;
    }

    //un-pauses the game and closes the text menus.
    private void EndChestDialogue()
    {
        PlayerController.player.currentState = PlayerState.walk;
        pauseGame.UnPause();
        animator.SetBool("isOpen", false);

        startedConversation = false;
    }


    public bool StartedConversation
    {
        get { return startedConversation; }
    }
    #region Methods

    #endregion
}
