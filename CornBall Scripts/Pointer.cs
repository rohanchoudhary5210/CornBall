using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    public static Pointer instance;
    public bool hasCornHole = false;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && hasCornHole == false)
        {
            if (collisions.instance.hasCollided == true)
            {
                GameManager.instance.score += 2;
                GameManager.instance.coins += 20;
            }
            else
            {
                GameManager.instance.score += 3;
                GameManager.instance.coins += 50;
            }
            hasCornHole = true;
        }
    }
}
