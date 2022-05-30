using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenBlock : MonoBehaviour
{
    public GameObject block;
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
        // player 
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // activate block
            if (player.rb.velocity.y > 0f)
            {
                block.SetActive(true);
            }  
        }
    }
}
