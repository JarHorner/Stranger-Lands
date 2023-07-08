
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HurtPlayer : MonoBehaviour
{

    #region Variables
    private PlayerController player;
    // private Enemy enemy;
    private float waitToHurt = 1f;
    private bool isTouching;
    //in code, eventually set to 1f.
    private float waitToHit = 0.0f;

    #endregion

    #region Unity Methods

    void Start()
    {
        // enemy = GetComponent<Enemy>();
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        //if true, counts down to hurt player again. used so damage is not continuous when enemy is pressed against player.
        if (isTouching)
        {
            waitToHurt -= Time.deltaTime;
            if (waitToHurt <= 0)
            {
                player.DamagePlayer(4);
                waitToHurt = 1f;
            }
        }
        if (waitToHit > 0)
            waitToHit -= Time.deltaTime;
    }

    //when collided with player, HurtPlayer function is called (see function in HealthManager script)
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "HitBox")
        {
            //Placed so player can't repeadly run back and forth into enemy and take damage. gives some leeway.
            if (waitToHit <= 0)
            {
                Debug.Log("Hit");
                Knockback.PushBack(this.transform, other.transform.parent.GetComponent<Rigidbody2D>());
                player.DamagePlayer(4);
                waitToHit = 1f;
            }
        }

    }

    //maintains consistant damage when pressed against player
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "HitBox")
        {
            isTouching = true;
        }
    }

    //needed to reset the consistant damage timer, so it always remains the same
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "HitBox")
        {
            isTouching = false;
            waitToHurt = 1f;
        }
    }

    #endregion
}
