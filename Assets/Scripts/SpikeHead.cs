using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeHead : MonoBehaviour
{
    PlayerControl player;
    bool isMoving;
    public float distMove = 0.1f;
    public float speed = 1f;
    public float timeMove;
    public float timeDestroy = 10f;
    Vector2 dist;
    float distX;
    public enum DirectionMove
    {
        Up,
        Down
    }
    public DirectionMove directionMove;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerControl>();
        isMoving = false;
        timeMove = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // distance
        dist = player.Distance(transform.position);
        distX = dist.x;

        // close to player
        if (distX > -distMove && distX < distMove) isMoving = true;

        // move
        if (isMoving)
        {
            if (directionMove == DirectionMove.Up)
            {
                // move up
                transform.Translate(Vector2.up * speed * Time.deltaTime);
                timeMove += Time.deltaTime;
            }
            else if (directionMove == DirectionMove.Down)
            {
                // move down
                transform.Translate(Vector2.down * speed * Time.deltaTime);
                timeMove += Time.deltaTime;
            }
            
        }

        // destroy
        if (timeMove > timeDestroy)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // moving
        if (isMoving)
        {
            // player
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                // player is damaged
                player.Damaged();
            }
            /*
            // ground
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                // destroy gameobject
                Destroy(gameObject);
            }
            */
        }    
    }
}
