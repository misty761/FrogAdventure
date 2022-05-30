using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockFall : MonoBehaviour
{
    PlayerControl player;
    Vector2 dist;
    float distX;
    public float distMove = 0.4f;
    bool isfalling;
    public float speed = 2f;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerControl>();
        isfalling = false;
    }

    // Update is called once per frame
    void Update()
    {
        // distance
        dist = player.Distance(transform.position);
        distX = dist.x;

        // close to player
        if (distX > -distMove && distX < distMove)
        {
            // fall
            isfalling = true;
        }

        if (isfalling)
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // falling
        if (isfalling)
        {
            // player
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                // player is bellow
                if (transform.position.y > collision.transform.position.y)
                {
                    // player damaged
                    player.Damaged();
                }
            }
            // ground
            else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                // destroy gameobject
                Destroy(gameObject);
            }
        }
    }
}
