using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

        ControlScheme.GetControlSchemes();
    }

    private void OnEnable()
    {
        Debug.Log("enabling actions for player");
        if (SaveSystem.LoadedGame)
        {
            currentState = PlayerState.walk;
            // animator.SetBool("isSwimming", false);
        }
        move.Enable();
        move.performed += PlayerMoving;
        move.canceled += PlayerMoving;
        // attack.Enable();
        // attack.started += PlayerAttack;
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
        // attack.started -= PlayerAttack;
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
        // //depending on the direction the player is moving, when moving diagonally, the player faces the same direction.
        // if (movingHorizontal == 1)
        // {
        //     animator.SetFloat("Horizontal", 1);
        //     animator.SetFloat("Vertical", 0);
        // }
        // else if (movingHorizontal == -1)
        // {
        //     animator.SetFloat("Horizontal", -1);
        //     animator.SetFloat("Vertical", 0);
        // }
        // else if (movingVertical == 1)
        // {
        //     animator.SetFloat("Horizontal", 0);
        //     animator.SetFloat("Vertical", 1);
        // }
        // else if (movingVertical == -1)
        // {
        //     animator.SetFloat("Horizontal", 0);
        //     animator.SetFloat("Vertical", -1);
        // }
        // else
        // {
        //     movingVertical = 0;
        //     movingHorizontal = 0;
        // }
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

    //Getters and Setters for player variables.
    public static bool PlayerExists
    {
        get { return playerExists; }
        set { playerExists = value; }
    }
    public int TotalHearts
    {
        get { return totalHearts; }
        set { totalHearts = value; }
    }
    #endregion
}
