using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpButton : MonoBehaviour
{
    PlayerControl player;

    private void Start()
    {
        player = FindObjectOfType<PlayerControl>();
    }

    public void ButtonDown()
    {
        if (GameManager.instance.state == GameManager.State.Playing
            && player.jumpCount == 0)
        {
            // sound
            SoundManager.instance.PlaySound(SoundManager.instance.audioJump, 1f);
            // velocity sets to 0
            player.rb.velocity = new Vector2(player.rb.velocity.x, 0);
            // add force
            player.rb.AddForce(new Vector2(0, player.forceJump));
            // jump count ++
            player.jumpCount++;
        }
    }

    public void ButtonUp()
    {
        player.bitJumpButtonUp = true;
    }
}
