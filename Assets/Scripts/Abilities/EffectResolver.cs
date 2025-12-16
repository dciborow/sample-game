using UnityEngine;

/// <summary>
/// Resolves effects (hitboxes, areas) into game actions (damage, etc.)
/// This system interprets the primitive effects dispatched by abilities
/// </summary>
public class EffectResolver : MonoBehaviour
{
    [Header("Effect Interpretation")]
    public float baseDamage = 10f;
    public LayerMask targetLayers;
    
    private void Start()
    {
        // Subscribe to hitbox and area events
        StartCoroutine(ProcessEffects());
    }
    
    private System.Collections.IEnumerator ProcessEffects()
    {
        while (true)
        {
            // Find all active hitboxes
            Hitbox[] hitboxes = FindObjectsOfType<Hitbox>();
            foreach (var hitbox in hitboxes)
            {
                ProcessHitbox(hitbox);
            }
            
            // Find all active areas
            AreaMarker[] areas = FindObjectsOfType<AreaMarker>();
            foreach (var area in areas)
            {
                ProcessArea(area);
            }
            
            yield return new WaitForSeconds(0.05f); // Check every 50ms
        }
    }
    
    private void ProcessHitbox(Hitbox hitbox)
    {
        if (hitbox.gameObject.CompareTag("Processed"))
            return;
            
        // Mark as processed to avoid re-processing
        hitbox.gameObject.tag = "Processed";
        
        Collider[] hits = null;
        
        // Get colliders based on shape
        switch (hitbox.shape)
        {
            case HitboxShape.Sphere:
                hits = Physics.OverlapSphere(hitbox.transform.position, hitbox.radius);
                break;
            case HitboxShape.Box:
                hits = Physics.OverlapBox(hitbox.transform.position, hitbox.size / 2f, hitbox.transform.rotation);
                break;
            case HitboxShape.Cone:
                hits = Physics.OverlapSphere(hitbox.transform.position, hitbox.radius);
                break;
        }
        
        if (hits == null)
            return;
        
        foreach (var hit in hits)
        {
            // Skip the source
            if (hit.gameObject == hitbox.source)
                continue;
            
            // For cone, check angle
            if (hitbox.shape == HitboxShape.Cone)
            {
                Vector3 directionToTarget = (hit.transform.position - hitbox.transform.position).normalized;
                float angle = Vector3.Angle(hitbox.direction, directionToTarget);
                
                if (angle > hitbox.arcAngle / 2f)
                    continue;
            }
            
            // Apply effect to target
            ApplyEffectToTarget(hit.gameObject, hitbox.source);
        }
    }
    
    private void ProcessArea(AreaMarker area)
    {
        if (area.gameObject.CompareTag("Processed"))
            return;
            
        // Mark as processed to avoid re-processing
        area.gameObject.tag = "Processed";
        
        // Get all colliders in area
        Collider[] hits = Physics.OverlapSphere(area.transform.position, area.radius);
        
        foreach (var hit in hits)
        {
            // Skip the source
            if (hit.gameObject == area.source)
                continue;
            
            // Apply effect to target
            ApplyEffectToTarget(hit.gameObject, area.source);
        }
    }
    
    private void ApplyEffectToTarget(GameObject target, GameObject source)
    {
        // Apply damage to enemies if source is player
        if (source.CompareTag("Player") && target.CompareTag("Enemy"))
        {
            var enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(baseDamage);
            }
        }
        
        // Could extend to handle other effect types:
        // - Player taking damage from enemy effects
        // - Healing effects
        // - Status effects
        // - Knockback
        // etc.
    }
}
