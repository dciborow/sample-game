using UnityEngine;

/// <summary>
/// Displays debug information on screen
/// </summary>
public class DebugDisplay : MonoBehaviour
{
    public PlayerController playerController;
    public AbilitySystem abilitySystem;
    public PlayerHealth playerHealth;
    public EncounterController encounterController;
    
    void OnGUI()
    {
        if (playerController == null || abilitySystem == null || playerHealth == null)
            return;
            
        GUILayout.BeginArea(new Rect(10, 10, 300, 400));
        GUILayout.Label("=== Player Status ===");
        GUILayout.Label($"Health: {playerHealth.GetHealthPercent() * 100:F0}%");
        GUILayout.Label($"State: {abilitySystem.currentState}");
        
        GUILayout.Space(10);
        GUILayout.Label("=== Cooldowns ===");
        GUILayout.Label($"Dodge: {(playerController.IsDodgeReady() ? "Ready" : $"{playerController.GetDodgeCooldownPercent() * 100:F0}%")}");
        
        for (int i = 0; i < abilitySystem.abilities.Count; i++)
        {
            var ability = abilitySystem.abilities[i];
            if (ability.ability != null)
            {
                bool ready = abilitySystem.IsAbilityReady(i);
                string status = ready ? "Ready" : $"{abilitySystem.GetCooldownPercent(i) * 100:F0}%";
                GUILayout.Label($"{ability.ability.abilityName}: {status}");
            }
        }
        
        GUILayout.Space(10);
        GUILayout.Label("=== Encounter ===");
        if (encounterController != null)
        {
            if (encounterController.IsEncounterComplete())
            {
                GUILayout.Label("Status: COMPLETE");
            }
            else
            {
                GUILayout.Label($"Enemies Remaining: {encounterController.GetRemainingEnemyCount()}");
            }
        }
        else
        {
            GUILayout.Label("No encounter controller");
        }
        
        GUILayout.Space(10);
        GUILayout.Label("=== Controls ===");
        GUILayout.Label("WASD: Move");
        GUILayout.Label("Space: Dodge");
        GUILayout.Label("LMB: Light Attack");
        GUILayout.Label("RMB: Heavy Attack");
        GUILayout.Label("Q: Ground Slam");
        
        GUILayout.EndArea();
    }
}
