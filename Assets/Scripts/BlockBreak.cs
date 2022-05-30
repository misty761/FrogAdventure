using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBreak : MonoBehaviour
{
    // part
    public GameObject part;
    // audio
    public AudioClip audioBreak;
    // point
    public int point = 10;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Break()
    {
        
        float offsetX = 0.08f;
        float offsetY = 0.08f;
        int forceX = 50;
        int forceY = 100;
        // score ++
        //GameManager.instance.AddScore(point);
        // sound
        SoundManager.instance.PlaySound(audioBreak, 1f);
        // make part
        MakePart(offsetX, offsetY, -forceX, forceY * 2);
        MakePart(-offsetX, offsetY, forceX, forceY * 2);
        MakePart(offsetX, -offsetY, -forceX - 30, forceY);
        MakePart(-offsetX, -offsetY, forceX + 30, forceY);
        // destory
        Destroy(gameObject);
    }

    void MakePart(float x, float y, float forceX, float forceY)
    {
        GameObject goPart = Instantiate(part, new Vector2(transform.position.x + x, transform.position.y + y), Quaternion.Euler(Vector2.zero));
        Rigidbody2D rb = goPart.GetComponent<Rigidbody2D>();
        rb.AddForce(new Vector2(forceX, forceY));
    }
}
