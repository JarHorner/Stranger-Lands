using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    #region Variables
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    // [SerializeField] private AudioSource hit;
    //these three varibles can be adjusted at any time
    [SerializeField] private int damageDealt;
    [SerializeField] private float speed;
    [SerializeField] private float lifespan;

    #endregion

    #region Methods

    void Update() 
    {
        //destroys projectile if havent touched anything within its lifespan.
        Destroy (gameObject, lifespan);
    }

    //does damamge to enemy, and connects with objects and walls.
    public void OnTriggerEnter2D(Collider2D other) 
    {
        // hit.Play();
        if (other.gameObject.layer == 10) //layer 10 is Walls
        {
            StartCoroutine(ArrowHitWall());
        }
        else if (other.tag == "Object")
        {
            Debug.Log("Hit Object");
            StartCoroutine(ArrowHitObject());
        }
        else if (other.tag == "Enemy" || other.tag == "Boss")
        {
            //EnemyHealthManager eHealthMan = other.gameObject.GetComponent<EnemyHealthManager>();
            // if (eHealthMan != null)
            // {
            //     foreach (var weakness in eHealthMan.weaknesses.itemsWeakTo)
            //     {
            //         Debug.Log("cycle");
            //         if (weakness == "Arrow")
            //         {
            //             Debug.Log("Hit");
            //             if (other.gameObject.tag == "Enemy")
            //                 eHealthMan.DamageEnemy(damageDealt.InitalValue, this.transform);
            //             else
            //                 eHealthMan.DamageBoss(damageDealt.InitalValue, this.transform);
            //             Destroy(gameObject);
            //             break;
            //         }
            //         else
            //         {
            //             Destroy(gameObject);
            //         }
            //     }
            // }
            // else 
            // {
            //     Destroy(gameObject);
            // }
        }
        else if (other.tag == "Switch")
        {
            // other.gameObject.GetComponent<ColorBlockSwitch>().SwapColor();
            Destroy(gameObject);
        }
    }

    //applies a force and ensure sprite is facing the correct roation.
    public void Setup(Vector2 velocity, Vector3 direction)
    {
        //rb.velocity = velocity.normalized * speed;
        rb.AddForce(velocity * speed, ForceMode2D.Impulse);
        this.transform.rotation = Quaternion.Euler(direction);
    }

    //2 different IEnumerators because it hits a wall and object alittle differently.

    private IEnumerator ArrowHitWall()
    {
        yield return new WaitForSeconds(0.135f);
        this.GetComponent<CapsuleCollider2D>().enabled = false;
        animator.SetBool("HitObject", true);
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private IEnumerator ArrowHitObject()
    {
        yield return new WaitForSeconds(0.05f);
        this.GetComponent<CapsuleCollider2D>().enabled = false;
        animator.SetBool("HitObject", true);
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    #endregion
}
