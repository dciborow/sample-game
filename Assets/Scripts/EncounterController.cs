using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

/// <summary>
/// Manages encounter state and tracks enemy completion
/// Emits event when all enemies are defeated
/// </summary>
public class EncounterController : MonoBehaviour
{
    [Header("Encounter Events")]
    public UnityEvent OnEncounterComplete;
    
    private readonly HashSet<Enemy> activeEnemies = new HashSet<Enemy>();
    private bool encounterCompleted = false;
    private bool hasRegisteredEnemies = false;
    
    void Awake()
    {
        // Auto-discover and register all enemies in the scene
        // Using Awake() ensures this runs before enemy Start() methods
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (Enemy enemy in enemies)
        {
            RegisterEnemy(enemy);
            enemy.SetEncounterController(this);
        }
    }


    
    /// <summary>
    /// Register an enemy to be tracked in this encounter
    /// </summary>
    public void RegisterEnemy(Enemy enemy)
    {
        if (enemy != null && !encounterCompleted)
        {
            activeEnemies.Add(enemy);
            hasRegisteredEnemies = true;
        }
    }
    
    /// <summary>
    /// Notify that an enemy has been defeated
    /// </summary>
    public void OnEnemyDefeated(Enemy enemy)
    {
        if (encounterCompleted)
            return;
        
        // Only process if the enemy was actually registered
        bool wasRemoved = activeEnemies.Remove(enemy);
        if (!wasRemoved)
        {
            Debug.LogWarning($"OnEnemyDefeated called for unregistered enemy: {enemy.name}");
            return;
        }
        
        // Check if encounter is complete (only if we've registered enemies)
        if (hasRegisteredEnemies && activeEnemies.Count == 0)
        {
            CompleteEncounter();
        }
    }
    
    private void CompleteEncounter()
    {
        encounterCompleted = true;
        Debug.Log("Encounter Complete!");
        OnEncounterComplete?.Invoke();
    }
    
    /// <summary>
    /// Get the number of remaining enemies
    /// </summary>
    public int GetRemainingEnemyCount()
    {
        return activeEnemies.Count;
    }
    
    /// <summary>
    /// Check if the encounter is complete
    /// </summary>
    public bool IsEncounterComplete()
    {
        return encounterCompleted;
    }
}
