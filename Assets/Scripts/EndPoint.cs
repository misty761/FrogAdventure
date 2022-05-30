using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    PlayerControl player;
    Vector2 dist;
    public float distLaser = 1f;
    public GameObject goLaser;
    bool isLaserEnabled;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerControl>();
        isLaserEnabled = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        dist = player.Distance(transform.position);

        // play is close
        if (!isLaserEnabled && dist.x > -distLaser)
        {
            // activate laser
            isLaserEnabled = true;
            goLaser.SetActive(true);
        }

        // go to the next level
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("End"))
        {
            GameManager.instance.NextLevel();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // collision with player
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // collision when player is left.
            if (dist.x < 0)
            {
                // player is damaged
                player.Damaged();
            }
            else
            {
                // go to the next level
                animator.SetTrigger("FlagOut");
            }
        }
        
    }
}
