using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    PlayerControl player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            player.scale = 1f;
            // player is damaged
            player.Damaged();
            // player moves to start position
            StartPoint startPoint = FindObjectOfType<StartPoint>();
            startPoint.MoveToStart();
            // camera moves to start position
            CameraManager camera = FindObjectOfType<CameraManager>();
            camera.clampedX = camera.clampedXInit;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            // don't destroy ground
        }
        else
        {
            // destroy gameobject
            Destroy(collision.gameObject);
        }
    }
}
