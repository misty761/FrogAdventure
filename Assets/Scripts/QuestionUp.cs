using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionUp : MonoBehaviour
{
    PlayerControl player;
    Vector2 positionOrigin;
    public float speed = 1f;
    bool bMoveUp;
    public float offsetUp = 0.64f;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerControl>();
        positionOrigin = transform.position;
        bMoveUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (bMoveUp)
        {
            if (transform.position.y < positionOrigin.y + offsetUp)
            {
                transform.Translate(Time.deltaTime * speed * Vector2.up);
            }
            else
            {
                positionOrigin = transform.position;
                bMoveUp = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Player가 아래에서 충돌
        if (collision.contacts[0].point.y > player.transform.position.y
            && collision.contacts[0].point.normalized.y >= 0f
            && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // move up
            bMoveUp = true;
        }
    }
}
