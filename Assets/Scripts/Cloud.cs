using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public GameObject pfSpikeHead;

    // Start is called before the first frame update
    void Start()
    {
        
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
            // play sound
            SoundManager.instance.PlaySound(SoundManager.instance.audioDenied, 0.5f);

            // spawn spike head(down)
            Instantiate(pfSpikeHead, transform.position, Quaternion.Euler(Vector2.zero));
        }
    }
}
