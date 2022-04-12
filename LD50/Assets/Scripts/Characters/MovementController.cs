using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovementController : MonoBehaviour
{
    CharacterController character;
    Rigidbody2D rb;
    Collider2D collider;
    CharacterAnimation _animation;
    public SoundManager audios;
    public AudioSource selfAudio;

    [SerializeField]
    bool simplified = false;
    [SerializeField]
    LayerMask groundLayerMask;

    [Header("Speed")]
    [SerializeField, Tooltip("jaja")]
    float movementSpeed = 10;
    [SerializeField]
    float airMovementSpeed = 7;
    [SerializeField]
    float maxFallSpeed = 50f;

    [Header("Jumps")]
    [SerializeField]
    float jumpHeight = 1;
    [SerializeField]
    float secondJumpHeight = 1;

    [Header("Dash")]
    [SerializeField]
    float dashSpeed = 10;
    [SerializeField]
    float dashDrag = 10;
    [SerializeField]
    float minDashSpeed = 1f;



    [Header("Physics")]
    [SerializeField]
    float defaultGravity = 1f;
    [SerializeField]
    [Range(0f, 1f)]
    float reducedGravityMultiplier = 0.5f;

    [Header("Extra")]
    [SerializeField]
    float coyoteTime = 0.05f;
    float lastTimeGrounded = 0;

    [SerializeField]
    float jumpBufferTime = 0.05f;
    float timeWhenJumpWasBuffered = 0f;
    bool jumpBuffered = false;

    [Header("Abilities")]
    [SerializeField]
    public bool canFly = false;
    [SerializeField]
    bool canDoubleJump = true;
    [SerializeField]
    bool canDash = true;

    bool canMove = true;
    bool dashAvailable = true;
    bool doubleJumpAvailable;

    bool onGround = false;
    bool onAir = false;
    bool dashing = false;
    bool floating = false;

    Vector3 input = Vector3.zero;

    [Header("Abilities")]
    [SerializeField]
    Transform directionTarget;

    [SerializeField]
    Transform feet;
    [SerializeField]
    GameObject jumpSmokePrefab;
    [SerializeField]
    GameObject dashPrefab;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _animation = GetComponent<CharacterAnimation>();
        collider = GetComponent<Collider2D>();
        audios = GameObject.FindObjectOfType<SoundManager>().GetComponent<SoundManager>();
        selfAudio = GetComponent<AudioSource>();
        character = GetComponent<CharacterController>();

        simplified = TryGetComponent<Enemy>(out Enemy e);

        ConfigureHabilities();

    }

    private void ConfigureHabilities()
    {
        canDash = !GameManager.achivedFlowers.Contains(FlowerTypes.DASH);
        canDoubleJump = !GameManager.achivedFlowers.Contains(FlowerTypes.DOUBLE_JUMP);
    }

    private void FixedUpdate()
    {

        GroundCheck();

        if (!simplified)
        {



            UpdateTarget();

            if (onGround && rb.velocity.y <= 0)
            {
                ResetGravity();

                lastTimeGrounded = Time.time;

                dashAvailable = canDash;
                doubleJumpAvailable = canDoubleJump;

                onAir = false;
                floating = false;

                if (jumpBuffered && Time.time < jumpBufferTime + timeWhenJumpWasBuffered)
                {
                    print("Buffered jump");
                    Jump();
                }
                jumpBuffered = false;
            }
            else if (!onGround)
            {
                if (Time.time > lastTimeGrounded + coyoteTime)
                {
                    onAir = true;
                }
            }

            //Check if character is dashing and the dash is not finished
            CheckEndOfDash();

            //If not floating reset gravity to default
            if (!floating)
            {
                rb.gravityScale = Mathf.MoveTowards(rb.gravityScale, defaultGravity, 30f * Time.fixedDeltaTime);
            }
            floating = false;
        }

        _animation.SetGround(onGround);

        if (!canMove || dashing)
            return;

        if (Mathf.Abs(input.x) > 0.01f)
        {
            character.direction = Mathf.RoundToInt(Mathf.Clamp(input.x * 1000f, -1f, 1f));
            _animation.SetFlip(character.direction < 0);
        }
        _animation.SetSpeedX(Mathf.Abs(input.x));

        if (input.y == 0)
        {
            input.y = rb.velocity.y;
        }

        if (input.y < -maxFallSpeed)
            input.y = -maxFallSpeed;

        if (!simplified)
        {
            float extra = 0.05f;
            RaycastHit2D hit = Physics2D.BoxCast(collider.bounds.center + character.direction * Vector3.right * (collider.bounds.size.x / 2f), new Vector3(extra, collider.bounds.size.y - 0.1f, 0f), character.direction * 90f, Vector2.right * character.direction, extra, groundLayerMask);
            if (hit.collider != null)
            {
                input.x = 0;
            }
        }

        rb.velocity = input;

        input = Vector3.zero;//Reset input for next frame
    }

    private void GroundCheck()
    {
        if (canFly)
            return;

        float extra = 0.1f;
        RaycastHit2D hit = Physics2D.BoxCast(collider.bounds.center + Vector3.down * (collider.bounds.size.y / 2f), new Vector3(collider.bounds.size.x - 0.1f, extra, 0f), 0f, Vector2.down, extra, groundLayerMask);
        onGround = hit.collider != null;

        if (onGround && hit.collider.TryGetComponent<Platform>(out Platform platform))
        {
            transform.parent = platform.transform;
        }
        else
        {
            transform.parent = null;
        }

#if UNITY_EDITOR
        Color rayColor;
        if (onGround)
            rayColor = Color.green;
        else
            rayColor = Color.red;
        Debug.DrawRay(collider.bounds.center, Vector2.down * ((collider.bounds.size.y / 2f) + (extra / 2f)), rayColor);
#endif
    }

    public void Move(float x)
    {
        if (onGround)
        {
            if (TryGetComponent<Player>(out Player _player))
            {
                audios.AudioSelect(SoundManager.audioType.Paso1, selfAudio);
            }
            input.x = x * movementSpeed;
        }
        else
            input.x = x * airMovementSpeed;
    }

    public void Move(float x, float y)
    {
        if (onGround)
        {   
            input.x = x * movementSpeed;
            input.y = y * movementSpeed;
        }
        else
        {
            input.x = x * airMovementSpeed;
            input.y = y * airMovementSpeed;
        }
    }


    public void Jump()
    {        
        if (onAir)
        {
            if (!dashing && doubleJumpAvailable)
            {
                audios.AudioSelect(SoundManager.audioType.Jump, selfAudio);
                input.y = Mathf.Sqrt(secondJumpHeight * Mathf.Abs(defaultGravity * Physics2D.gravity.y));
                doubleJumpAvailable = false;
                _animation.Jump();
                GameObject.Instantiate(jumpSmokePrefab, feet.position, Quaternion.identity);
            }
            else
            {
                jumpBuffered = true;
                timeWhenJumpWasBuffered = Time.time;
            }
        }
        else
        {
            audios.AudioSelect(SoundManager.audioType.Jump, selfAudio);
            input.y = Mathf.Sqrt(jumpHeight * Mathf.Abs(defaultGravity * Physics2D.gravity.y));
            _animation.Jump();
            GameObject.Instantiate(jumpSmokePrefab, feet.position, Quaternion.identity);
            onAir = true;
        }
    }

    public void Float()
    {
        floating = true;
        rb.gravityScale = defaultGravity * reducedGravityMultiplier;
    }
    public void ResetGravity()
    {
        rb.gravityScale = defaultGravity;
    }

    public void Dash()
    {
        if (dashing || !dashAvailable || !canDash)
            return;
        audios.AudioSelect(SoundManager.audioType.Dash, selfAudio);
        rb.gravityScale = 0;
        rb.velocity = new Vector2(character.direction * dashSpeed, 0);
        rb.drag = dashDrag;
        dashAvailable = false;
        dashing = true;

        AnimatedEffect dashEffect = GameObject.Instantiate(dashPrefab, transform.position + Vector3.right * character.direction, Quaternion.identity).GetComponent<AnimatedEffect>();
        dashEffect.user = character;
        if (character.direction < 0)
            dashEffect.Flip();
        dashEffect.transform.parent = transform;

        _animation.Dash();
    }

    public void CheckEndOfDash()
    {
        if (dashing && Mathf.Abs(rb.velocity.x) < minDashSpeed)
        {
            rb.gravityScale = defaultGravity;
            rb.drag = 0;
            dashing = false;
            _animation.StopDash();

            if (jumpBuffered && Time.time < jumpBufferTime + timeWhenJumpWasBuffered)
            {
                print("Buffered jump after dash");
                Jump();
            }
        }
    }

    internal void Knock(Vector2 velocity)
    {
        canMove = false;
        rb.velocity = velocity;

        //rb.AddForce(velocity, ForceMode2D.Impulse);
        //rb.velocity = new Vector2(0, Mathf.Sqrt(jumpHeight * Mathf.Abs(defaultGravity * Physics2D.gravity.y)));
    }

    public void Recover()
    {
        canMove = true;
    }

    private void UpdateTarget()
    {
        //Moving target
        if (directionTarget)
        {
            Vector2 target = rb.velocity;
            if (target.magnitude > 1)
                target.Normalize();

            directionTarget.localPosition = target;
        }
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent<Platform>(out Platform platform) && onGround)
        {
            transform.parent = platform.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.transform == transform.parent)
        {
            transform.parent = null;
        }
    }*/
}
