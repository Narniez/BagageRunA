using System.Collections;
using UnityEngine;
public class CharacterController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 30f;
    [SerializeField] private float jumpForce = 5f;
    //[SerializeField] private float dashForce = 3f;

    [SerializeField] private float flyingForce = 1f;
    [SerializeField] private bool canSpecialJump = true;
    [SerializeField] float maxHoverTime = 4f;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    //[SerializeField] private float groundCheckDistance = 0.05f;

    [SerializeField] private GameObject inGameMenu;


    private float horizontalInput;
    private float verticalInput;
    private Rigidbody2D playerRb;


    private bool isGrounded;
    private bool isFacingRight = true;
    private bool isAttacking = false;
    private bool canMove = true;

    private float hoverTimer = 0f;


    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashTime = 0.2f;
    private float dashingCooldown = 1f;


    [SerializeField] private TrailRenderer trailRenderer;

    // private PlayerMeleeAttack meleeAttack;
    private Vector2 movement;


    [SerializeField] AudioClip walkingSound;
    [SerializeField] AudioClip dashSound;
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
        if (isDashing || !canMove) return;
        Movement();

    }
    void Update()
    {
        if (isDashing) return;

        if (Input.GetKeyDown(KeyCode.Space) && canMove)
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && canMove)
        {
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            canMove = false;
            inGameMenu.SetActive(true);
            playerRb.linearVelocity = Vector3.zero;
        }

        CheckGroundStatus();
        ApplyFlyingForce();
        HandleFlipping();
    }

    void ApplyFlyingForce()
    {
        if (canSpecialJump && !isGrounded)
        {
            hoverTimer += Time.deltaTime;

            if (hoverTimer <= maxHoverTime)
            {
                //Debug.Log($"Apply fly force. Hover time: {hoverTimer}");
                playerRb.AddForce(Vector2.up * flyingForce, ForceMode2D.Force);
            }
            else
            {
                //Debug.Log("Hover time limit reached");
            }
        }
        else
        {
            hoverTimer = 0f;
        }
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


    private IEnumerator Dash()
    {
        helperSource.PlayOneShot(dashSound);
        canDash = false;
        isDashing = true;
        float originalGravity = playerRb.gravityScale;
        playerRb.gravityScale = 0f;
        playerRb.linearVelocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        trailRenderer.emitting = true;

        yield return new WaitForSeconds(dashTime);
        trailRenderer.emitting = false;
        playerRb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;

    }

    void CheckGroundStatus()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if (isGrounded)
        {
            hoverTimer = 0f;
        }
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public bool CanMove() => canMove;
    public bool IsDashing() => isDashing;
    public bool IsPlayerJumping() => !isGrounded;
    public bool IsFacingRight() => isFacingRight;
    public void StartAttack() => isAttacking = true;
    public void EndAttack() => isAttacking = false;
}