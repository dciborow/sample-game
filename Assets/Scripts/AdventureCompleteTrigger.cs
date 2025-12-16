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
    
    private bool hasTriggered = false;
    
    void Start()
    {
        if (autoTriggerOnStart)
        {
            Invoke(nameof(TriggerAdventureComplete), autoTriggerDelay);
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            TriggerAdventureComplete();
        }
    }
    
    public void TriggerAdventureComplete()
    {
        if (hasTriggered)
            return;
            
        hasTriggered = true;
        CancelInvoke(nameof(TriggerAdventureComplete));
        GameEvents.TriggerAdventureComplete();
        // Disable this trigger after use
        enabled = false;
    }
}
