using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Base ScriptableObject for ability data with timing phases
/// Pure timing + effect dispatch - no combat logic
/// </summary>
[CreateAssetMenu(fileName = "NewAbility", menuName = "Abilities/Ability Data")]
public class AbilityData : ScriptableObject
{
    [Header("Ability Info")]
    public string abilityName;
    public string description;
    
    [Header("Timing (in seconds)")]
    public float cooldown = 1f;
    public float windUpTime = 0.2f;
    public float activeTime = 0.3f;
    public float recoveryTime = 0.3f;
    
    [Header("Targeting")]
    public AbilityTargetType targetType;
    
    [Header("Effects")]
    public List<EffectData> effects = new List<EffectData>();
    
    /// <summary>
    /// Total duration of the ability execution
    /// </summary>
    public float TotalDuration => windUpTime + activeTime + recoveryTime;
    
    /// <summary>
    /// Called when ability is activated
    /// </summary>
    public virtual void OnActivate(AbilityContext context)
    {
        // Override in derived classes for specific behavior
    }
    
    /// <summary>
    /// Called during the active phase - dispatches effects
    /// </summary>
    public virtual void OnExecute(AbilityContext context)
    {
        // Dispatch all effects
        foreach (var effect in effects)
        {
            if (effect != null)
            {
                EffectContext effectContext = new EffectContext
                {
                    source = context.caster,
                    position = context.targetPosition,
                    direction = context.direction,
                    rotation = Quaternion.LookRotation(context.direction),
                    timestamp = context.executionTime
                };
                
                effect.Dispatch(effectContext);
            }
        }
    }
}

public enum AbilityTargetType
{
    Self,
    Direction,
    GroundTarget
}

/// <summary>
/// Context information passed during ability execution
/// </summary>
public class AbilityContext
{
    public GameObject caster;
    public Vector3 targetPosition;
    public Vector3 direction;
    public float executionTime;
}
