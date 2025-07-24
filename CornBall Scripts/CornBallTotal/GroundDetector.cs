using UnityEngine;

/// <summary>
/// Detects when the sandbag hits the ground.
/// It updates the sandbag's state directly.
/// </summary>
public class GroundDetector : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // Check if the object that hit is the sandbag
        if (collision.gameObject.CompareTag("Player"))
        {
            SandbagController sandbag = collision.gameObject.GetComponent<SandbagController>();
            
            // If we found a sandbag component and it hasn't already been marked as hitting the ground
            if (sandbag != null && !sandbag.HasHitGround)
            {
                Debug.Log("Sandbag hit the ground.");
                sandbag.HasHitGround = true;
            }
        }
    }
}
