using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    PlayerControl player;
    public float offsetX = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerControl>();
        MoveToStart();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToStart()
    {
        Vector2 pos = new Vector2(transform.position.x + offsetX, transform.position.y);
        player.transform.position = pos;
    }
}
