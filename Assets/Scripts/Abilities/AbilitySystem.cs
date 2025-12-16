using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Manages ability execution, cooldowns, and state
/// </summary>
public class AbilitySystem : MonoBehaviour
{
    [System.Serializable]
    public class AbilitySlot
    {
        public AbilityData ability;
        [HideInInspector] public float cooldownRemaining;
        [HideInInspector] public bool isOnCooldown;
    }
    
    [Header("Abilities")]
    public List<AbilitySlot> abilities = new List<AbilitySlot>();
    
    [Header("Current State")]
    public AbilityState currentState = AbilityState.Idle;
    public float stateTimer;
    
    private AbilityData currentAbility;
    private AbilityContext currentContext;
    
    void Update()
    {
        // Update cooldowns
        foreach (var slot in abilities)
        {
            if (slot.isOnCooldown)
            {
                slot.cooldownRemaining -= Time.deltaTime;
                if (slot.cooldownRemaining <= 0)
                {
                    slot.cooldownRemaining = 0;
                    slot.isOnCooldown = false;
                }
            }
        }
        
        // Update ability state machine
        UpdateAbilityState();
    }
    
    private void UpdateAbilityState()
    {
        if (currentState == AbilityState.Idle)
            return;
            
        stateTimer -= Time.deltaTime;
        
        if (stateTimer <= 0)
        {
            switch (currentState)
            {
                case AbilityState.WindUp:
                    TransitionToActive();
                    break;
                case AbilityState.Active:
                    TransitionToRecovery();
                    break;
                case AbilityState.Recovery:
                    TransitionToIdle();
                    break;
            }
        }
    }
    
    public bool TryUseAbility(int abilityIndex, Vector3 targetPosition, Vector3 direction)
    {
        if (currentState != AbilityState.Idle)
            return false;
            
        if (abilityIndex < 0 || abilityIndex >= abilities.Count)
            return false;
            
        var slot = abilities[abilityIndex];
        
        if (slot.ability == null || slot.isOnCooldown)
            return false;
            
        // Start ability execution
        currentAbility = slot.ability;
        currentContext = new AbilityContext
        {
            caster = gameObject,
            targetPosition = targetPosition,
            direction = direction,
            executionTime = Time.time
        };
        
        // Activate ability
        currentAbility.OnActivate(currentContext);
        
        // Start wind-up phase
        currentState = AbilityState.WindUp;
        stateTimer = currentAbility.windUpTime;
        
        // Put on cooldown
        slot.cooldownRemaining = currentAbility.cooldown;
        slot.isOnCooldown = true;
        
        return true;
    }
    
    private void TransitionToActive()
    {
        currentState = AbilityState.Active;
        stateTimer = currentAbility.activeTime;
        
        // Execute ability effect
        currentAbility.OnExecute(currentContext);
    }
    
    private void TransitionToRecovery()
    {
        currentState = AbilityState.Recovery;
        stateTimer = currentAbility.recoveryTime;
    }
    
    private void TransitionToIdle()
    {
        currentState = AbilityState.Idle;
        stateTimer = 0;
        currentAbility = null;
        currentContext = null;
    }
    
    public bool IsAbilityReady(int abilityIndex)
    {
        if (abilityIndex < 0 || abilityIndex >= abilities.Count)
            return false;
            
        var slot = abilities[abilityIndex];
        return slot.ability != null && !slot.isOnCooldown;
    }
    
    public float GetCooldownPercent(int abilityIndex)
    {
        if (abilityIndex < 0 || abilityIndex >= abilities.Count)
            return 0;
            
        var slot = abilities[abilityIndex];
        if (slot.ability == null || !slot.isOnCooldown)
            return 0;
            
        return slot.cooldownRemaining / slot.ability.cooldown;
    }
}

public enum AbilityState
{
    Idle,
    WindUp,
    Active,
    Recovery
}
