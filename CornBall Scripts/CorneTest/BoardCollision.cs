using UnityEngine;
using System.Collections;

/// <summary>
/// Detects when the sandbag collides with the cornhole board.
/// It communicates with the GameManager to award points.
/// Renamed from 'collisions.cs'.
/// </summary>
public class BoardCollision : MonoBehaviour
{
    private const int POINTS_FOR_BOARD = 1;
    private const int COINS_FOR_BOARD = 10;

    void OnCollisionEnter(Collision collision)
    {
        // Check if the object that hit is the sandbag
        if (collision.gameObject.CompareTag("Player"))
        {
            SandbagController sandbag = collision.gameObject.GetComponent<SandbagController>();
            if (sandbag != null && !sandbag.HasLandedOnBoard)
            {
                // Mark that the bag has landed on the board
                sandbag.HasLandedOnBoard = true;
                
                // Use a coroutine to wait and see if it hit the ground first
                StartCoroutine(AwardPointsAfterDelay(sandbag));
            }
        }
    }

    /// <summary>
    /// Waits a moment before awarding points to ensure the ground isn't hit first.
    /// </summary>
    private IEnumerator AwardPointsAfterDelay(SandbagController sandbag)
    {
        // Wait for a short duration to allow ground collision to register
        yield return new WaitForSeconds(0.5f);

        // Award points only if it hasn't hit the ground and hasn't already scored in the hole
        if (!sandbag.HasHitGround && !sandbag.HasScoredInHole)
        {
            Debug.Log("Landed on board! +1 point.");
            GameManager.Instance.AddScore(POINTS_FOR_BOARD);
            GameManager.Instance.AddCoins(COINS_FOR_BOARD);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // This logic might need review. If a bag slides off the board and then into the hole,
        // you might not want to deduct points. For now, this is commented out as the new
        // system is more robust.
        /*
        if (collision.gameObject.CompareTag("Player"))
        {
            SandbagController sandbag = collision.gameObject.GetComponent<SandbagController>();
            if (sandbag != null && sandbag.HasLandedOnBoard)
            {
                // If it leaves the board, it should lose the point it gained.
                // But only if it hasn't already scored in the hole.
                if(!sandbag.HasScoredInHole)
                {
                    Debug.Log("Fell off board! -1 point.");
                    GameManager.Instance.AddScore(-POINTS_FOR_BOARD);
                    GameManager.Instance.AddCoins(-COINS_FOR_BOARD);
                    sandbag.HasLandedOnBoard = false;
                }
            }
        }
        */
    }
}
