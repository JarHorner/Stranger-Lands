using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public enum PlayerState
{
    idle,
    walk,
    swim,
    attack,
    interact,
    dead,
    stagger
}

public class PlayerController : MonoBehaviour
{
    #region Variables
    private static bool playerExists;
    public static PlayerController player;
    [SerializeField] private InputActionAsset inputMaster;
    private InputAction move, attack, useItem1, useItem2, pause, inventory;
    public PlayerState currentState;
    private GameManager gameManager;
    private UIManager uiManager;
    private InventoryMenu inventoryMenu;
    public int totalHearts = 3;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CapsuleCollider2D hitBox;
    [SerializeField] private CapsuleCollider2D physicBox;
    [SerializeField] private GameObject interactBox;
    [SerializeField] private Animator animator;
    public float moveSpeed;
    private Vector2 movement;
    private int movingVertical, movingHorizontal;
    private bool isMoving = false;
    private bool isSwimming = false;
    private bool isCarrying = false;
    private bool isReviving = false;
    private bool flashActive;
    private float flashCounter = 0.8f;
    private float flashLength = 0.8f;
    private float invulnerableTime = 0.5f;
    private float waitToLoad = 1.8f;
    private float attackCounter = 0.25f;
    private float shootCounter = 0f;
    private bool deathCoRunning = false;
    public Vector3 lastPlayerLocation;
    #endregion

    #region Methods

    void Awake()
    {
        Debug.Log("player exists" + PlayerExists);
        //Singleton affect code
        if (!playerExists)
        {
            playerExists = true;
            player = this;
            //ensures same player object is not destoyed when loading new scences
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        gameManager = FindObjectOfType<GameManager>();
        uiManager = FindObjectOfType<UIManager>();
        lastPlayerLocation = this.transform.position;
        Debug.Log(lastPlayerLocation);

        var playerActionMap = inputMaster.FindActionMap("Player");

        move = playerActionMap.FindAction("Movement");
        attack = playerActionMap.FindAction("Attack");
        pause = playerActionMap.FindAction("Pause");
        inventory = playerActionMap.FindAction("Inventory");
        useItem1 = playerActionMap.FindAction("useItem1");
        useItem2 = playerActionMap.FindAction("useItem2");
    }

    void Start()
    {
        currentState = PlayerState.walk;
        physicBox.enabled = true;
        hitBox.enabled = true;
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", -1);

        ControlScheme.GetControlSchemes();
    }

    private void OnEnable()
    {
        Debug.Log("enabling actions for player");
        if (SaveSystem.LoadedGame)
        {
            currentState = PlayerState.walk;
            animator.SetBool("isSwimming", false);
        }
        move.Enable();
        move.performed += PlayerMoving;
        move.canceled += PlayerMoving;
        attack.Enable();
        attack.started += PlayerAttack;
        pause.Enable();
        pause.started += uiManager.OpenPauseMenu;
        inventory.Enable();
        inventory.started += uiManager.OpenInventoryMenu;
        // useItem1.Enable();
        // useItem1.started += gameManager.UseItem;
        // useItem2.Enable();
        // useItem2.started += gameManager.UseItem;
    }

    private void OnDisable()
    {
        PlayerExists = false;
        move.performed -= PlayerMoving;
        move.canceled -= PlayerMoving;
        move.Disable();
        attack.started -= PlayerAttack;
        attack.Disable();
        pause.started -= uiManager.OpenPauseMenu;
        pause.Disable();
        inventory.started -= uiManager.OpenInventoryMenu;
        inventory.Disable();
        // useItem1.started -= gameManager.UseItem;
        // useItem1.Disable();
        // useItem2.started -= gameManager.UseItem;
        // useItem2.Disable();
    }

    private void Update()
    {

        //ensures when hurt, starts invulnerable time so player cannot be hurt until the time is 0 or less.
        if (invulnerableTime >= 0)
        {
            invulnerableTime -= Time.deltaTime;
        }

        if (currentState == PlayerState.dead && !deathCoRunning)
        {
            StartCoroutine(PlayerDead());
        }
        if (move.triggered && currentState != PlayerState.stagger && currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }
        if (move.triggered && isSwimming && currentState != PlayerState.stagger & currentState != PlayerState.interact)
        {
            currentState = PlayerState.swim;
        }

            //this code activates when player is damaged, causing the flashing of the sprite
        if (flashActive)
        {
            //if true, starts process of changing the players alpha level to flash when hit
            DamageFlashing.SpriteFlashing(flashLength, flashCounter, this.gameObject.GetComponent<SpriteRenderer>());
            flashCounter -= Time.deltaTime;
            if (flashCounter < 0)
            {
                flashCounter = flashLength;
                flashActive = false;
            }
        }


        // //depending on the direction the player is moving, when moving diagonally, the player faces the same direction.
        if (movingHorizontal == 1)
        {
            animator.SetFloat("Horizontal", 1);
            animator.SetFloat("Vertical", 0);
        }
        else if (movingHorizontal == -1)
        {
            animator.SetFloat("Horizontal", -1);
            animator.SetFloat("Vertical", 0);
        }
        else if (movingVertical == 1)
        {
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", 1);
        }
        else if (movingVertical == -1)
        {
            animator.SetFloat("Horizontal", 0);
            animator.SetFloat("Vertical", -1);
        }
        else
        {
            movingVertical = 0;
            movingHorizontal = 0;
        }
    }

    //this is where the actual movement happens. better for performance, not tying movement to framerate.
    void FixedUpdate()
    {
        if (currentState == PlayerState.walk || currentState == PlayerState.swim && currentState != PlayerState.dead)
        {
            //enables movement
            rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime * Time.timeScale);
        }
    }

