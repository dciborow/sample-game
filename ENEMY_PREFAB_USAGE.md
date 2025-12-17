# Enemy Prefab - Usage Guide

## Overview
The Enemy prefab is a minimal enemy GameObject that can receive damage and be destroyed. It fulfills the requirements for a basic enemy with health and death functionality.

## Prefab Location
`Assets/Prefabs/Enemy.prefab`

## Components

### GameObject Structure
- **Name**: Enemy
- **Tag**: Enemy (required for damage detection)
- **Transform**: Standard transform component

### Visual Representation
- **Mesh**: Capsule primitive (2 units tall, 1 unit diameter)
- **Material**: Default Unity material (red color when created via editor script)

### Collider
- **Type**: CapsuleCollider
- **Configuration**:
  - Radius: 0.5
  - Height: 2
  - Center: (0, 0, 0)
  - Is Trigger: False (physical collider for ability detection)

### Enemy Component
The core component that manages health, damage, and death.

**Health Settings**:
- `maxHealth`: 50 HP (configurable)
- Health tracking is automatic

**AI Settings** (Disabled for minimal behavior):
- `detectionRange`: 0 (enemy won't detect player)
- `attackRange`: 0 (enemy won't attack)
- `moveSpeed`: 0 (enemy won't move)
- `attackCooldown`: 999 seconds (attacks effectively disabled)
- `attackDamage`: 0 (no damage even if attack triggers)

**Other Settings**:
- `isBoss`: false (regular enemy, not a boss)
- `onDeath`: UnityEvent that fires when enemy dies

## Features

### ✅ Health System
- Enemy starts with 50 health points
- Health can be reduced via the `TakeDamage(float damage)` method
- Health is automatically tracked by the Enemy component

### ✅ Damage Reception
- Player abilities that hit the enemy's collider will deal damage
- The EffectResolver system automatically processes ability hits and calls `TakeDamage()`
- Damage is applied when player abilities (hitboxes, areas) overlap with enemy collider

### ✅ Death System
- Enemy dies when health reaches zero
- Death is handled automatically by the Enemy component
- On death, the enemy:
  1. Sets `IsDead` property to true
  2. Emits `onDeath` UnityEvent
  3. Notifies EncounterController (if registered)
  4. Destroys itself after 2 seconds

### ✅ Event System
- `onDeath` UnityEvent allows external systems to react to death
- Compatible with EnemyManager for cleanup
- Compatible with EncounterController for encounter tracking
- See ENEMY_DEATH_USAGE.md for advanced event usage patterns

## Constraints Met

As per the issue requirements, this enemy prefab follows these constraints:

✅ **No AI beyond idle or simple facing**
- Detection range set to 0
- No movement toward player
- Enemy remains stationary

✅ **No movement logic required**
- Movement speed set to 0
- Enemy stays at spawn position

✅ **No attack logic**
- Attack range set to 0
- Attack damage set to 0
- Attack cooldown set to 999 seconds
- Enemy will not damage player

## Usage Examples

### 1. Basic Scene Setup
```csharp
// Place Enemy prefab in scene at desired location
// Enemy will automatically:
// - Have health
// - Receive damage from player abilities
// - Die when health reaches zero
// - Destroy itself 2 seconds after death
```

### 2. Spawning Enemies Programmatically
```csharp
// Load the enemy prefab
GameObject enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy");

// Instantiate at position
GameObject enemy = Instantiate(enemyPrefab, new Vector3(5, 1, 5), Quaternion.identity);

// Optionally register with EncounterController
EncounterController encounter = FindObjectOfType<EncounterController>();
if (encounter != null)
{
    Enemy enemyComponent = enemy.GetComponent<Enemy>();
    encounter.RegisterEnemy(enemyComponent);
    enemyComponent.SetEncounterController(encounter);
}
```

### 3. Using with EncounterController
```csharp
// EncounterController automatically discovers all enemies in the scene
// When all enemies die, it triggers OnEncounterComplete event
// The Enemy prefab works seamlessly with this system

// In your scene:
// 1. Place EncounterController GameObject
// 2. Place one or more Enemy prefabs
// 3. EncounterController will auto-register them in Awake()
```

### 4. Listening to Death Events
```csharp
public class CustomSystem : MonoBehaviour
{
    void Start()
    {
        // Find the enemy
        Enemy enemy = FindObjectOfType<Enemy>();
        
        // Subscribe to death event
        enemy.onDeath.AddListener(OnEnemyDeath);
    }
    
    void OnEnemyDeath()
    {
        Debug.Log("Enemy died! Award XP, spawn loot, etc.");
        // Your custom logic here
    }
}
```

## Testing the Enemy

### Manual Testing Steps
1. Open Unity Editor
2. Open any scene (MainScene, AdventureScene, or BossScene)
3. Drag the Enemy prefab from `Assets/Prefabs/Enemy.prefab` into the scene
4. Position the enemy near the player spawn point
5. Enter Play Mode
6. Move the player close to the enemy
7. Use player abilities (Left Mouse, Right Mouse, or Q) to attack the enemy
8. Observe:
   - Enemy takes damage (health decreases)
   - Enemy turns red or shows damage indication
   - After taking ~50 damage (depending on ability), enemy dies
   - Enemy GameObject is destroyed after 2 seconds

### Expected Behavior
- ✅ Enemy is visible in scene
- ✅ Enemy has a collider (can be hit by abilities)
- ✅ Enemy takes damage when hit by player abilities
- ✅ Enemy health decreases with each hit
- ✅ Enemy dies when health reaches zero
- ✅ Enemy disappears 2 seconds after death
- ✅ No scene transitions occur (as per requirements)

## Customization

You can customize the enemy by modifying these values in the Inspector:

### Health
- **maxHealth**: Change starting/maximum health (default: 50)

### Visual
- Change the mesh to a different primitive or custom model
- Modify material color or texture
- Add particle effects for damage or death

### Behavior
If you want to enable basic AI:
- Set `detectionRange` > 0 to enable player detection
- Set `moveSpeed` > 0 to allow movement
- Set `attackRange` > 0 and `attackDamage` > 0 to enable attacks
- Adjust `attackCooldown` for attack frequency

**Note**: The default configuration keeps these at 0 to meet the "minimal enemy" requirement.

## Integration with Existing Systems

### Player Abilities
The Enemy prefab works with the existing ability system:
- LightMelee.asset (15 damage)
- HeavyMelee.asset (35 damage)  
- GroundSlam.asset (25 damage)

The EffectResolver component automatically handles ability hits and applies damage.

### EncounterController
The Enemy prefab integrates with EncounterController for encounter management:
- Automatically registered when EncounterController is present
- Death notifications sent to controller
- Helps track encounter completion

### EnemyManager
The Enemy prefab can be registered with EnemyManager for centralized enemy management:
- Use `EnemyManager.RegisterEnemy(enemy)` to register
- Manager handles cleanup and memory management
- See ENEMY_DEATH_USAGE.md for patterns

## Technical Details

### Death Handling
The Enemy component includes:
- Race condition protection (prevents multiple death triggers)
- Automatic cleanup (destroys GameObject after 2 seconds)
- Event emission before destruction
- IsDead property for external queries

### Memory Management
- Death event listeners are automatically managed
- GameObject destruction is delayed to allow death animations/effects
- No memory leaks from event subscriptions

### Collision Detection
- Uses physical collider (non-trigger)
- Works with Unity's physics system
- Compatible with ability hitbox detection

## Troubleshooting

### Enemy doesn't take damage
1. Verify enemy has "Enemy" tag assigned
2. Check that EffectResolver is present in scene
3. Ensure player abilities are configured correctly
4. Verify collider is not set to trigger mode

### Enemy doesn't die
1. Check that damage values are non-zero in abilities
2. Verify Enemy component is enabled
3. Ensure health value is not set too high

### Death event doesn't fire
1. Verify event listeners are registered before enemy dies
2. Check that UnityEvent is not null
3. Ensure listener methods are public

## Related Documentation
- `ENEMY_DEATH_USAGE.md` - Comprehensive guide on enemy death event patterns
- `IMPLEMENTATION_SUMMARY.md` - Overall project architecture
- `Assets/Scripts/Enemy/Enemy.cs` - Enemy component source code
- `Assets/Editor/EnemyPrefabCreator.cs` - Editor script for creating prefabs

## Definition of Done - Verified ✅

This prefab meets all requirements from the issue:

✅ **Player can damage the enemy**
- Enemy has collider for hit detection
- TakeDamage method receives damage from abilities
- Works with existing ability system

✅ **Enemy disappears when health reaches zero**
- Die() method called at zero health
- GameObject destroyed after 2 second delay
- Automatic cleanup

✅ **No scene transitions**
- Death handling is local to enemy
- No scene loading triggered
- Event-based system allows controlled responses

✅ **Constraints followed**
- No AI (all AI values set to 0)
- No movement (speed = 0)
- No attacks (attack values = 0)
- Minimal implementation

## Summary
The Enemy prefab is a complete, minimal enemy implementation that provides:
- Health management
- Damage reception from player abilities  
- Death handling with events
- Automatic cleanup
- Integration with existing game systems

It meets all requirements without unnecessary complexity, following the principle of minimal implementation.
