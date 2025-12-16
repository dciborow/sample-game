using UnityEngine;

/// <summary>
/// Manages player health
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    private float currentHealth;
    
    void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        
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
        // Could reload scene, show game over, etc.
    }
    
    public float GetHealthPercent()
    {
        return currentHealth / maxHealth;
    }
}
