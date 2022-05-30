using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionScore : MonoBehaviour
{
    PlayerControl player;
    public GameObject blockScored;
    public int score = 10;
    public GameObject floatingText;
    Rigidbody2D rb;
    Collider2D col;

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
            SoundManager.instance.PlaySound(SoundManager.instance.audioScore, 0.5f);
            // no gravity
            rb.gravityScale = 0f;
            // collider sets to trigger
            col.isTrigger = true;

            // instantiate scored block
            Instantiate(blockScored, transform.position, Quaternion.Euler(Vector2.zero));

            // add score
            GameManager.instance.AddScore(score);
            // add floating score
            GameObject goText = Instantiate(floatingText, transform.position, Quaternion.Euler(Vector2.zero));
            FloatingText text = goText.GetComponent<FloatingText>();
            text.value = score;

            // remove question block
            Destroy(gameObject);
        }
    }
}
