using UnityEngine;

/// <summary>
/// Ground-targeted area ability
/// </summary>
[CreateAssetMenu(fileName = "NewAreaAbility", menuName = "Abilities/Area Ability")]
public class AreaAbilityData : AbilityData
{
    [Header("Visual Settings")]
    public GameObject areaIndicatorPrefab;
    
    public override void OnExecute(AbilityContext context)
    {
        base.OnExecute(context);
        
        // Deal damage to all enemies in the target area
        Collider[] hits = Physics.OverlapSphere(context.targetPosition, areaOfEffectRadius);
        
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                var enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }
        
        // Create visual effect at target position
        if (areaIndicatorPrefab != null)
        {
            GameObject vfx = Instantiate(areaIndicatorPrefab, context.targetPosition, Quaternion.identity);
            Destroy(vfx, 1f);
        }
    }
}
