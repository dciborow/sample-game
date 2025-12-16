# System Architecture: 2.5D Isometric Action Combat

## Overview
This document describes the architecture of the minimal, composable, data-driven 2.5D isometric action combat system.

## Core Design Principles

### 1. Data-Driven Design
All abilities are defined as ScriptableObjects, allowing designers to create and tune abilities without writing code. This makes the system:
- **Composable**: Mix and match ability properties
- **Scalable**: Add new abilities easily
- **Maintainable**: Single source of truth for ability data
- **Designer-friendly**: No code changes required for balance

### 2. State Machine Pattern
The ability execution follows a clear state machine with phases:
- **Idle**: Ready to act
- **WindUp**: Telegraph/preparation
- **Active**: Effect execution
- **Recovery**: Cool-down before next action

### 3. Input Buffering
Inputs during recovery phase are buffered and executed when possible, providing:
- Responsive gameplay
- Smooth ability chaining
- Forgiveness for imperfect timing

## System Components

### Ability System

#### AbilityData (ScriptableObject)
Base class for all abilities with timing phases:
```csharp
- cooldown: Time before reuse
- windUpTime: Telegraph duration
- activeTime: Effect window
- recoveryTime: Post-effect lockout
- damage: Base damage value
- range: Maximum distance
- targetType: Self/Direction/GroundTarget
```

**Derived Types:**
- `MeleeAbilityData`: Cone-based melee attacks
- `AreaAbilityData`: Ground-targeted AoE

#### AbilitySystem (Component)
Manages ability slots, cooldowns, and state machine:
- Tracks ability cooldowns independently
- Executes state transitions
- Handles ability triggering
- Provides cooldown query API

**Key Methods:**
- `TryUseAbility()`: Attempt to execute ability
- `IsAbilityReady()`: Check if ability available
- `GetCooldownPercent()`: For UI display

### Player System

#### PlayerController (Component)
Main player control hub:
- Movement with CharacterController
- Dodge mechanic with cooldown
- Input handling and buffering
- Ability triggering
- Facing direction tracking

**Movement:**
- Top-down WASD control
- Character rotates to face movement direction
- Smooth rotation interpolation

**Dodge:**
- Quick movement burst
- Independent cooldown
- Uses movement direction or facing

**Input Buffering:**
- 0.3s buffer window (configurable)
- Executes when state allows
- Single buffered input (latest wins)

#### PlayerHealth (Component)
Simple health management:
- Current/max health tracking
- Damage/heal methods
- Death handling

#### IsometricCamera (Component)
Camera follows player at fixed angle:
- Configurable offset and angle
- Smooth follow with lerp
- Auto-finds player if not assigned

#### PlayerUI (Component)
Visual feedback for player state:
- Health bar
- Cooldown overlays for abilities
- Dodge cooldown indicator

### Enemy System

#### Enemy (Component)
Simple AI-driven enemy:
- Detection range for player awareness
- Attack range for melee combat
- Move speed for pursuit
- Attack cooldown
- Health system

**AI Behavior:**
1. Detect player in range
2. Face player
3. Move toward player if outside attack range
4. Attack when in range and off cooldown

### Utility Components

#### DebugDisplay (Component)
On-screen debug information:
- Player health percentage
- Current ability state
- All cooldown statuses
- Control reminders

#### SceneSetup (Editor Script)
Menu command to auto-setup demo scene:
- Creates ground plane
- Spawns player with all components
- Assigns abilities from assets
- Creates enemy
- Sets up isometric camera
- Adds arena walls

## Data Flow

### Ability Execution Flow
```
1. Input received (mouse/key)
2. PlayerController.TryUseAbility()
3. AbilitySystem checks:
   - Is state Idle?
   - Is ability not on cooldown?
4. If valid:
   - Create AbilityContext
   - Call ability.OnActivate()
   - Transition to WindUp state
   - Start cooldown timer
5. After windUpTime:
   - Transition to Active
   - Call ability.OnExecute()
   - Apply damage to targets
6. After activeTime:
   - Transition to Recovery
7. After recoveryTime:
   - Transition to Idle
   - Process buffered input if any
```

### Input Buffering Flow
```
1. Input during Recovery phase
2. Store in bufferedInput with timestamp
3. On each Update:
   - Check if buffer expired (>0.3s)
   - Check if state now Idle
   - If valid, execute buffered input
   - Clear buffer
```

### Dodge Flow
```
1. Space pressed while Idle and not dodging
2. Check dodge cooldown
3. If ready:
   - Set isDodging = true
   - Set dodge timer to duration
   - Start cooldown
   - Determine direction (input or facing)
4. Each frame while dodging:
   - Move at dodge speed
5. When timer expires:
   - Set isDodging = false
```

## Performance Considerations

### Optimization Strategies
1. **Physics Queries**: Use OverlapSphere for hit detection
   - Efficient spatial query
   - Layer masks can filter (future enhancement)

2. **Object Pooling**: Not implemented yet, but recommended for:
   - Visual effects
   - Damage numbers
   - Enemy spawning

