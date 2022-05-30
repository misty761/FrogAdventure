using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonRetry : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Retry()
    {
        GameManager.instance.currentStage = 1;
        GameManager.instance.Retry();
    }
}
