using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float speed = 0.5f;
    //public float distanceMoveX = 30f;
    //public float originalX;
    //public float originalY;
    public float timeMove = 0f;
    public float jumpForce = 130f;
    public bool isStopped;
    public bool isLookRight;
    public bool isGrounded;
    float timeAfterStart;
    public Rigidbody2D rb;
    Animator animator;
    public int point = 10;
    public bool isDead;
    PlayerControl player;
    Vector2 dist;
    public float distanceMoveX = 2f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerControl>();
        //isLookRight = true;
        //originalX = transform.position.x;
        //originalY = transform.position.y;
        timeAfterStart = 0f;
        isStopped = true;
        isGrounded = false;
        isDead = false;
        speed += GameManager.instance.offsetEnemySpeed;
    }

    // Update is called once per frame
    void Update()
    {
        // return
        if (GameManager.instance.state != GameManager.State.Playing) return;
        if (GameManager.instance.state == GameManager.State.Cleared) return;

        // distance from player
        dist = player.Distance(transform.position);

        // move
        // close to player
        if (dist.x > -distanceMoveX && dist.x < distanceMoveX)
        {
            // move
            isStopped = false;
        }
        if(!isStopped)
        {
            // looking left
            if (!isLookRight)
            {
                // move left
                transform.Translate(Vector2.left * Time.deltaTime * speed);
            }
            // looking right
            else
            {
                // move right
                transform.Translate(Vector2.right * Time.deltaTime * speed);
            }
        }

        // 적이 바라 보는 방향에 따라 스프라이트 회전
        if (!isLookRight) transform.localScale = new Vector2(1, 1);
        else transform.localScale = new Vector2(-1, 1);

        // die
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("End"))
        {
            // destroy gameobject
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.x > 0.9f || collision.contacts[0].normal.x < -0.9f)
        {
            // 충돌시 반대 방향으로 감
            isLookRight = !isLookRight;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0) 
        {
            // touch ground
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // leave ground
        isGrounded = false;
    }

    public void Die()
    {
        // score
        if (!isDead) GameManager.instance.AddScore(point);
        // animation
        animator.SetTrigger("Hit");
        // not move
        speed = 0f;
        // dead
        isDead = true;
        // disable gravity
        rb.gravityScale = 0;
        // disable collider
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
        // stop
        rb.velocity = Vector2.zero;   
    }
}
