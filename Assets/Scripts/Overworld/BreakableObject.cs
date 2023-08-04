using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BreakableObject : MonoBehaviour
{
    #region Varibles
    [SerializeField] InputActionAsset inputMaster;
    private InputAction interact;
    //[SerializeField] private RandomLoot loot;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CircleCollider2D objectCollider;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sprite;
    private PlayerController player;
    private int damage = 1;
    private Vector3 thrownPos;
    private bool pickup = false;
    private bool thrown = false;
    private float speed = 10f;
    private float airTime = 0.4f;
    private float dropTime = 0.05f;
    #endregion

    #region Methods

    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        var playerActionMap = inputMaster.FindActionMap("Player");

        interact = playerActionMap.FindAction("Interact");
    }

    void Update()
    {
        //THIRD
        //this ensures the object moves correctly, and breaks when it collides or hits target location.
        if (thrown)
        {
            airTime -= Time.deltaTime;
            if (airTime <= 0)
            {
                dropTime -= Time.deltaTime;
                rb.gravityScale += 1f;
                if (dropTime <= 0)
                {
                    rb.velocity = Vector2.zero;
                    rb.gravityScale = 0;
                    //loot.DropItem();
                    animator.SetTrigger("Thrown");

                    interact.performed -= PickupOrThrow;
                    interact.Disable();

                    thrown = false;
                    Destroy(this.gameObject);
                }
            }
        }
    }

    private void PickupOrThrow(InputAction.CallbackContext context)
    {
        if (pickup && !player.IsCarrying)
        {
            Debug.Log("Picking up");
            PickUp(context);
        }
        else
        {
            Debug.Log("Throwing");
            Throw(context);
        }
    }

    //FIRST
    //When able to be picked up, player lifts object above head.
    private void PickUp(InputAction.CallbackContext context)
    {
        if (pickup && !player.IsCarrying)
        {
            this.transform.position = new Vector3(player.transform.position.x, (player.transform.position.y + 0.8f), 0);
            this.transform.parent = player.transform;
            player.IsCarrying = true;
            objectCollider.isTrigger = true;
            sprite.sortingLayerName = "Player";
            pickup = false;
        }
    }

    //SECOND
    //when able to be thrown, player will throw object.
    private void Throw(InputAction.CallbackContext context)
    {
        float temp = Mathf.Atan2(player.Animator.GetFloat("Vertical"), player.Animator.GetFloat("Horizontal")) * Mathf.Rad2Deg;
        if (temp == 0)
        {
            thrownPos = transform.right;
        }
        else if (temp > 0 && temp <= 135)
        {
            thrownPos = transform.up;
        }
        else if (temp == 180)
        {
            thrownPos = -transform.right;
        }
        else
        {
            thrownPos = -transform.up;
        }

        this.transform.parent = null;
        //this.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0);
        objectCollider.isTrigger = false;
        player.IsCarrying = false;
        rb.isKinematic = false;
        rb.AddForce(thrownPos * speed, ForceMode2D.Impulse);
        //changes layer to PlayerProjectile
        this.gameObject.layer = 8;
        thrown = true;
    }


    //drops the obejct and breaks when in contact with other object
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (thrown)
        {
            //check if it layers "Object" and "Walls"
            if (other.gameObject.layer == 13 || other.gameObject.layer == 10)
            {
                thrown = false;
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;
                //loot.DropItem();
                animator.SetTrigger("Thrown");
                StartCoroutine(RemoveRubble());
            }
            if (other.gameObject.tag == "Enemy")
            {
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;
                //other.gameObject.GetComponent<EnemyHealthManager>().DamageEnemy(damage, this.transform);
                //loot.DropItem();
                animator.SetTrigger("Thrown");
                StartCoroutine(RemoveRubble());
            }
        }
    }

    //enables destorying pot with sword, or picking it up
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Sword")
        {
            //loot.DropItem();
            animator.SetTrigger("cut");
            sprite.sortingLayerName = "Object";
            sprite.sortingOrder = 0;
            interact.performed -= PickupOrThrow;
            interact.Disable();
        }
        if (other.gameObject.tag == "InteractBox")
        {
            pickup = true;
            interact.performed += PickupOrThrow;
            interact.Enable();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "InteractBox" && !player.IsCarrying)
        {
            pickup = false;
            interact.performed -= PickupOrThrow;
            interact.Disable();
        }
    }

    //Destroys gameobject after 3 seconds
    private IEnumerator RemoveRubble()
    {
        sprite.sortingLayerName = "Object";
        sprite.sortingOrder = 0;
        interact.performed -= PickupOrThrow;
        interact.Disable();
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }

    public float DropTime
    {
        get { return dropTime; }
    }

    #endregion
}
