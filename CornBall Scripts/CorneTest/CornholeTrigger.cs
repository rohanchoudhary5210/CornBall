using UnityEngine;

/// <summary>
/// Detects when the sandbag enters the hole's trigger collider.
/// Awards points based on whether the bag was on the board first.
/// Renamed from 'Pointer.cs'.
/// </summary>
public class CornholeTrigger : MonoBehaviour
{
    private const int POINTS_FOR_AIRMAIL = 3; // Straight in the hole
    private const int COINS_FOR_AIRMAIL = 50;
    private const int POINTS_FOR_SLIDE_IN = 2; // Landed on board, then slid in
    private const int COINS_FOR_SLIDE_IN = 20;
    private const float TIME_BONUS = 10f;

    void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered is the sandbag
        if (other.gameObject.CompareTag("Player"))
        {
            SandbagController sandbag = other.gameObject.GetComponent<SandbagController>();

            // Ensure we have a sandbag and it hasn't already scored
            if (sandbag != null && !sandbag.HasScoredInHole)
            {
                // Mark that it has scored to prevent duplicate scoring
                sandbag.HasScoredInHole = true;

                if (sandbag.HasLandedOnBoard)
                {
                    // It was on the board first, then slid in. This is worth 2 points.
                    // The board collision script already gave 1 point, so we add 1 more.
                    Debug.Log("Slid in the hole! +2 points total.");
                    GameManager.Instance.AddScore(POINTS_FOR_SLIDE_IN);
                    GameManager.Instance.AddCoins(COINS_FOR_SLIDE_IN);
                }
                else
                {
                    // It went straight in without touching the board ("Airmail"). Worth 3 points.
                    Debug.Log("Airmail! +3 points.");
                    GameManager.Instance.AddScore(POINTS_FOR_AIRMAIL);
                    GameManager.Instance.AddCoins(COINS_FOR_AIRMAIL);
                }

                // Give a time bonus for scoring in the hole
                GameManager.Instance.AddTime(TIME_BONUS);
            }
        }
    }
}
