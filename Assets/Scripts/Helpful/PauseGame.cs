using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PauseGame : MonoBehaviour
{

    //pauses the the timescale of the game if true. if false, only the player will be stopped (good for cutscene stuff)
    public void Pause(bool timePause) 
    {
        if (timePause)
        {
            Time.timeScale = 0;
        }
        PlayerController player = PlayerController.player;   
        // setting speed to zero and inMenu to true ensure the player is not in moving animation 
        //player.GetComponent<Animator>().SetFloat("Speed", 0);
        //player.GetComponent<Animator>().SetBool("inMenu", true);
        // needed to ensure movement completely stops
        player.currentState = PlayerState.idle;
        // ensures player doesnt face direction of keys
        player.enabled = false;
    }

    //returns the changes during a Pause back to normal
    public void UnPause() 
    {
        Time.timeScale = 1;
        PlayerController player = PlayerController.player; 
        // these ensure player goes back to normal
        player.enabled = true;
        //player.GetComponent<Animator>().SetBool("inMenu", false);
    }
}
