using UnityEngine;

/// <summary>
/// Simple trigger to signal adventure completion.
/// Can be attached to a trigger zone or called manually.
/// </summary>
public class AdventureCompleteTrigger : MonoBehaviour
{
    [Header("Trigger Settings")]
    public bool autoTriggerOnStart = false;
    public float autoTriggerDelay = 5f;
    
    void Start()
    {
        if (autoTriggerOnStart)
        {
            Invoke(nameof(TriggerAdventureComplete), autoTriggerDelay);
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerAdventureComplete();
        }
    }
    
    public void TriggerAdventureComplete()
    {
        GameEvents.TriggerAdventureComplete();
        // Disable this trigger after use
        enabled = false;
    }
}
