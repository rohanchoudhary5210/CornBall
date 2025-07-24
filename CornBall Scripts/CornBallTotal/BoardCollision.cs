using UnityEngine;

/// <summary>
/// Detects when the sandbag collides with the cornhole board.
/// It ONLY sets a flag on the sandbag. Scoring is handled by the SandbagController.
/// </summary>
public class BoardCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SandbagController sandbag = collision.gameObject.GetComponent<SandbagController>();
            if (sandbag != null && !sandbag.HasLandedOnBoard)
            {
                // Set the flag. Do NOT award points here.
                sandbag.HasLandedOnBoard = true;
                Debug.Log("Flag set: HasLandedOnBoard");
            }
        }
    }
}
