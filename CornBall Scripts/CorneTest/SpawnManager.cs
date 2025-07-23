using UnityEngine;

/// <summary>
/// Responsible only for spawning new sandbags.
/// It is controlled by the GameManager.
/// </summary>
public class SpawnManager : MonoBehaviour
{
    // Assign the sandbag prefab in the Unity Inspector
    [SerializeField] private GameObject sandbagPrefab;
    [SerializeField] private Transform spawnPoint;
    
    /// <summary>
    /// Instantiates a new sandbag at the designated spawn point.
    /// </summary>
    public void SpawnSandbag()
    {
        if (sandbagPrefab != null && spawnPoint != null)
        {
            // Instantiate the new sandbag and let it handle its own logic.
            Instantiate(sandbagPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogError("SpawnManager is missing Sandbag Prefab or Spawn Point reference!");
        }
    }
}
