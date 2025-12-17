using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Simple enemy with health and basic AI
/// Emits death event for external systems to handle
/// </summary>
public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Health")]
    public float maxHealth = 50f;
    private float currentHealth;
    
    [Header("Boss Settings")]
    public bool isBoss = false;
    
    [Header("AI")]
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float moveSpeed = 2f;
    public float attackCooldown = 2f;
    public float attackDamage = 10f;
    
    [Header("Events")]
    public UnityEvent onDeath = new UnityEvent();
    
    private Transform player;
    private float attackCooldownTimer;
    private bool isDead;
    private EncounterController encounterController;
    
    /// <summary>
    /// Check if enemy is dead
    /// </summary>
    public bool IsDead => isDead;
    
    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }
    
    /// <summary>
    /// Set the encounter controller for this enemy
    /// </summary>
    public void SetEncounterController(EncounterController controller)
    {
        encounterController = controller;
    }
    
    void Update()
    {
        if (isDead || player == null)
            return;
            
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        
        // Attack cooldown
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;
        }
        
        // AI behavior
        if (distanceToPlayer <= detectionRange)
        {
            // Face player
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            directionToPlayer.y = 0;
            if (directionToPlayer.magnitude > 0.1f)
            {
                transform.rotation = Quaternion.LookRotation(directionToPlayer);
            }
            
            // Move towards player or attack
            if (distanceToPlayer > attackRange)
            {
                // Move towards player
                Vector3 movement = directionToPlayer * moveSpeed * Time.deltaTime;
                transform.position += movement;
            }
            else if (attackCooldownTimer <= 0)
            {
                // Attack
                AttackPlayer();
            }
        }
    }
    
    private void AttackPlayer()
    {
        attackCooldownTimer = attackCooldown;
        
        // Apply damage through the IDamageable interface
        var damageable = player.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(attackDamage);
        }
    }
    
    public void TakeDamage(float damage)
    {
        if (isDead)
            return;
            
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        // Prevent multiple death events from race conditions
        if (isDead)
            return;
            
        isDead = true;
        
        // Notify encounter controller if one exists
        if (encounterController != null)
        {
            encounterController.OnEnemyDefeated(this);
        }

        // Trigger boss defeated event if this is a boss
        if (isBoss)
        {
            GameEvents.TriggerBossDefeated();
        }
        
        // Simple death - destroy after delay
        Destroy(gameObject, 2f);
    }
    
    public float GetHealthPercent()
    {
        return currentHealth / maxHealth;
    }
}
