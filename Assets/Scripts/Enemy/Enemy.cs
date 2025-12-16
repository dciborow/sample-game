using UnityEngine;

/// <summary>
/// Simple enemy with health and basic AI
/// </summary>
public class Enemy : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 50f;
    private float currentHealth;
    
    [Header("AI")]
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float moveSpeed = 2f;
    public float attackCooldown = 2f;
    public float attackDamage = 10f;
    
    private Transform player;
    private float attackCooldownTimer;
    private bool isDead;
    private EncounterController encounterController;
    
    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        encounterController = FindObjectOfType<EncounterController>();
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
        
        // Simple damage to player (would need PlayerHealth component)
        var playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
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
        isDead = true;
        
        // Notify encounter controller if one exists
        if (encounterController != null)
        {
            encounterController.OnEnemyDefeated(this);
        }
        
        // Simple death - destroy after delay
        Destroy(gameObject, 2f);
    }
    
    public float GetHealthPercent()
    {
        return currentHealth / maxHealth;
    }
}
