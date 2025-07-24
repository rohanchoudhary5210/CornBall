using UnityEngine;

/// <summary>
/// Detects when the sandbag enters the hole's trigger collider.
/// It ONLY sets a flag on the sandbag. Scoring is handled by the SandbagController.
/// </summary>
public class CornholeTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SandbagController sandbag = other.gameObject.GetComponent<SandbagController>();
            if (sandbag != null && !sandbag.HasScoredInHole)
            {
                // Set the flag. Do NOT award points here.
                sandbag.HasScoredInHole = true;
                Debug.Log("Flag set: HasScoredInHole");
            }
        }
    }
}
