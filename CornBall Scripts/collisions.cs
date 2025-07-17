using System.Collections;
using System.Collections.Generic;
using System.Threading;

using UnityEngine;

public class collisions : MonoBehaviour
{
public static collisions instance;
    public bool hasCollided = false;
    void Start()
    {
        instance = this;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && hasCollided==false)
        {
            Debug.Log("Collided");
            GameManager.instance.score += 1;
            hasCollided = true;
        }
    }
}
