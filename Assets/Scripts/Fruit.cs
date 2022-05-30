using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    bool isRight;
    public float speed = 1f;
    public GameObject effectCollected;
    PlayerControl player;

    // Start is called before the first frame update
    void Start()
    {
        isRight = true;
        player = FindObjectOfType<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        // move
        if (isRight)
        {
            // move right
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        else
        {
            // move left
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // player
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // play sound
            SoundManager.instance.PlaySound(SoundManager.instance.audioScore);
            // spawn effect
            Instantiate(effectCollected, transform.position, transform.rotation);
            // size of player is double
            player.scale = 2f;
            // destroy gameobject
            Destroy(gameObject);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            
        }
        else
        {
            if (collision.contacts[0].normal.x > 0.9f || collision.contacts[0].normal.x < -0.9f)
            {
                // 충돌시 반대 방향으로 감
                isRight = !isRight;
            }
        }
    }
}
