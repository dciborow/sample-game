using UnityEngine;

/// <summary>
/// Manages player health
/// </summary>
public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Health")]
    public float maxHealth = 100f;
    private float currentHealth;
    
    void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(int amount, Vector3 hitPoint)
    {
        currentHealth -= (float)amount;
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }
    
    private void Die()
    {
        // Simple death handling
        Debug.Log("Player died!");
        // Trigger game flow event
        GameEvents.TriggerPlayerDeath();
    }
    
    public float GetHealthPercent()
    {
        return currentHealth / maxHealth;
    }
}
