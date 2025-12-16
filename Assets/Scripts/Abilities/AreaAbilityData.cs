using UnityEngine;

/// <summary>
/// Ground-targeted area ability that spawns effects at target position
/// Pure timing - effects handle the rest
/// </summary>
[CreateAssetMenu(fileName = "NewAreaAbility", menuName = "Abilities/Area Ability")]
public class AreaAbilityData : AbilityData
{
    public override void OnExecute(AbilityContext context)
    {
        // Position area at target location
        EffectContext effectContext = new EffectContext
        {
            source = context.caster,
            position = context.targetPosition,
            direction = context.direction,
            rotation = Quaternion.identity,
            timestamp = context.executionTime
        };
        
        // Dispatch all effects
        foreach (var effect in effects)
        {
            if (effect != null)
            {
                effect.Dispatch(effectContext);
            }
        }
    }
}
