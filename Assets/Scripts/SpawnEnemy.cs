using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    Transform[] transforms;
    public GameObject pfEnemy;
    PlayerControl player;
    Vector2 dist;
    float distX;
    public float distSpawn = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerControl>();
        transforms = GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // distance from player
        dist = player.Distance(transform.position);
        distX = dist.x;

        // player is close
        if (distX > -distSpawn && distX < distSpawn)
        {
            // spawn enemy
            for (int i = 0; i < transforms.Length; i++)
            {
                Instantiate(pfEnemy, transforms[i].position, Quaternion.Euler(Vector2.zero));
            }

            // destroy gameobject
            Destroy(gameObject);
        }
    }
}
