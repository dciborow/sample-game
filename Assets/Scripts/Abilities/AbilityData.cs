using UnityEngine;

/// <summary>
/// Base ScriptableObject for ability data with timing phases
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
    
    [Header("Damage & Range")]
    public float damage = 10f;
    public float range = 2f;
    
    [Header("Targeting")]
    public AbilityTargetType targetType;
    public float areaOfEffectRadius = 0f;
    
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
    /// Called during the active phase
    /// </summary>
    public virtual void OnExecute(AbilityContext context)
    {
        // Override in derived classes for specific behavior
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
