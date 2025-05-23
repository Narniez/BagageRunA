using UnityEngine;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 30f;
    [SerializeField] private float jumpForce = 5f;
    //[SerializeField] private float dashForce = 3f;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    //[SerializeField] private float groundCheckDistance = 0.05f;

    [SerializeField] private GameObject inGameMenu;


    private float horizontalInput;
    private float verticalInput;
    private Rigidbody2D playerRb;


    private bool isGrounded;
    private bool isFacingRight = true;
    private bool canMove = true;

    // private PlayerMeleeAttack meleeAttack;
    private Vector2 movement;


    [SerializeField] AudioClip walkingSound;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioSource helperSource;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        //meleeAttack = GetComponent<PlayerMeleeAttack>();

    }

    // Update is called once per frame

    void Jump()
    {
        //Debug.Log(playerRb.velocity.y);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && canMove)
        {
            playerRb.linearVelocity = new Vector2(0, jumpForce);
        }
    }

    void Movement()
    {
        movement.x = Input.GetAxis("Horizontal");
        horizontalInput = Input.GetAxis("Horizontal");
        float horizontalMovemet = horizontalInput * playerSpeed * Time.deltaTime;



        playerRb.linearVelocity = new Vector2(horizontalMovemet, playerRb.linearVelocity.y);

        if (Mathf.Abs(playerRb.linearVelocity.x) > 0 && playerRb.linearVelocity.y == 0)
        {
            audioSource.clip = walkingSound;
            Debug.Log("Player is moving");
            if (audioSource.clip == walkingSound && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.clip = null;
        }
    }

    private void FixedUpdate()
    {
        if (!canMove) return;
        Movement();

    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && canMove)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            canMove = false;
            inGameMenu.SetActive(true);
            playerRb.linearVelocity = Vector3.zero;
        }

        CheckGroundStatus();
        //HandleFlipping();
    }

    void HandleFlipping()
    {
        if (horizontalInput > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (horizontalInput < 0 && isFacingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

    }

    void CheckGroundStatus()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if (isGrounded)
        {

        }
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public bool CanMove() => canMove;
    public bool IsPlayerJumping() => !isGrounded;
    public bool IsFacingRight() => isFacingRight;
}