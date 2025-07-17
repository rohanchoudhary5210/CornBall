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
        if (collisions.instance.hasCollided == true && onGroundHit==false)
        {
            GameManager.instance.score -= 1;
            onGroundHit = true;
        }
    }
}
