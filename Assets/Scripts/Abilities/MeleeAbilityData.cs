using UnityEngine;

/// <summary>
/// Melee ability that damages enemies in front of the player
/// </summary>
[CreateAssetMenu(fileName = "NewMeleeAbility", menuName = "Abilities/Melee Ability")]
public class MeleeAbilityData : AbilityData
{
    [Header("Melee Settings")]
    public float arcAngle = 90f;
    
    public override void OnExecute(AbilityContext context)
    {
        base.OnExecute(context);
        
        // Find all enemies in range
        Collider[] hits = Physics.OverlapSphere(context.caster.transform.position, range);
        
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                // Check if enemy is in front arc
                Vector3 directionToEnemy = (hit.transform.position - context.caster.transform.position).normalized;
                float angle = Vector3.Angle(context.direction, directionToEnemy);
                
                if (angle <= arcAngle / 2f)
                {
                    // Deal damage
                    var enemy = hit.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        enemy.TakeDamage(damage);
                    }
                }
            }
        }
    }
}