    private void PlayerMoving(InputAction.CallbackContext context)
    {
        if (currentState != PlayerState.interact)
        {
            //gets input
            movement.x = move.ReadValue<Vector2>().x;
            movement.y = move.ReadValue<Vector2>().y;

            if (movement != Vector2.zero)
            {
                int[] movingAxis = new int[2];
                //sets new directions from the movement and sets bool, to play walking sound
                if (!ControlScheme.isController)
                {
                    movingAxis = DetermineDirection.DetermineKeyboardFacingDirection(animator, movement, movingHorizontal, movingVertical);
                    movingHorizontal = movingAxis[0];
                    movingVertical = movingAxis[1];
                }
                else
                {
                    movingAxis = DetermineDirection.DetermineGamepadFacingDirection(animator, movement, movingHorizontal, movingVertical);
                    movingHorizontal = movingAxis[0];
                    movingVertical = movingAxis[1];
                }
                currentState = PlayerState.walk;
                isMoving = true;
            }
            else
            {
                currentState = PlayerState.idle;
                isMoving = false;
            }
            animator.SetFloat("Speed", movement.sqrMagnitude);
        }
    }

    private void PlayerAttack(InputAction.CallbackContext context)
    {
        if (currentState != PlayerState.interact)
            StartCoroutine(AttackCo());
    }

    //if not currently attacking, starts the animation, if attack button has been pressed again, another attack
    //animation is qued up. Creating smooth attacking if the attack button is spammed.
    private IEnumerator AttackCo()
    {
        currentState = PlayerState.attack;
        animator.SetBool("isAttacking", true);

        yield return new WaitForSeconds(attackCounter);

        animator.SetBool("isAttacking", false);
        currentState = PlayerState.walk;
    }

    //Damages the player using the totalHeartsVisual, creates damage popup, and actives the flashing. only initiates code if invulnerable time is over.
    public void DamagePlayer(int damageNum)
    {
        if (invulnerableTime <= 0)
        {
            HealthVisual.healthSystemStatic.Damage(damageNum);
            flashActive = true;
            invulnerableTime = 0.5f;
        }
    }

    private IEnumerator PlayerDead()
    {
        deathCoRunning = true;
        move.Disable();

        physicBox.enabled = false;
        hitBox.enabled = false;
        bool deathWhileSwimming = animator.GetCurrentAnimatorStateInfo(0).IsTag("Swimming");
        //if player was swimming when died, this variable is true and when spawns again, wont be swimming.
        animator.SetBool("isDead", true);
        animator.SetBool("isSwimming", false);

        yield return new WaitForSeconds(waitToLoad);

        animator.SetBool("isDead", false);
        //checks to see if current animation state is swimming to reset animations before respawn
        if (deathWhileSwimming)
        {
            animator.SetBool("isSwimming", false);
            animator.Play("Walk Idle", 0, 1f);
        }
        Debug.Log("Loaded!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        move.Enable();
        physicBox.enabled = true;
        hitBox.enabled = true;
        isReviving = true;
        deathCoRunning = false;
        currentState = PlayerState.idle;
    }

    public void Knock(float knockTime)
    {
        StartCoroutine(KnockCo(knockTime));
    }

    private IEnumerator KnockCo(float knockTime)
    {
        yield return new WaitForSeconds(knockTime);
        rb.velocity = Vector2.zero;
        currentState = PlayerState.walk;
        rb.velocity = Vector2.zero;
    }
    
    //THIS TRIGGERS SWIMMING!!!!
    //enables regular movement when colliding with OutOfWater trigger when leaving water.
    private void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.gameObject.tag == "Water") 
        {
            Debug.Log("water triggered");
            //sets swimming animation and new player speed
            animator.SetBool("isSwimming", true);
            isSwimming = true;
            currentState = PlayerState.swim;
            moveSpeed = 4f;
            useItem1.Disable();
            useItem2.Disable();
        }
        else if (collider.gameObject.tag == "OutOfWater")
        {
            Debug.Log("Out of water triggered");
            animator.SetBool("isSwimming", false);
            if (isSwimming)
                isSwimming = false;
            currentState = PlayerState.walk;
            moveSpeed = 6f;
            useItem1.Enable();
            useItem2.Enable();
        }
    }

    //Getters and Setters for player variables.

   public Vector2 LastPlayerLocation
    {
        get { return lastPlayerLocation; }
        set { lastPlayerLocation = value; }
    }
    public Vector2 Movement
    {
        get { return movement; }
        set { movement = value; }
    }
    public Animator Animator
    {
        get { return animator; }
    }
    public static bool PlayerExists
    {
        get { return playerExists; }
        set { playerExists = value; }
    }
    public float ShootCounter
    {
        get { return shootCounter; }
        set { shootCounter = value; }
    }
    public bool IsCarrying
    {
        get { return isCarrying; }
        set { isCarrying = value; }
    }
     public bool IsMoving
    {
        get { return isMoving; }
        set { isMoving = value; }
    }
    public bool IsReviving
    {
        get { return isReviving; }
        set { isReviving = value; }
    }
    public int TotalHearts
    {
        get { return totalHearts; }
        set { totalHearts = value; }
    }
    #endregion
}
