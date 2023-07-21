using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
        #region Variables
    // private Enemy enemy;
    private Animator anim;

    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player")
        {
            anim.SetBool("Activate", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player")
        {
            anim.SetBool("Activate", false);
        }
    }
    #endregion
}
