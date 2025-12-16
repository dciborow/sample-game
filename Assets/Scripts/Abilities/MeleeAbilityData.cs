using UnityEngine;

/// <summary>
/// Melee ability that spawns a hitbox in front of the caster
/// Pure timing - effects handle the rest
/// </summary>
[CreateAssetMenu(fileName = "NewMeleeAbility", menuName = "Abilities/Melee Ability")]
public class MeleeAbilityData : AbilityData
{
    public override void OnExecute(AbilityContext context)
    {
        // Position hitbox in front of caster
        EffectContext effectContext = new EffectContext
        {
            source = context.caster,
            position = context.caster.transform.position,
            direction = context.direction,
            rotation = Quaternion.LookRotation(context.direction),
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
