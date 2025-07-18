using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundhit : MonoBehaviour
{
    public float speed;
    public static groundhit instance;
    public bool onGroundHit = false;
     void Start()
    {
        instance = this;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && onGroundHit == false)
        {

            onGroundHit = true;
            Debug.Log("Ground hit: " + onGroundHit);
        }
        Debug.Log("Ground hit: "+onGroundHit);
    }
}
