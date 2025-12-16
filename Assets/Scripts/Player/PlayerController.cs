using UnityEngine;

/// <summary>
/// Player controller with top-down movement for isometric view
/// </summary>
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AbilitySystem))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    
    [Header("Dodge")]
    public float dodgeDistance = 3f;
    public float dodgeDuration = 0.3f;
    public float dodgeCooldown = 1f;
    
    [Header("Input Buffering")]
    public float inputBufferWindow = 0.3f;
    
    private CharacterController characterController;
    private AbilitySystem abilitySystem;
    private Vector3 moveDirection;
    private Vector3 facingDirection = Vector3.forward;
    
    // Player locomotion state
    private PlayerState playerState = PlayerState.Idle;
    
    // Dodge state
    private float dodgeTimer;
    private Vector3 dodgeDirection;
    private float dodgeCooldownTimer;
    
    // Input buffering
    private struct BufferedInput
    {
        public int abilityIndex;
        public float timestamp;
        public bool isValid;
    }
    private BufferedInput bufferedInput;
    
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        abilitySystem = GetComponent<AbilitySystem>();
        facingDirection = transform.forward;
    }
    
    /// <summary>
    /// Single source of truth for "can the player act?"
    /// </summary>
    private bool CanAct()
    {
        return playerState == PlayerState.Idle;
    }
    
    void Update()
    {
        UpdatePlayerState();
        HandleInput();
        UpdateDodge();
        UpdateMovement();
        UpdateInputBuffer();
    }
    
    private void UpdatePlayerState()
    {
        // Transition from Acting back to Idle when ability finishes
        if (playerState == PlayerState.Acting && abilitySystem.currentState == AbilityState.Idle)
        {
            playerState = PlayerState.Idle;
        }
    }
    
    private void HandleInput()
    {
        // Movement input
        if (CanAct())
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            moveDirection = new Vector3(horizontal, 0, vertical).normalized;
            
            // Update facing direction when moving
            if (moveDirection.magnitude > 0.1f)
            {
                facingDirection = moveDirection;
            }
        }
        else
        {
            moveDirection = Vector3.zero;
        }
        
        // Dodge input
        if (Input.GetKeyDown(KeyCode.Space) && CanAct() && dodgeCooldownTimer <= 0)
        {
            StartDodge();
        }
        
        // Ability inputs
        if (Input.GetKeyDown(KeyCode.Mouse0)) // Light attack
        {
            TryUseAbility(0);
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1)) // Heavy attack
        {
            TryUseAbility(1);
        }
        else if (Input.GetKeyDown(KeyCode.Q)) // Area ability
        {
            TryUseAbility(2);
        }
    }
    
    private void TryUseAbility(int abilityIndex)
    {
        // Check if we can use ability immediately
        if (CanAct())
        {
            Vector3 targetPos = transform.position + facingDirection * 5f;
            
            // For ground-targeted abilities, use mouse position
            if (abilityIndex == 2) // Area ability
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100f))
                {
                    targetPos = hit.point;
                }
            }
            
            if (abilitySystem.TryUseAbility(abilityIndex, targetPos, facingDirection))
            {
                // Ability started - transition to Acting state
                playerState = PlayerState.Acting;
            }
        }
        else if (playerState == PlayerState.Acting && abilitySystem.currentState == AbilityState.Recovery)
        {
            // Buffer the input during recovery phase
            bufferedInput = new BufferedInput
            {
                abilityIndex = abilityIndex,
                timestamp = Time.time,
                isValid = true
            };
        }
    }
    
    private void UpdateInputBuffer()
    {
        if (!bufferedInput.isValid)
            return;
            
        // Check if buffer has expired
        if (Time.time - bufferedInput.timestamp > inputBufferWindow)
        {
            bufferedInput.isValid = false;
            return;
        }
        
        // Try to execute buffered input when able
        if (CanAct())
        {
            TryUseAbility(bufferedInput.abilityIndex);
            bufferedInput.isValid = false;
        }
    }
    
    private void StartDodge()
    {
        playerState = PlayerState.Dodging;
        dodgeTimer = dodgeDuration;
        dodgeCooldownTimer = dodgeCooldown;
        dodgeDirection = moveDirection.magnitude > 0.1f ? moveDirection : facingDirection;
    }
    
    private void UpdateDodge()
    {
        // Update cooldown
        if (dodgeCooldownTimer > 0)
        {
            dodgeCooldownTimer -= Time.deltaTime;
        }
        
        if (playerState != PlayerState.Dodging)
            return;
            
        dodgeTimer -= Time.deltaTime;
        
        if (dodgeTimer <= 0)
        {
            playerState = PlayerState.Idle;
            return;
        }
        
        // Move during dodge
        float dodgeSpeed = dodgeDistance / dodgeDuration;
        Vector3 dodgeMovement = dodgeDirection * dodgeSpeed * Time.deltaTime;
        characterController.Move(dodgeMovement);
    }
    
    private void UpdateMovement()
    {
        if (!CanAct())
            return;
            
        // Apply movement
        Vector3 movement = moveDirection * moveSpeed * Time.deltaTime;
        characterController.Move(movement);
        
        // Rotate to face direction
        if (facingDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(facingDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    
    public bool IsDodgeReady()
    {
        return dodgeCooldownTimer <= 0;
    }
    
    public float GetDodgeCooldownPercent()
    {
        return dodgeCooldownTimer / dodgeCooldown;
    }
    
    /// <summary>
    /// Get current player state for debugging/UI
    /// </summary>
    public PlayerState GetPlayerState()
    {
        return playerState;
    }
}

/// <summary>
/// Player locomotion state - answers "can I act?"
/// </summary>
public enum PlayerState
{
    Idle,      // Ready to act
    Dodging,   // Performing dodge
    Acting     // Executing ability
}
