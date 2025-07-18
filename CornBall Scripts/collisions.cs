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
        if (hasCollided == false && collision.gameObject.CompareTag("Player"))
        {
            hasCollided = true;
            StartCoroutine(HandleGroundHit());
        }
        Debug.Log("Board hit: "+hasCollided);
    }
    IEnumerator HandleGroundHit()
    {
        
        yield return new WaitForSeconds(2f);
        if (groundhit.instance.onGroundHit == true)
        {
            Debug.Log("Ground hit: in collisions" + groundhit.instance.onGroundHit);
            GameManager.instance.score += 0;
        }
        else
        {
            GameManager.instance.score += 1;
            GameManager.instance.coins += 10;
        }
    }
}
