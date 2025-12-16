using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Example manager that handles enemy death events
/// Demonstrates how to use the Enemy death contract
/// </summary>
public class EnemyManager : MonoBehaviour
{
    /// <summary>
    /// Register an enemy with this manager
    /// </summary>
    /// <param name="enemy">The enemy to register</param>
    public void RegisterEnemy(Enemy enemy)
    {
        if (enemy != null)
        {
            // Create a UnityAction to allow proper listener removal
            UnityAction deathHandler = null;
            deathHandler = () => 
            {
                OnEnemyDeath(enemy);
                // Remove listener after death to prevent memory leak
                enemy.onDeath.RemoveListener(deathHandler);
            };
            
            // Subscribe to the enemy's death event
            enemy.onDeath.AddListener(deathHandler);
        }
    }
    
    /// <summary>
    /// Handle enemy death
    /// </summary>
    /// <param name="enemy">The enemy that died</param>
    private void OnEnemyDeath(Enemy enemy)
    {
        Debug.Log($"Enemy {enemy.name} died!");
        
        // Default behavior: destroy after delay
        Destroy(enemy.gameObject, 2f);
        
        // Future systems can extend this:
        // - Update encounter counter
        // - Spawn loot
        // - Trigger boss phase
        // - Load next scene
        // Note: Always remove event listeners after handling to prevent memory leaks
    }
}
