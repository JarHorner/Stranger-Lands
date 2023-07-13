using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextClue : MonoBehaviour
{
    #region Variables
    [SerializeField] private Sprite interactSprite;
    [SerializeField] private Sprite talkSprite;
    [SerializeField] private List<Sprite> speachSprites;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Animator animator;

    #endregion

    #region  Methods

    // starts the speach animation 
    public void Speach() 
    {
        sprite.enabled = true;
        animator.enabled = true;
    }

    private void Interact() 
    {
        sprite.enabled = true;
        sprite.sprite = interactSprite;
    }

    private void Talk() 
    {
        sprite.enabled = true;
        sprite.sprite = talkSprite;
    }

    public void ChangeContextClue(bool isNPC) 
    {
        // if animator is playing, disables it so it does not keep running
        animator.enabled = false;

        if (isNPC)
        {
            Talk();
        } 
        else
        {
            Interact();     
        }
    }

    public void Disappear()
    {
        sprite.enabled = false;
    }

    #endregion
}