3. **State Caching**: 
   - Components cached in Start()
   - No GetComponent calls in Update()

4. **Minimal Allocations**:
   - AbilityContext reused per ability
   - String formatting only in debug display

### Scalability
Current design supports:
- **Abilities**: Unlimited via ScriptableObjects
- **Enemies**: Multiple instances with individual AI
- **Players**: Single player (easily extended)

## Extension Points

### Adding New Ability Types
```csharp
[CreateAssetMenu(fileName = "NewAbilityType", menuName = "Abilities/Custom")]
public class CustomAbilityData : AbilityData
{
    public CustomParams customParams;
    
    public override void OnExecute(AbilityContext context)
    {
        // Custom logic here
    }
}
```

### Adding Animation
Hook into state transitions:
```csharp
// In AbilitySystem, add events:
public UnityEvent<AbilityData> OnAbilityWindUp;
public UnityEvent<AbilityData> OnAbilityActive;

// In animator controller:
void OnAbilityWindUp(AbilityData ability)
{
    animator.SetTrigger(ability.animationTrigger);
}
```

### Adding VFX
Similar to animation, use events or:
```csharp
// In ability OnExecute:
public GameObject vfxPrefab;

public override void OnExecute(AbilityContext context)
{
    if (vfxPrefab != null)
    {
        Instantiate(vfxPrefab, context.targetPosition, Quaternion.identity);
    }
    // ... damage logic
}
```

### Adding Status Effects
Extend AbilityData and damage system:
```csharp
public class StatusEffect : ScriptableObject
{
    public float duration;
    public float tickRate;
    public float damagePerTick;
}

public class AbilityData
{
    public List<StatusEffect> appliedEffects;
}
```

## Testing Strategy

### Manual Testing Checklist
- [ ] Player moves in all directions
- [ ] Player rotates to face movement direction
- [ ] Dodge works and has cooldown
- [ ] Light attack hits enemy in front
- [ ] Heavy attack has longer wind-up
- [ ] Ground slam damages in area
- [ ] Abilities have cooldowns
- [ ] Input buffering works (spam during recovery)
- [ ] Enemy detects and chases player
- [ ] Enemy attacks when in range
- [ ] Health systems work (player and enemy)
- [ ] UI displays cooldowns correctly

### Unit Test Opportunities
- Ability cooldown calculations
- State machine transitions
- Input buffer timing
- Damage calculations
- Range/angle checks for abilities

### Integration Tests
- Full ability execution flow
- Player vs Enemy combat
- Multiple enemy scenarios
- Ability chaining

## Known Limitations

1. **No Animation**: System is code-only, ready for animation integration
2. **No VFX**: Minimal visual feedback (can add particle effects)
3. **Simple AI**: Enemy has basic behavior (easily extended)
4. **Single Player**: No multiplayer support
5. **Legacy Input**: Uses old Input Manager (can upgrade to new Input System)
6. **No Camera Collision**: Camera can clip through walls
7. **No Terrain**: Flat plane only (heightmaps possible)

## Future Enhancements

### Phase 1 - Polish
- Add animation system
- Particle effects for abilities
- Sound effects
- Better visual feedback (hit effects, damage numbers)

### Phase 2 - Content
- More ability types (projectile, buff, debuff)
- More enemy types
- Boss enemy with phases
- Multiple arena layouts

### Phase 3 - Systems
- Combo system (ability chains)
- Resource system (mana/stamina)
- Progression (leveling, unlocks)
- Save/load system

### Phase 4 - Advanced
- New Input System integration
- Ability customization (modifiers)
- Procedural arena generation
- Multiplayer foundation

## File Structure
```
Assets/
├── Editor/
│   └── SceneSetup.cs              # Scene setup utility
├── Scripts/
│   ├── Abilities/
│   │   ├── AbilityData.cs         # Base ability SO
│   │   ├── AbilitySystem.cs       # Ability execution manager
│   │   ├── MeleeAbilityData.cs    # Melee attack type
│   │   ├── AreaAbilityData.cs     # AoE attack type
│   │   └── AreaIndicator.cs       # Visual indicator
│   ├── Player/
│   │   ├── PlayerController.cs    # Main player control
│   │   ├── PlayerHealth.cs        # Health management
│   │   ├── PlayerUI.cs            # UI display
│   │   └── IsometricCamera.cs     # Camera follow
│   ├── Enemy/
│   │   └── Enemy.cs               # Enemy AI and health
│   └── DebugDisplay.cs            # On-screen debug info
├── ScriptableObjects/
│   └── Abilities/
│       ├── LightMelee.asset       # Fast melee ability
│       ├── HeavyMelee.asset       # Slow powerful melee
│       └── GroundSlam.asset       # AoE ground attack
└── Scenes/
    └── MainScene.unity            # Demo arena scene
```

## Conclusion

This architecture provides a solid foundation for a 2.5D isometric action combat game with:
- Clean separation of concerns
- Data-driven ability design
- Extensible component system
- Responsive player controls
- Simple but functional AI

The minimal design makes it easy to understand, extend, and build upon while maintaining good performance and code quality.
