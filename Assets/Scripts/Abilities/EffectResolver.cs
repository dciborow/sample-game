using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Resolves effects (hitboxes, areas) into game actions (damage, etc.)
/// This system interprets the primitive effects dispatched by abilities
/// </summary>
public class EffectResolver : MonoBehaviour
{
    [Header("Effect Interpretation")]
    public LayerMask targetLayers;
    
    private static EffectResolver instance;
    private HashSet<int> processedEffects = new HashSet<int>();
    
    private void Awake()
    {
        instance = this;
    }
    
    /// <summary>
    /// Called by effects when they are spawned
    /// </summary>
    public static void RegisterEffect(GameObject effect)
    {
        if (instance != null)
        {
            instance.ProcessEffect(effect);
        }
    }
    
    private void ProcessEffect(GameObject effect)
    {
        int effectId = effect.GetInstanceID();
        if (processedEffects.Contains(effectId))
            return;
            
        processedEffects.Add(effectId);
        
        // Process hitbox
        var hitbox = effect.GetComponent<Hitbox>();
        if (hitbox != null)
        {
            ProcessHitbox(hitbox);
            return;
        }
        
        // Process area
        var area = effect.GetComponent<AreaMarker>();
        if (area != null)
        {
            ProcessArea(area);
            return;
        }
    }
    
    private void ProcessHitbox(Hitbox hitbox)
    {
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
            
            // Apply effect to target with hitbox-specific damage
            ApplyEffectToTarget(hit.gameObject, hitbox.source, hitbox.damage);
        }
    }
    
    private void ProcessArea(AreaMarker area)
    {
        // Get all colliders in area
        Collider[] hits = Physics.OverlapSphere(area.transform.position, area.radius);
        
        foreach (var hit in hits)
        {
            // Skip the source
            if (hit.gameObject == area.source)
                continue;
            
            // Apply effect to target with area-specific damage
            ApplyEffectToTarget(hit.gameObject, area.source, area.damage);
        }
    }
    
    private void ApplyEffectToTarget(GameObject target, GameObject source, float damage)
    {
        // Skip applying damage to the source itself
        if (target == source)
            return;
        
        // Apply damage through the IDamageable contract
        var damageable = target.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage((int)damage, target.transform.position);
        }
        
        // Could extend to handle other effect types:
        // - Healing effects
        // - Status effects
        // - Knockback
        // etc.
    }
}
