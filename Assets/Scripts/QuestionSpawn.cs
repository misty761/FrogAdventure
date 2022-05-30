using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionSpawn : MonoBehaviour
{
    PlayerControl player;
    public GameObject pfSpawn;
    public GameObject pfBlock;
    public Transform transformSpawn;
    Rigidbody2D rb;
    Collider2D col;
    public AudioClip audioSpawn;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerControl>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Player가 아래에서 충돌
        if (collision.contacts[0].point.y > player.transform.position.y
            && collision.contacts[0].point.normalized.y >= 0f
            && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // activate scored block & remove question block
            // play sound
            SoundManager.instance.PlaySound(audioSpawn, 0.5f);
            // no gravity
            rb.gravityScale = 0f;
            // collider sets to trigger
            col.isTrigger = true;

            // instantiate block
            Instantiate(pfBlock, transform.position, Quaternion.Euler(Vector2.zero));

            // Instantiate enemy
            GameObject go = Instantiate(pfSpawn, transformSpawn.position, Quaternion.Euler(Vector2.zero));
            try
            {
                EnemyBehavior enemy = go.GetComponent<EnemyBehavior>();
                enemy.isLookRight = true;
            }
            catch { }

            // remove question block
            Destroy(gameObject);
        }
    }
}
