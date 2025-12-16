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
        // Override position to be at caster location for melee
        context.targetPosition = context.caster.transform.position;
        base.OnExecute(context);
    }
}
