using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl instance;
    public float speed = 1f;
    public float forceJump = 150f;
    public GameObject effectCollected;
    Joystick joystick;
    public Animator animator;
    public Rigidbody2D rb;
    float h;
    float v;
    float timeAferDamaged;
    public float delayDamage = 1f;
    int life;
    bool isLookRight;
    bool isMoving;
    public bool isGrounded;
    bool isFalling;
    public bool bitJumpButtonUp;
    public int jumpCount;
    public bool isAtLadder;
    bool isJoystickUp;
    bool isJoystickDown;
    public bool isUsingLadder;
    public bool isGoingUpLadder;
    public bool isGoingDownLadder;
    float fallingDistance;
    public float fallingDistanceDamage = 0.6f;
    public Vector2 originalPosition;
    public float scale;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame upd   ate
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        isLookRight = true;
        isMoving = false;
        isGrounded = false;
        bitJumpButtonUp = false;
        jumpCount = 0;
        timeAferDamaged = delayDamage;
        isAtLadder = false;
        isUsingLadder = false;
        isGoingUpLadder = false;
        isGoingDownLadder = false;
        isJoystickUp = false;
        isJoystickDown = false;
        fallingDistance = 0f;
        originalPosition = transform.position;
        scale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        // 애니메이션
        animator.SetBool("Moving", isMoving);
        animator.SetBool("Grounded", isGrounded);
        animator.SetBool("Falling", isFalling);

        if (GameManager.instance.state != GameManager.State.Playing) return;

        // 조이스틱
        joystick = FindObjectOfType<Joystick>();

        if (joystick != null)
        {
            // 플레이어 이동
            float factorJoystick = 2f;
            h = joystick.Horizontal * factorJoystick;
            v = joystick.Vertical * factorJoystick;
            if (h > 1f) h = 1f;
            else if (h < -1f) h = -1f;
            if (v >= 1f) v = 1f;
            else if (v < -1f) v = -1f;
            float minMove = 0.1f;
            if (!isUsingLadder)
            {
                if (h > minMove & joystick.isTouched)
                {
                    isMoving = true;
                    isLookRight = true;
                    transform.Translate(Vector2.right * Time.deltaTime * speed * h);
                }
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    isMoving = true;
                    isLookRight = true;
                    transform.Translate(Vector2.right * Time.deltaTime * speed);
                }
                else if (h < -minMove & joystick.isTouched)
                {
                    isMoving = true;
                    isLookRight = false;
                    transform.Translate(Vector2.left * Time.deltaTime * speed * -h);
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    isMoving = true;
                    isLookRight = false;
                    transform.Translate(Vector2.left * Time.deltaTime * speed);
                }
                else
                {
                    isMoving = false;
                }
            }

            // 조이스틱 위 아래
            if (v > 0.9f || Input.GetKey(KeyCode.UpArrow))
            {
                isJoystickUp = true;
            }
            else if (v < -0.3f || Input.GetKey(KeyCode.DownArrow))
            {
                isJoystickDown = true;
            }
            else
            {
                isJoystickUp = false;
                isJoystickDown = false;
            }

            // 사다리에서 이동
            if (isUsingLadder)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    transform.Translate(Vector2.up * Time.deltaTime * speed);
                    isGoingUpLadder = true;
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    transform.Translate(Vector2.down * Time.deltaTime * speed);
                    isGoingDownLadder = true;
                }
                else if (v > minMove)
                {
                    transform.Translate(Vector2.up * Time.deltaTime * speed * v);
                    isGoingUpLadder = true;
                }
                else if (v < -minMove)
                {
                    transform.Translate(Vector2.down * Time.deltaTime * speed * -v);
                    isGoingDownLadder = true;
                }
                else
                {
                    rb.velocity = Vector2.zero;
                    isGoingUpLadder = false;
                    isGoingDownLadder = false;
                }

                // 사다리에서 벗어남
                float h_exitLadder = 0.99f;
                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) 
                    || h > h_exitLadder || h < -h_exitLadder)
                {
                    //NotUsingLadder();
                }
            }
            
        }

        // 사다리에서 조이스틱을 위/아래로 움직이면 사다리 이용
        if (isAtLadder && (isJoystickUp || isJoystickDown))
        {
            //UsingLadder();
        }

        // 플레이어가 바라보는 방향에 따라 스프라이트 회전
        if (isLookRight)
        {
            transform.localScale = new Vector2(scale, scale);
        }
        else
        {
            transform.localScale = new Vector2(-scale, scale);
        }

        // falling 판단
        if (!isGrounded && rb.velocity.y < 0)
        {
            isFalling = true;
            fallingDistance -= rb.velocity.y * Time.deltaTime;
        }
        else
        {
            //if (fallingDistance > fallingDistanceDamage) Damaged();
            fallingDistance = 0f;
            isFalling = false;
        }

        // jump(keyboard)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButton jumpButton = FindObjectOfType<JumpButton>();
            jumpButton.ButtonDown();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            JumpButton jumpButton = FindObjectOfType<JumpButton>();
            jumpButton.ButtonUp();
        }
        // 점프 버튼을 땠을 때 부드럽게 감속이 되도록
        if (bitJumpButtonUp)
        {
            float velocityY = rb.velocity.y;
            float decelY = 0.03f;
            if (velocityY > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, velocityY -= decelY);
            }
            else
            {
                bitJumpButtonUp = false;
            }
        }

        // 데미지 받고 지난 시간 계산(데미지 받고 delayDamage 시간이 지난 후에 다시 데미지를 받을 수 있음)
        if (timeAferDamaged < delayDamage) timeAferDamaged += Time.deltaTime;  
    }

    /*
    void UsingLadder()
    {
        animator.SetTrigger("UsingLadder");
        isUsingLadder = true;
        rb.gravityScale = 0;
    }

    public void NotUsingLadder()
    {
        animator.SetTrigger("NotUsingLadder");
        isAtLadder = false;
        isUsingLadder = false;
        isGoingUpLadder = false;
        isGoingDownLadder = false;
        rb.gravityScale = 1;
    }
    */

    public Vector2 Distance(Vector2 vector)
    {
        float x = transform.position.x - vector.x;
        float y = transform.position.y - vector.y;
        return new Vector2(x, y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        life = GameManager.instance.playerLife;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            float distY = Distance(new Vector2(collision.contacts[0].point.x, collision.contacts[0].point.y)).y;
            EnemyBehavior enemy = collision.gameObject.GetComponent<EnemyBehavior>();
            // player hits top of enemy
            if (distY > 0.04f)
            {
                enemy.Die();
            }
            // player is damaged
            else if (!enemy.isDead)
            {
                Damaged(collision);
            }
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("BlockBreak"))
        {
            // player hits bottom of block
            if (collision.contacts[0].point.y > transform.position.y)
            {
                // destrory block
                BlockBreak block = collision.gameObject.GetComponent<BlockBreak>();
                block.Break();
            }
        }

        // ground
        if (transform.position.y > collision.contacts[0].point.y)
        {
            isGrounded = true;
            jumpCount = 0;
        }    
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0)
        {
            isGrounded = true;
            jumpCount = 0;
        } 
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
        if (rb.velocity.y > 0) jumpCount++;
    }

    

    void Damaged(Collision2D collision)
    {
        EnemyBehavior enemy = collision.gameObject.GetComponent<EnemyBehavior>();
        if (!enemy.isStopped)
        {
            Damaged();
        } 
    }
    
    public void Damaged()
    {
        // show AD
        //GoogleAd ad = FindObjectOfType<GoogleAd>();
        //if (!ad.isFailedToLoad) ad.ShowAd();

        if (timeAferDamaged >= delayDamage)
        {
            timeAferDamaged = 0f;
            if (scale >= 2f)
            {
                SoundManager.instance.PlaySound(SoundManager.instance.audioNegative, 0.5f);
                scale = 1f;
            }
            else
            {
                GameManager.instance.ReducePlayerLife();
                animator.SetTrigger("Hit");
                life = GameManager.instance.playerLife;
            }
        }
    }
}
